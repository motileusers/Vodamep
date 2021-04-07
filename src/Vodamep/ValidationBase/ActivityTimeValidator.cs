using System;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class ActivityTimeValidator : AbstractValidator<IPersonActivity>
    {
        public ActivityTimeValidator(float minValue, float maxValue)
        {
            this.RuleFor(x => x.Time)
                .GreaterThanOrEqualTo(minValue)
                .WithMessage(x => Validationmessages.ReportBaseActivityWrongValue(x.PersonId, $"< {minValue}"));

            this.RuleFor(x => x.Time)
                .LessThanOrEqualTo(maxValue)
                .WithMessage(x => Validationmessages.ReportBaseActivityWrongValue(x.PersonId,$"> {maxValue}"));
        }

        public ActivityTimeValidator(DateTime reportDate, float minValue, float maxValue)
        {
            this.RuleFor(x => x.Time)
                .GreaterThanOrEqualTo(minValue)
                .WithMessage(x => Validationmessages.ReportBaseActivityWrongValue(x.PersonId, reportDate.ToShortDateString(), $"< {minValue}"));

            this.RuleFor(x => x.Time)
                .LessThanOrEqualTo(maxValue)
                .WithMessage(x => Validationmessages.ReportBaseActivityWrongValue(x.PersonId, reportDate.ToShortDateString(), $"> {maxValue}"));
        }
    }
}
