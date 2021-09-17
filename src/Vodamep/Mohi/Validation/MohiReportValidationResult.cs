using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace Vodamep.Mohi.Validation
{
    public class MohiReportValidationResult : ValidationResult
    {
        public MohiReportValidationResult(ValidationResult result)
            : base(result.Errors)
        {

        }
        public override bool IsValid => this.Errors.Where(x => x.Severity == Severity.Error).Count() == 0;
    }
}
