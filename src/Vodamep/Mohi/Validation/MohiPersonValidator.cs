using System;
using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.Mohi.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mohi.Validation
{
    internal class MohiPersonValidator : AbstractValidator<Person>
    {
        public MohiPersonValidator()
        {
            this.RuleFor(x => x.Gender).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.Postcode).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.City).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.CareAllowance).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.MainAttendanceCloseness).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
         
            this.RuleFor(x => x).SetValidator(x => new CountryValidator());
        }
    }
}
