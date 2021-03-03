using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace Vodamep.Cm.Validation
{
    public class CmReportValidationResult : ValidationResult
    {
        public CmReportValidationResult(ValidationResult result)
            : base(result.Errors)
        {

        }
        public override bool IsValid => this.Errors.Where(x => x.Severity == Severity.Error).Count() == 0;
    }
}
