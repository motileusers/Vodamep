using FluentValidation;
using Vodamep.Cm.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Cm.Validation
{
    internal class CmActivityValidator : AbstractValidator<Activity>
    {
        public CmActivityValidator(CmReport report)
        {
            this.RuleFor(x => x.ActivityType).NotEmpty().WithMessage(x => Validationmessages.ReportBaseActivityNoCategory(x.PersonId, x.Date.ToDateTime().ToShortDateString()));
            this.RuleFor(x => x.Date).Must(x => x >= report.From && x <= report.To).WithMessage(x => Validationmessages.ReportBaseActivityWrongDate(x.PersonId, x.Date.ToDateTime().ToShortDateString()));
            this.RuleFor(x => x).SetValidator(x => new ActivityTimeValidator (x.DateD, 1, 10000));
        }
    }
}