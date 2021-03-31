using FluentValidation;
using Vodamep.Data;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class CountryValidator : AbstractValidator<INamedPerson>
    {
        public CountryValidator()
        {
            this.RuleFor(x => x.Country).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));

            this.RuleFor(x => x.Country)
                .Must((person, country) => CountryCodeProvider.Instance.IsValid(country))
                .Unless(x => string.IsNullOrWhiteSpace(x.Country))
                .WithMessage(x => Validationmessages.ReportBaseInvalidValue(x.Id));

        }
    }
}