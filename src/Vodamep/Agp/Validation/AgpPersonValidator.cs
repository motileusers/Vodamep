using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.Agp.Model;
using Vodamep.Data;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class AgpPersonValidator : AbstractValidator<Person>
    {
        public AgpPersonValidator()
        {
            AgpDisplayNameResolver displayNameResolver = new AgpDisplayNameResolver();

            this.RuleFor(x => x.Gender).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(Person)), x.GetDisplayName()));
            this.RuleFor(x => $"{x.Postcode} {x.City}")
                                .SetValidator(new CodeValidator<Postcode_CityProvider>())
                                .Unless(x => string.IsNullOrEmpty(x.City) || string.IsNullOrEmpty(x.Postcode))
                                .WithMessage(Validationmessages.InvalidPostCode_City);
            this.RuleFor(x => x.CareAllowance).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(Person)), x.GetDisplayName()));
            this.RuleFor(x => x.Referrer).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(Person)), x.GetDisplayName()));
            this.RuleFor(x => x.OtherReferrer).NotEmpty()
                                    .When(y => y.Referrer == Referrer.OtherReferrer)
                                    .WithMessage(x => Validationmessages.ReportBaseReferrerIsOtherRefererrerThenOtherReferrerMustBeSet(x.GetDisplayName(), displayNameResolver.GetDisplayName(nameof(Person))));

            this.RuleFor(x => x.Postcode).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(Person)), x.GetDisplayName()));
            this.RuleFor(x => x.City).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(Person)), x.GetDisplayName()));


            this.RuleFor(x => x.Nationality).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.GetDisplayName()));
            this.RuleFor(x => x.Nationality)
                .Must((person, country) => CountryCodeProvider.Instance.IsValid(country))
                .Unless(x => string.IsNullOrWhiteSpace(x.Nationality))
                .WithMessage(x => Validationmessages.ReportBaseInvalidValue(x.GetDisplayName()));


            this.RuleFor(x => x.Insurance).SetValidator(new CodeValidator<InsuranceCodeProvider>()).Unless(x => string.IsNullOrEmpty(x.Insurance)).WithMessage(x => Validationmessages.ReportBaseInvalidCode(displayNameResolver.GetDisplayName(nameof(Person)), x.GetDisplayName()));
            this.RuleFor(x => x.Diagnoses).NotEmpty().WithMessage(x => Validationmessages.AtLeastOneDiagnosisGroup(x.GetDisplayName()));
            this.Include(new DiagnosisGroupIsUniqueValidator());

            // Änderung 5.11.2018, LH
            var r = new Regex(@"^[\p{L}][-\p{L}. ]*[\p{L}.]$");
            this.RuleFor(x => x.HospitalDoctor).Matches(r).Unless(x => string.IsNullOrEmpty(x.HospitalDoctor)).WithMessage(x => Validationmessages.ReportBasePropertyInvalidFormat(displayNameResolver.GetDisplayName(nameof(Person)), x.GetDisplayName()));
            this.RuleFor(x => x.LocalDoctor).Matches(r).Unless(x => string.IsNullOrEmpty(x.LocalDoctor)).WithMessage(x => Validationmessages.ReportBasePropertyInvalidFormat(displayNameResolver.GetDisplayName(nameof(Person)), x.GetDisplayName()));
        }
    }
}