using FluentValidation;
using System;
using Google.Protobuf.WellKnownTypes;
using Vodamep.Hkpv.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Hkpv.Validation
{
    internal class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator(DateTime from, DateTime to)
        {
            this.RuleFor(x => x.Date).NotEmpty();
            this.RuleFor(x => x.Date).SetValidator(new TimestampWithOutTimeValidator<Activity, Timestamp>()).Unless(x => x.Date == null);

            if (from != DateTime.MinValue)
            {
                this.RuleFor(x => x.DateD).GreaterThanOrEqualTo(from).Unless(x => x.Date == null);
            }
            if (to > from)
            {
                this.RuleFor(x => x.DateD).LessThanOrEqualTo(to).Unless(x => x.Date == null);
            }

            this.RuleFor(x => x.StaffId).NotEmpty();

            this.RuleFor(x => x.PersonId).NotEmpty().Unless(x => x.WithoutPersonId());
            this.RuleFor(x => x.PersonId).Empty().Unless(x => x.RequiresPersonId());


            this.RuleFor(x => x.Entries).NotEmpty();
            this.RuleForEach(x => x.Entries).NotEqual(ActivityType.UndefinedActivity);
        }
    }

}
