using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class SumOfTravelTimesMustBeLowerThan5HoursValidator : AbstractValidator<MkkpReport>
    {
        //10 hours max
        private const int maxNoOfMinutes = 5 * 60;

        public SumOfTravelTimesMustBeLowerThan5HoursValidator()
        {
            this.RuleFor(x => x.TravelTimes)
                .Custom((travelTimes, ctx) =>
                {
                    var sumOfMinutes = travelTimes.Sum(x => x.Minutes);

                    if (sumOfMinutes > maxNoOfMinutes)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(MkkpReport.TravelTimes), Validationmessages.MaxSumOfMinutesTravelTimesIs10Hours));

                    }
                });
        }
    }
}
