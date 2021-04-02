using System;
using FluentValidation;
using Vodamep.Cm.Model;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class ActivityMinutesValidator : AbstractValidator<IClientActivity>
    {
        public ActivityMinutesValidator(DateTime reportDate, float minValue, float maxValue)
        {
            this.RuleFor(x => x.Minutes)
                .GreaterThanOrEqualTo(minValue)
                .WithMessage(x => Validationmessages.ReportBaseActivityWrongValue(x.PersonId, reportDate.ToShortDateString(), $"< {minValue}"));

            this.RuleFor(x => x.Minutes)
                .LessThanOrEqualTo(maxValue)
                .WithMessage(x => Validationmessages.ReportBaseActivityWrongValue(x.PersonId, reportDate.ToShortDateString(), $"> {maxValue}"));
        }
    }
}
