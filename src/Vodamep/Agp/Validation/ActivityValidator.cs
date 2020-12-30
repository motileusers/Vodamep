using System;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator(DateTime from, DateTime to)
        {
            this.RuleFor(x => x.Date).NotEmpty();
            this.RuleFor(x => x.Date).SetValidator(new TimestampWithOutTimeValidator()).Unless(x => x.Date == null);

            if (from != DateTime.MinValue)
            {
                this.RuleFor(x => x.DateD).GreaterThanOrEqualTo(from).Unless(x => x.Date == null);
            }
            if (to > from)
            {
                this.RuleFor(x => x.DateD).LessThanOrEqualTo(to).Unless(x => x.Date == null);
            }

            this.RuleFor(x => x.PersonId).NotEmpty();

            this.RuleFor(x => x.StaffId).NotEmpty();

            this.RuleFor(x => x.Entries).NotEmpty();
        
            this.RuleFor(x => x.Minutes).GreaterThan(0);
            this.RuleFor(x => x.Minutes)
                .Custom((minute, ctx) =>
                {
                    if (minute > 0 && minute % 5 != 0)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(Activity.Minutes), Validationmessages.MinutesHasToBeEnteredInFiveMinuteSteps));
                    }
                });
        }

    }
}