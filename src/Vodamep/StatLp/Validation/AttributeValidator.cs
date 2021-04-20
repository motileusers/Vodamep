using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class AttributeValidator : AbstractValidator<Attribute>
    {
        public AttributeValidator(StatLpReport parentReport)
        {
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());

            this.RuleFor(x => x.From)
                .Must(x => parentReport.From <= x && x <= parentReport.To)
                .WithMessage(x => Validationmessages.ReportBaseItemMustBeInCurrentMonth("Das Attribut", x.PersonId));


            this.RuleFor(x => x.PersonId)
                .Must((admission, personId) =>
                {
                    return parentReport.Persons.Any(y => y.Id == personId);
                })
                .WithMessage(Validationmessages.PersonIsNotAvailable);

        }
    }
}
