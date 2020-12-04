using FluentValidation;
using Vodamep.Agp.Model;
using Vodamep.Data;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
         
            this.Include(new PersonBirthdayValidator());
           
            this.RuleFor(x => x.Insurance).NotEmpty();
            this.RuleFor(x => x.Insurance).SetValidator(new CodeValidator<InsuranceCodeProvider>());

            this.RuleFor(x => x.CareAllowance).NotEmpty();

            this.RuleFor(x => x.Postcode).NotEmpty();
            this.RuleFor(x => x.City).NotEmpty();
            this.RuleFor(x => $"{x.Postcode} {x.City}")
                .SetValidator(new CodeValidator<Postcode_CityProvider>())
                .Unless(x => string.IsNullOrEmpty(x.City) || string.IsNullOrEmpty(x.Postcode))
                .WithMessage(Validationmessages.InvalidPostCode_City);

            this.RuleFor(x => x.Gender).NotEmpty();


        }
    }
}