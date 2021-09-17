using FluentValidation;
using Vodamep.Mohi.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mohi.Validation
{
    internal class MohiActivityValidator : AbstractValidator<Activity>
    {
        public MohiActivityValidator()
        {
            this.RuleFor(x => x).SetValidator(x => new PersonActivityTimeValidator(0.25f, 10000));
            this.RuleFor(x => x).SetValidator(x => new ActivityStepLengthValidator(0.25f));
        }
    }
}