using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class OnlyOneTravelTimesEntryPerStaffMemberAndDayValidator : AbstractValidator<AgpReport>
    {
        public OnlyOneTravelTimesEntryPerStaffMemberAndDayValidator()
        {
            this.RuleFor(x => x.TravelTimes)
                .Custom((travelTimes, ctx) =>

                {
                    var travelTimesPerStaffId = travelTimes.GroupBy(y => y.StaffId)
                        .Select((group) => new { Key = group.Key, Items = group.ToList() });

                    foreach (var travelTimePerStaffId in travelTimesPerStaffId)
                    {
                        var travelTimesPerStaffIdAndDates = travelTimePerStaffId.Items.GroupBy(y => y.DateD).Select((group) => new { Key = group.Key, Items = group.ToList() }).Where(z => z.Items.Count > 1);

                        foreach (var travelTimesPerStaffIdAndDate in travelTimesPerStaffIdAndDates)
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(AgpReport.TravelTimes), Validationmessages.OnlyOneTravelTimeEntryPerStaffMemberAndDay));
                        }
                    }
                });
        }
    }
}
