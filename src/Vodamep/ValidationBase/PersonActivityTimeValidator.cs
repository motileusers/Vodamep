using System;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class PersonActivityTimeValidator : AbstractValidator<IPersonActivity>
    {
        public PersonActivityTimeValidator(float minValue, float maxValue)
        {
            this.RuleFor(x => x.Time)
                .GreaterThanOrEqualTo(minValue)
                .WithMessage(x => Validationmessages.ReportBasePersonActivityWrongValue(x.PersonId, $"< {minValue}"));

            this.RuleFor(x => x.Time)
                .LessThanOrEqualTo(maxValue)
                .WithMessage(x => Validationmessages.ReportBasePersonActivityWrongValue(x.PersonId,$"> {maxValue}"));
        }

        public PersonActivityTimeValidator(DateTime reportDate, float minValue, float maxValue)
        {
            this.RuleFor(x => x.Time)
                .GreaterThanOrEqualTo(minValue)
                .WithMessage(x => Validationmessages.ReportBasePersonActivityWrongValue(x.PersonId, reportDate.ToShortDateString(), $"< {minValue}"));

            this.RuleFor(x => x.Time)
                .LessThanOrEqualTo(maxValue)
                .WithMessage(x => Validationmessages.ReportBasePersonActivityWrongValue(x.PersonId, reportDate.ToShortDateString(), $"> {maxValue}"));
        }
    }
}
