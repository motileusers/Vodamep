using FluentValidation;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation.Adjacent
{
    internal class StatLpAdjacentReportsValidator : AbstractValidator<(StatLpReport Predecessor, StatLpReport Report)>
    {
        public StatLpAdjacentReportsValidator()
        {
            this.RuleFor(x => x).SetValidator(new StatLpAdjacentReportsAdmissionsDataValidator());

            this.RuleFor(x => x).SetValidator(new StatLpAdjacentReportsPersonsDataValidator());

            this.RuleFor(x => x).SetValidator(new StatLpAdjacentReportsStaysValidator());
        }
    }
}
