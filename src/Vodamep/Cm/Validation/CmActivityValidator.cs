using FluentValidation;
using Vodamep.Cm.Model;

namespace Vodamep.Cm.Validation
{
    internal class CmActivityValidator : AbstractValidator<Activity>
    {
        public CmActivityValidator()
        {
            //this.RuleFor(x => x).SetValidator(new ActivityMinutesValidator(1, 10000));
        }
    }
}