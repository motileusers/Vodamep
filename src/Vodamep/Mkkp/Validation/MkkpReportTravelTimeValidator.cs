using FluentValidation;
using Vodamep.Mkkp.Model;
using Vodamep.ReportBase;

namespace Vodamep.Mkkp.Validation
{
    internal class MkkpReportTravelTimeValidator : AbstractValidator<MkkpReport>
    {
        public MkkpReportTravelTimeValidator()
        {
            this.RuleForEach(report => report.TravelTimes).SetValidator(r => new TravelTimeValidator(r));

            this.Include(new OnlyOneTravelTimesEntryPerStaffMemberAndDayValidator());
        }
    }
}