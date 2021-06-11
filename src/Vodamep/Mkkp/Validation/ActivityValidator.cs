using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Data.Mkkp;
using Vodamep.Mkkp.Model;
using Vodamep.ReportBase;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class ActivityValidator : AbstractValidator<Activity>
    {
        private readonly MkkpReport mkkpReport;

        public ActivityValidator(MkkpReport report, DateTime from, DateTime to)
        {
            var displayNameResolver = new MkkpDisplayNameResolver();

            this.mkkpReport = report;

            this.RuleFor(x => x.Date).NotEmpty();

            this.RuleFor(x => x.Date)
                .SetValidator(new TimestampWithOutTimeValidator())
                .Unless(x => x.Date == null)
                .WithMessage(x => Validationmessages.ReportBaseDateMustNotHaveTime(this.GetClient(x.PersonId)));


            if (from != DateTime.MinValue)
            {
                this.RuleFor(x => x.DateD).GreaterThanOrEqualTo(from).Unless(x => x.Date == null);
            }
            if (to > from)
            {
                this.RuleFor(x => x.DateD).LessThanOrEqualTo(to).Unless(x => x.Date == null);
            }

            this.RuleFor(x => x.StaffId).NotEmpty();

            this.RuleFor(x => x.PersonId).NotEmpty();

            this.RuleFor(x => x.Entries).NotEmpty();
            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                var entries = x.Entries;

                var doubledQuery = entries.GroupBy(y => y)
                    .Where(y => y.Count() > 1)
                    .Select(group => group.Key);

                if (doubledQuery.Any())
                {
                    ctx.AddFailure(new ValidationFailure(nameof(Activity.Minutes), Validationmessages.WithinAnActivityThereAreNoDoubledActivityTypesAllowed(this.GetClient(x.PersonId))));
                }

                if (entries.Any(y => y == ActivityType.AccompanyingWithContact) &&
                    entries.Any(y => y == ActivityType.AccompanyingWithoutContact))
                {
                    ctx.AddFailure(new ValidationFailure(nameof(Activity.Entries),
                        Validationmessages.WithinAnActivityTheValuesAreNotAllowedInCombination
                        (this.GetClient(x.PersonId), ActivityTypeProvider.Instance.GetValue(ActivityType.AccompanyingWithContact.ToString())
                            , ActivityTypeProvider.Instance.GetValue(ActivityType.AccompanyingWithoutContact.ToString())
                        )));
                }

            });

            this.RuleFor(x => x).SetValidator(x => new ActivityMinutesValidator(displayNameResolver.GetDisplayName(nameof(x.Minutes)), this.GetClient(x.PersonId)));

            //this.RuleFor(x => x.Minutes).GreaterThan(0);
            //this.RuleFor(x => x)
            //    .Custom((activity, ctx) =>
            //    {
            //        var activityMinutes = activity.Minutes;

            //        if (activityMinutes > 0 && activityMinutes % 5 != 0)
            //        {
            //            var client = this.GetClient(activity.PersonId);
            //            ctx.AddFailure(new ValidationFailure(nameof(Activity.Minutes), Validationmessages.ReportBaseStepWidthWrong(this.GetClient(client), 5)));
            //        }
            //    });

            this.RuleFor(x => x.PlaceOfAction).NotEmpty();

        }

        private string GetClient(string id)
        {
            string client = id;

            var person = this.mkkpReport.Persons.FirstOrDefault(p => p.Id == id);

            if (person != null)
            {
                client = $"{person.GivenName} {person.FamilyName}";
            }

            return client;
        }
    }
}
