using FluentValidation;
using FluentValidation.Results;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class SumOfActivtiesMinutesPerStaffMustBeLowerThan10HoursValidator : AbstractValidator<AgpReport>
    {
        //10 hours max
        private const int maxNoOfMinutes = 12 * 60;

        public SumOfActivtiesMinutesPerStaffMustBeLowerThan10HoursValidator()
        {
            this.RuleFor(x => x.Activities)
                .Custom((activities, ctx) =>
                {
                    var activitiesByStaffId = activities.GroupBy(y => y.StaffId)
                        .Select( (group) => new { Key = group.Key, Items = group.ToList() });

                    foreach (var activityByStaffId in activitiesByStaffId)
                    {
                        var sumOfMinutes = activityByStaffId.Items.Sum(x => x.Minutes);

                        if (sumOfMinutes > maxNoOfMinutes)
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(AgpReport.Activities), Validationmessages.MaxSumOfMinutesPerStaffMemberIs12Hours));

                        }
                    }

                });
        }
    }

}
