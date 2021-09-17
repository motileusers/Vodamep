using FluentValidation;
using FluentValidation.Results;
using Vodamep.ValidationBase;

namespace Vodamep.ReportBase
{
    internal class ActivityMinutesValidator : AbstractValidator<IMinutesActivity>
    {
        public ActivityMinutesValidator(string propertyName, string client)
        {
            this.RuleFor(x => x.Minutes)
                .GreaterThan(0)
                .WithMessage(x => Validationmessages.ReportBaseMinutesMustBeGreater0(propertyName, client));

            this.RuleFor(x => x)
                .Custom((activity, ctx) =>
                {
                    var activityMinutes = activity.Minutes;

                    if (activityMinutes > 0 && activityMinutes % 5 != 0)
                    {
                        ctx.AddFailure(new ValidationFailure(propertyName, Validationmessages.ReportBaseStepWidthWrong(propertyName, client, 5)));
                    }
                });
        }
    }
}