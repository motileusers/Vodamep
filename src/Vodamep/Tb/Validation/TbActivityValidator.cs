using FluentValidation;
using Vodamep.Tb.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Tb.Validation
{
    internal class TbActivityValidator : AbstractValidator<Activity>
    {
        public TbActivityValidator()
        {
            this.RuleFor(x => x).SetValidator(x => new ActivityTimeValidator(0.25f, 10000));
            this.RuleFor(x => x).SetValidator(x => new ActivityStepLengthValidator(0.25f));
        }
    }
}