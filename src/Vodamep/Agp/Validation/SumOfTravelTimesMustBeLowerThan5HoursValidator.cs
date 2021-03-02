using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class SumOfTravelTimesMustBeLowerThan5HoursValidator : AbstractValidator<AgpReport>
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
                        ctx.AddFailure(new ValidationFailure(nameof(AgpReport.Activities), Validationmessages.MaxSumOfMinutesTravelTimesIs10Hours));

                    }
                });
        }
    }
}
