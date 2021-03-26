using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class LeavingValidator : AbstractValidator<Leaving>
    {
        public LeavingValidator()
        {
            this.RuleFor(x => x.LeavingReason).NotEmpty();
        }
    }
}