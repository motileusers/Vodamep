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
            
            this.RuleFor(x => x.PersonId)
                .Must((admission, personId) =>
                {
                    return parentReport.Persons.Any(y => y.Id == personId);
                })
                .WithMessage(Validationmessages.PersonIsNotAvailable);

        }
    }
}
