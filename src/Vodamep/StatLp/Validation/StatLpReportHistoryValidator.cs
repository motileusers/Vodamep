using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class StatLpReportHistoryValidator : AbstractValidator<StatLpReportHistory>
    {
        static StatLpReportHistoryValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }
        public StatLpReportHistoryValidator()
        {
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<StatLpReportHistory> context, CancellationToken cancellation = default(CancellationToken))
        {
            return new StatLpReportValidationResult(await base.ValidateAsync(context, cancellation));
        }

        public override ValidationResult Validate(ValidationContext<StatLpReportHistory> context)
        {
            return new StatLpReportValidationResult(base.Validate(context));
        }
    }
}
