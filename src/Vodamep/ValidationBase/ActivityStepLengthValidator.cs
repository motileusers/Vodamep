using System.Globalization;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class ActivityStepLengthValidator : AbstractValidator<IPersonActivity>
    {
        public ActivityStepLengthValidator(float stepLength)
        {
            this.RuleFor(x => x.Time)
                .Must(x => x % stepLength < float.Epsilon)
                .WithMessage(x => Validationmessages.ReportBaseActivityWrongStepLength(x.PersonId, stepLength.ToString("F2", CultureInfo.InvariantCulture)));
        }

    }
}