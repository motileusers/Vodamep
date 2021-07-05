using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Agp.Model;
using Vodamep.ReportBase;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator(DateTime from, DateTime to)
        {
            var displayNameResolver = new AgpDisplayNameResolver();

            this.RuleFor(x => x.PersonId).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmptyWithParentProperty(displayNameResolver.GetDisplayName(nameof(x.PersonId)), displayNameResolver.GetDisplayName(nameof(Activity))));

            this.RuleFor(x => x.StaffId).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.StaffId)), displayNameResolver.GetDisplayName(nameof(Activity)), x.PersonId));
            this.RuleFor(x => x.PlaceOfAction).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.PlaceOfAction)), displayNameResolver.GetDisplayName(nameof(Activity)), x.PersonId));
            this.RuleFor(x => x.Entries).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Entries)), displayNameResolver.GetDisplayName(nameof(Activity)), x.PersonId));
            this.RuleFor(x => x.Date).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Date)), displayNameResolver.GetDisplayName(nameof(Activity)), x.PersonId));

            var name = displayNameResolver.GetDisplayName(nameof(Activity.Date));

            this.RuleFor(x => x.DateD)
                .SetValidator(x => new DateTimeValidator(displayNameResolver.GetDisplayName(nameof(x.Date)),
                    x.PersonId, from, to, x.Date));

            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                var entries = x.Entries;

                var query = entries.GroupBy(y => y)
                    .Where(activityTypes => activityTypes.Count() > 1)
                    .Select(group => group.Key);

                if (query.Any())
                {
                    ctx.AddFailure(new ValidationFailure(nameof(Activity.Minutes), Validationmessages.WithinAnActivityThereAreNoDoubledActivityTypesAllowed(x.PersonId)));
                }

            });

            this.RuleFor(x => x.PlaceOfAction).NotEmpty();

            //this.RuleFor(x => x.Minutes).GreaterThan(0);
            //this.RuleFor(x => x.Minutes)
            //    .Custom((minute, ctx) =>
            //    {
            //        if (minute > 0 && minute % 5 != 0)
            //        {
            //            ctx.AddFailure(new ValidationFailure(nameof(Activity.Minutes), Validationmessages.ReportBaseStepWidthWrong));
            //        }
            //    });

            this.RuleFor(x => x).SetValidator(x => new ActivityMinutesValidator(displayNameResolver.GetDisplayName(nameof(Activity.Minutes)), x.PersonId));

        }
    }
}