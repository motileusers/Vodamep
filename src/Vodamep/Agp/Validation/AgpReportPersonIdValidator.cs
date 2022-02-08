using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class AgpReportPersonIdValidator : AbstractValidator<AgpReport>
    {
        public AgpReportPersonIdValidator()
        {
            var displayNameResolver = new AgpDisplayNameResolver();

            this.RuleFor(x => x).SetValidator(new UniqePersonIdValidator());

            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: new { x.Staffs, x.Activities, x.Consultations }
            this.RuleFor(x => new Tuple<IList<Person>, IEnumerable<Activity>>(x.Persons, x.Activities))
                .Custom((a, ctx) =>
                {
                    var persons = a.Item1;
                    var activities = a.Item2;

                    var idPersons = persons.Select(x => x.Id).Distinct().ToArray();
                    var idActivities = (
                        activities.Select(x => x.PersonId)
                    ).Distinct().ToArray();

                    foreach (var id in idPersons.Except(idActivities))
                    {
                        var item = persons.Where(x => x.Id == id).First();

                        AgpReport report = ctx.InstanceToValidate as AgpReport;

                        ctx.AddFailure(new ValidationFailure(nameof(Activity), Validationmessages.ReportBaseWithoutActivity(displayNameResolver.GetDisplayName(nameof(Person)), report.GetClient(id))));
                    }

                    foreach (var activity in activities)
                    {
                        if (!idPersons.Contains(activity.PersonId))
                            ctx.AddFailure(new ValidationFailure(nameof(Activity), Validationmessages.ReportBaseActivityWithoutPerson(activity.Id, activity.PersonId, activity.DateD)));
                    }
                });
        }
    }
}