using System.Linq;
using FluentValidation;
using System.Text.RegularExpressions;
using Vodamep.Data;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();

            this.RuleFor(x => x.FamilyName).MinimumLength(2).WithMessage(x => Validationmessages.ReportBaseInvalidLength(x.Id));
            this.RuleFor(x => x.GivenName).MinimumLength(2).WithMessage(x => Validationmessages.ReportBaseInvalidLength(x.Id));

            this.RuleFor(x => x.FamilyName).MaximumLength(50).WithMessage(x => Validationmessages.ReportBaseInvalidLength(x.Id));
            this.RuleFor(x => x.GivenName).MaximumLength(30).WithMessage(x => Validationmessages.ReportBaseInvalidLength(x.Id));

            var r = new Regex(@"^[a-zA-ZäöüÄÖÜß][-a-zA-ZäöüÄÖÜß ]*?[a-zA-ZäöüÄÖÜß]$");
            this.RuleFor(x => x.FamilyName).Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName));
            this.RuleFor(x => x.GivenName).Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName));

            this.Include(new PersonBirthdayValidator());

            this.RuleFor(x => x.Gender).NotEmpty();

            this.RuleFor(x => x.Country).NotEmpty();

            this.RuleFor(x => x.Country)
                .Must((person, country) => CountryCodeProvider.Instance.IsValid(country))
                .Unless(x => string.IsNullOrEmpty(x.Country))
                .WithMessage(x => Validationmessages.ReportBaseInvalidValue(x.Id));
        }
    }
}
