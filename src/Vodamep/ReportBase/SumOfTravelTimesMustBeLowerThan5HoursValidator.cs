using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.ValidationBase;

namespace Vodamep.ReportBase
{
    internal class SumOfTravelTimesMustBeLowerThan5HoursValidator : AbstractValidator<ITravelTimeReport>
    {
        
        //10 hours max
        private const int maxNoOfMinutes = 5 * 60;

        public SumOfTravelTimesMustBeLowerThan5HoursValidator(string propertyName)
        {
          
            this.RuleFor(x => x)
                .Custom((report, ctx) =>
                {
                    foreach (var staff in report.Staffs)
                    {
                        var travelTimesPerStaffMember = report.TravelTimes.Where(t => t.StaffId == staff.Id);

                        var sumOfMinutes = travelTimesPerStaffMember.Sum(x => x.Minutes);

                        if (sumOfMinutes > maxNoOfMinutes)
                        {
                            ctx.AddFailure(new ValidationFailure(propertyName, Validationmessages.MaxSumOfMinutesTravelTimesIs10Hours(this.GetStaff(staff.Id, report))));
                        }
                    }
                });
        }

        private string GetStaff(string id, ITravelTimeReport report)
        {
            string staffName = id;

            var staff = report.Staffs.FirstOrDefault(p => p.Id == id);

            if (staff != null)
            {
                staffName = staff.GetDisplayName();
            }

            return staffName;
        }
    }
}
