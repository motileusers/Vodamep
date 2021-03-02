using FluentValidation;
using Vodamep.Mkkp.Model;

namespace Vodamep.Mkkp.Validation
{
    internal class InstitutionValidator : AbstractValidator<Institution>
    {
        public InstitutionValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty();

        }
    }
}