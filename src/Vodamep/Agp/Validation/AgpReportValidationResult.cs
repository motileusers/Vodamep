using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace Vodamep.Agp.Validation
{
    public class AgpReportValidationResult : ValidationResult
    {
        public AgpReportValidationResult(ValidationResult result)
            : base(result.Errors)
        {

        }
        public override bool IsValid => this.Errors.Where(x => x.Severity == Severity.Error).Count() == 0;
    }
}
