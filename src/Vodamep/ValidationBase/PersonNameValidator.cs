using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class PersonNameValidator : AbstractValidator<INamedPerson>
    {
        public PersonNameValidator(string nameRegex,int minLengthGivenName, int maxLengthGivenName, int minLengthFamilyName, int maxLengthFamilyName)
        {
            this.RuleFor(x => x.FamilyName).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.GetDisplayName()));
            this.RuleFor(x => x.GivenName).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.GetDisplayName()));

            var r = new Regex(nameRegex);
            this.RuleFor(x => x.FamilyName)
                .Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName))
                .WithMessage(x => Validationmessages.ReportBasePropertyInvalidFormat(x.GetDisplayName()));

            this.RuleFor(x => x.GivenName)
                .Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName))
                .WithMessage(x => Validationmessages.ReportBasePropertyInvalidFormat(x.GetDisplayName()));

            if (minLengthGivenName > 0)
            {
                this.RuleFor(x => x.GivenName).MinimumLength(minLengthGivenName).WithMessage(x => Validationmessages.ReportBaseInvalidLength(x.GetDisplayName()));
            }

            if (maxLengthGivenName > 0)
            {
                this.RuleFor(x => x.GivenName).MaximumLength(maxLengthGivenName).WithMessage(x => Validationmessages.ReportBaseInvalidLength(x.GetDisplayName()));
            }

            if (minLengthFamilyName > 0)
            {
                this.RuleFor(x => x.FamilyName).MinimumLength(minLengthFamilyName).WithMessage(x => Validationmessages.ReportBaseInvalidLength(x.Id));
            }

            if (maxLengthFamilyName > 0)
            {
                this.RuleFor(x => x.FamilyName).MaximumLength(maxLengthFamilyName).WithMessage(x => Validationmessages.ReportBaseInvalidLength(x.Id));
            }
        }
    }
}