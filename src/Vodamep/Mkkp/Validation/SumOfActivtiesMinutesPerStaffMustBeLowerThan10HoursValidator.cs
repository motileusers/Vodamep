using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class SumOfActivtiesMinutesPerStaffMustBeLowerThan10HoursValidator : AbstractValidator<MkkpReport>
    {
        //10 hours max
        private const int maxNoOfMinutes = 10 * 60;

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
                            ctx.AddFailure(new ValidationFailure(nameof(MkkpReport.Activities), Validationmessages.MaxSumOfMinutesPerStaffMemberIs10Hours));

                        }
                    }

                });
        }
    }

}
