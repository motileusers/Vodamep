using FluentValidation;
using Vodamep.Agp.Model;

namespace Vodamep.Agp.Validation
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