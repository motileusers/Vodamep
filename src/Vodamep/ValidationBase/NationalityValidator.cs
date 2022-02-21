using FluentValidation;
using Vodamep.Data;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class NationalityValidator : AbstractValidator<INationalityPerson>
    {
        public NationalityValidator()
        {
            this.RuleFor(x => x.Nationality).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.GetDisplayName()));

            this.RuleFor(x => x.Nationality)
                .Must((person, country) => CountryCodeProvider.Instance.IsValid(country) && country != CountryCodeProvider.Instance.Unknown)
                .Unless(x => string.IsNullOrWhiteSpace(x.Nationality))
                .WithMessage(x => Validationmessages.ReportBaseInvalidValue(x.GetDisplayName()));

        }
    }
}