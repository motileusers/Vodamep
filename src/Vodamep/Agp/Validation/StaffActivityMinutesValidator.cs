using FluentValidation;
using FluentValidation.Results;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class StaffActivityMinutesValidator : AbstractValidator<StaffActivity>
    {
        public StaffActivityMinutesValidator(string propertyName, string id)
        {
            this.RuleFor(x => x.Minutes)
                .GreaterThan(0)
                .WithMessage(x => Validationmessages.StaffActivityMinutesMustBeGreater0(propertyName, id));

            this.RuleFor(x => x)
                .Custom((activity, ctx) =>
                {
                    var activityMinutes = activity.Minutes;

                    if (activityMinutes > 0 && activityMinutes % 5 != 0)
                    {
                        ctx.AddFailure(new ValidationFailure(propertyName, Validationmessages.ReportBaseStepWidthWrong(propertyName, id, 5)));
                    }
                });
        }
    }
}