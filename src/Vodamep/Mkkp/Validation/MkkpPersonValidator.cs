using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.Data;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class MkkpPersonValidator : AbstractValidator<Person>
    {
        public MkkpPersonValidator()
        {
            MkkpDisplayNameResolver displayNameResolver = new MkkpDisplayNameResolver();

            this.RuleFor(x => x.Birthday).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Birthday)), x.GetDisplayName()));
            this.RuleFor(x => x.Referrer).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Referrer)), x.GetDisplayName()));
            this.RuleFor(x => x.HospitalDoctor).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.HospitalDoctor)), x.GetDisplayName()));
            this.RuleFor(x => x.LocalDoctor).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.LocalDoctor)), x.GetDisplayName()));
            this.RuleFor(x => x.Insurance).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Insurance)), x.GetDisplayName()));
            this.RuleFor(x => x.CareAllowance).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.CareAllowance)), x.GetDisplayName()));
            this.RuleFor(x => x.Postcode).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Postcode)), x.GetDisplayName()));
            this.RuleFor(x => x.City).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.City)), x.GetDisplayName()));
            this.RuleFor(x => x.Gender).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Gender)), x.GetDisplayName()));

            // Änderung 5.11.2018, LH
            var r = new Regex(@"^[\p{L}][-\p{L}. ]*[\p{L}.]$");
            this.RuleFor(x => x.HospitalDoctor).Matches(r).Unless(x => string.IsNullOrEmpty(x.HospitalDoctor)).WithMessage(x => Validationmessages.ReportBasePropertyInvalidFormat(x.GetDisplayName()));
            this.RuleFor(x => x.LocalDoctor).Matches(r).Unless(x => string.IsNullOrEmpty(x.LocalDoctor)).WithMessage(x => Validationmessages.ReportBasePropertyInvalidFormat(x.GetDisplayName()));

            this.RuleFor(x => x.Insurance).SetValidator(new CodeValidator<InsuranceCodeProvider>());
         
            this.RuleFor(x => $"{x.Postcode} {x.City}")
                .SetValidator(new CodeValidator<Postcode_CityProvider>())
                .Unless(x => string.IsNullOrEmpty(x.City) || string.IsNullOrEmpty(x.Postcode))
                .WithMessage(x => Validationmessages.ReportBaseInvalidPostCodeCity(x.GetDisplayName()));

            this.RuleFor(x => x.OtherReferrer).NotEmpty()
                .When(y => y.Referrer == Referrer.OtherReferrer)
                .WithMessage(x => Validationmessages.ReportBaseReferrerIsOtherRefererrerThenOtherReferrerMustBeSet(x.GetDisplayName()));
         
            this.RuleFor(x => x.Diagnoses).NotEmpty().WithMessage(x => Validationmessages.AtLeastOneDiagnosisGroup(x.GetDisplayName()));
            this.Include(new DiagnosisGroupIsUniqueValidator());
            this.Include(new DiagnosisGroupMustNotContainUndefinedValueValidator());
            this.Include(new DiagnosisGroupOnlyOnePalliativCareValidator());
        }

    }
}