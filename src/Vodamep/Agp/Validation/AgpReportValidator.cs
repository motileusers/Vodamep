using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Agp.Model;

namespace Vodamep.Agp.Validation
{

    internal class AgpReportValidator : AbstractValidator<AgpReport>
    {
        static AgpReportValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new AgpDisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }
        public AgpReportValidator()
        {
            // todo
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<AgpReport> context, CancellationToken cancellation = default(CancellationToken))
        {
            return new AgpReportValidationResult(await base.ValidateAsync(context, cancellation));
        }

        public override ValidationResult Validate(ValidationContext<AgpReport> context)
        {
            return new AgpReportValidationResult(base.Validate(context));
        }
    }
}
