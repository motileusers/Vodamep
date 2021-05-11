using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class StayValidator : AbstractValidator<Stay>
    {
        public StayValidator(StatLpReport parentReport)
        {
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());
            this.RuleFor(x => x.To).SetValidator(new TimestampWithOutTimeValidator());

            this.RuleFor(x => x.To.AsDate()).GreaterThanOrEqualTo(x => x.From.AsDate()).Unless(x => x.From == null || x.To == null).WithMessage(Validationmessages.FromMustBeBeforeTo);

            this.RuleFor(x => x)
                .Must(x => x.From >= parentReport.From &&
                           x.From <= parentReport.To &&
                           x.To >= parentReport.From &&
                           x.To <= parentReport.To)
                .WithMessage(x => Validationmessages.ReportBaseItemMustBeInCurrentMonth("Ein Aufenthalt", x.PersonId));

            this.RuleFor(x => x.PersonId)
                .Must((stay, personId) =>
                {
                    return parentReport.Persons.Any(y => y.Id == personId);
                })
                .WithMessage(Validationmessages.PersonIsNotAvailable);
        }
    }
}
