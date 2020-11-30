using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace Vodamep.Mkkp.Validation
{
    public class MkkpReportValidationResult : ValidationResult
    {
        public MkkpReportValidationResult(ValidationResult result)
            : base(result.Errors)
        {

        }
        public override bool IsValid => this.Errors.Where(x => x.Severity == Severity.Error).Count() == 0;
    }
}
