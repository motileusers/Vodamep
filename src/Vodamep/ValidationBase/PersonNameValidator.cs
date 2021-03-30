using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class PersonNameValidator : AbstractValidator<INamedPerson>
    {
        public PersonNameValidator(string nameRegex)
        {
            this.RuleFor(x => x.FamilyName).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.GivenName).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));

            var r = new Regex(nameRegex);
            this.RuleFor(x => x.FamilyName)
                .Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName))
                .WithMessage(x => Validationmessages.ReportBasePropertInvalidFormat(x.Id));

            this.RuleFor(x => x.GivenName)
                .Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName))
                .WithMessage(x => Validationmessages.ReportBasePropertInvalidFormat(x.Id));
        }
    }
}