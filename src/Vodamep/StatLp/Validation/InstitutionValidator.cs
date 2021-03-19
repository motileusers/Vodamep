using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class InstitutionValidator : AbstractValidator<Institution>
    {
        public InstitutionValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty();

            //Einrichtungsnummer: Numerisch, genau 4 Zeichen, nicht 0000
            var r = new Regex(@"^(?<!\d)(?!0000)\d{4}(?!\d)");
            this.RuleFor(x => x.Id).Matches(r).Unless(x => string.IsNullOrEmpty(x.Id)).WithMessage(Validationmessages.InvalidInstitutionNumber);
        }
    }
}