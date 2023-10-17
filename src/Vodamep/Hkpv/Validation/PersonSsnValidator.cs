using FluentValidation;
using Vodamep.Hkpv.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Hkpv.Validation
{
    internal class PersonSsnValidator : AbstractValidator<Person>
    {
        public PersonSsnValidator()   
        {
            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Ssn)
                .NotEmpty();

            RuleFor(x => x.Ssn)                
                .Must(x => SSNHelper.IsValid(x))
                .WithMessage(Validationmessages.SsnNotValid);            
        }

    }
}
