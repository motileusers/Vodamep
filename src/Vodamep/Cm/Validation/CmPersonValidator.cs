using System;
using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.Cm.Model;
using Vodamep.ReportBase.Validation;
using Vodamep.ValidationBase;

namespace Vodamep.Cm.Validation
{
    internal class CmPersonValidator : AbstractValidator<Person>
    {
        public CmPersonValidator()
        {
            this.RuleFor(x => x.Gender).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.Country).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.Postcode).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.City).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
            this.RuleFor(x => x.CareAllowance).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));
        }
    }
}
