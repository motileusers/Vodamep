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
        public ActivityValidator(MkkpReport report, DateTime from, DateTime to)
        {
            var displayNameResolver = new MkkpDisplayNameResolver();

            this.RuleFor(x => x.PersonId).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmptyWithParentProperty(displayNameResolver.GetDisplayName(nameof(x.PersonId)), displayNameResolver.GetDisplayName(nameof(Activity))));
            this.RuleFor(x => x.StaffId).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.StaffId)), displayNameResolver.GetDisplayName(nameof(Activity)), report.GetClient(x.PersonId)));
            this.RuleFor(x => x.PlaceOfAction).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.PlaceOfAction)), displayNameResolver.GetDisplayName(nameof(Activity)), report.GetClient(x.PersonId)));
            this.RuleFor(x => x.Entries).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Entries)), displayNameResolver.GetDisplayName(nameof(Activity)), report.GetClient(x.PersonId)));
            this.RuleFor(x => x.Date).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Date)), displayNameResolver.GetDisplayName(nameof(Activity)), report.GetClient(x.PersonId)));

            this.RuleFor(x => x.DateD)
                .SetValidator(x => new DateTimeValidator(displayNameResolver.GetDisplayName(nameof(x.Date)),
                    report.GetClient(x.PersonId), report.GetStaffName(x.StaffId), from, to, x.Date));

            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                var entries = x.Entries;

                var doubledQuery = entries.GroupBy(y => y)
                    .Where(y => y.Count() > 1)
                    .Select(group => group.Key);

                if (doubledQuery.Any())
                {
                    ctx.AddFailure(new ValidationFailure(nameof(Activity.Minutes), Validationmessages.WithinAnActivityThereAreNoDoubledActivityTypesAllowed(report.GetClient(x.PersonId))));
                }

                if (entries.Any(y => y == ActivityType.AccompanyingWithContact) &&
                    entries.Any(y => y == ActivityType.AccompanyingWithoutContact))
                {
                    ctx.AddFailure(new ValidationFailure(nameof(Activity.Entries),
                        Validationmessages.WithinAnActivityTheValuesAreNotAllowedInCombination
                        (report.GetClient(x.PersonId), ActivityTypeProvider.Instance.GetValue(ActivityType.AccompanyingWithContact.ToString())
                            , ActivityTypeProvider.Instance.GetValue(ActivityType.AccompanyingWithoutContact.ToString())
                        )));
                }

            });

            this.RuleFor(x => x).SetValidator(x => new ActivityMinutesValidator(displayNameResolver.GetDisplayName(nameof(x.Minutes)), report.GetClient(x.PersonId)));
        }
    }
}
