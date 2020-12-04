using System;
using FluentValidation;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
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

            this.RuleFor(x => x.StaffId).NotEmpty();

            this.RuleFor(x => x.PersonId).NotEmpty();
            this.RuleFor(x => x.PersonId).Empty();

            this.RuleFor(x => x.Entries).NotEmpty();
            this.RuleForEach(x => x.Entries).NotEqual(ActivityType.UndefinedActivity);
        }
    }
}