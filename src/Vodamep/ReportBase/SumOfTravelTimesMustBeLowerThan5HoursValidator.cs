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
                    var staffs = report.Staffs;
                    var travelTimes = report.TravelTimes;

                    var tavelTimesPerStaffId = travelTimes.GroupBy(y => y.StaffId)
                        .Select((group) => new { Key = group.Key, Items = group.ToList() });

                    foreach (var travelTimeStaffId in tavelTimesPerStaffId)
                    {
                        var travelTimesPerStaffIdAndDate = travelTimeStaffId.Items.GroupBy(z => z.DateD)
                            .Select(group => new { Date = group.Key, Items = group.ToList() });

                        foreach (var tt in travelTimesPerStaffIdAndDate)
                        {
                            var sumOfMinutes = tt.Items.Sum(x => x.Minutes);

                            if (sumOfMinutes > maxNoOfMinutes)
                            {
                                var staff = staffs.FirstOrDefault(x => x.Id == travelTimeStaffId.Key);

                                ctx.AddFailure(new ValidationFailure(propertyName, Validationmessages.MaxSumOfMinutesTravelTimesIs10Hours(staff.GetDisplayName() , tt.Date.ToShortDateString())));
                            }
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
