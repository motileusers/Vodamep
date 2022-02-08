using System.Text.RegularExpressions;
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
            // Änderung 5.11.2018, LH
            var r = new Regex(@"^[\p{L}][-\p{L}. ]*[\p{L}.]$");
            this.RuleFor(x => x.HospitalDoctor).Matches(r).Unless(x => string.IsNullOrEmpty(x.HospitalDoctor));
            this.RuleFor(x => x.LocalDoctor).Matches(r).Unless(x => string.IsNullOrEmpty(x.LocalDoctor));

            this.Include(new PersonBirthdayValidator());
           
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
                .WithMessage(x => Validationmessages.ReportBaseReferrerIsOtherRefererrerThenOtherReferrerMustBeSet (x.GetDisplayName()));

            this.RuleFor(x => x.Diagnoses).NotEmpty().WithMessage(x => Validationmessages.AtLeastOneDiagnosisGroup(x.GetDisplayName()));

            this.Include(new DiagnosisGroupIsUniqueValidator());
        }
    }
}