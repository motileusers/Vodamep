using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace Vodamep.Tb.Validation
{
    public class TbReportValidationResult : ValidationResult
    {
        public TbReportValidationResult(ValidationResult result)
            : base(result.Errors)
        {

        }
        public override bool IsValid => this.Errors.Where(x => x.Severity == Severity.Error).Count() == 0;
    }
}
