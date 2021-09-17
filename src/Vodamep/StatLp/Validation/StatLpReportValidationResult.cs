using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace Vodamep.StatLp.Validation
{
    public class StatLpReportValidationResult : ValidationResult
    {
        public StatLpReportValidationResult(ValidationResult result)
            : base(result.Errors)
        {

        }
        public override bool IsValid => this.Errors.Where(x => x.Severity == Severity.Error).Count() == 0;
    }
}
