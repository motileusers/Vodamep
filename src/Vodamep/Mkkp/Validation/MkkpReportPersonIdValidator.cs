using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class MkkpReportPersonIdValidator : AbstractValidator<MkkpReport>
    {
        public MkkpReportPersonIdValidator()
        {
            MkkpDisplayNameResolver displayNameResolver = new MkkpDisplayNameResolver();

            this.RuleFor(x => x.Persons)
                .Custom((list, ctx) =>
                {
                    foreach (var id in list.Select(x => x.Id).OrderBy(x => x).GroupBy(x => x).Where(x => x.Count() > 1))
                    {
                        var item = list.Where(x => x.Id == id.Key).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(MkkpReport.Persons)}[{index}]", Validationmessages.ReportBaseIdIsNotUnique(id.Key)));
                    }
                });

            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: new { x.Staffs, x.Activities, x.Consultations }
            this.RuleFor(x => new Tuple<IList<Person>, IEnumerable<Activity>>(x.Persons, x.Activities))
                .Custom((a, ctx) =>
                {
                    var persons = a.Item1;
                    var activities = a.Item2;

                    var idPersons = persons.Select(x => x.Id).Distinct().ToArray();
                    var idPersonActivities = activities.Select(x => x.PersonId).Distinct().ToArray();

                    foreach (var id in idPersons.Except(idPersonActivities))
                    {
                        var item = persons.FirstOrDefault(x => x.Id == id);
                        if (item == null)
                            continue;

                        var index = persons.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure(nameof(Staff), Validationmessages.ReportBaseWithoutActivity(displayNameResolver.GetDisplayName(nameof(Person)), item.GetDisplayName())));
                    }

                    foreach (var activity in activities)
                    {
                        if (!idPersons.Contains(activity.PersonId))
                            ctx.AddFailure(new ValidationFailure(nameof(Activity), Validationmessages.ReportBaseActivityWithoutPerson(activity.Id, activity.PersonId)));
                    }

                });
        }
    }
}