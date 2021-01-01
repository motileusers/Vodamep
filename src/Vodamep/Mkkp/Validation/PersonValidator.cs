using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.Data;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();
            this.RuleFor(x => x.Referrer).NotEmpty();
            this.RuleFor(x => x.HospitalDoctor).NotEmpty();
            this.RuleFor(x => x.LocalDoctor).NotEmpty();
            
            // Änderung 5.11.2018, LH
            var r = new Regex(@"^[\p{L}][-\p{L}. ]*[\p{L}.]$");
            this.RuleFor(x => x.FamilyName).Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName));
            this.RuleFor(x => x.GivenName).Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName));
            this.RuleFor(x => x.HospitalDoctor).Matches(r).Unless(x => string.IsNullOrEmpty(x.HospitalDoctor));
            this.RuleFor(x => x.LocalDoctor).Matches(r).Unless(x => string.IsNullOrEmpty(x.LocalDoctor));

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

            this.RuleFor(x => x.OtherReferrer).NotEmpty()
                .When(y => y.Referrer == Referrer.OtherReferrer)
                .WithMessage(Validationmessages.ReferrerIsOtherRefererreThenOtherReferrerMustBeSet);
         
            this.RuleFor(x => x.Diagnoses).NotEmpty().WithMessage(Validationmessages.AtLeastOneDiagnosisGroup);
            this.Include(new DiagnosisGroupIsUniqueValidator());
        }

    }
}