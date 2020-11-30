using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Mkkp.Model;

namespace Vodamep.Mkkp.Validation
{

    internal class MkkpReportValidator : AbstractValidator<MkkpReport>
    {
        static MkkpReportValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new MkkpDisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }
        public MkkpReportValidator()
        {
            // todo
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<MkkpReport> context, CancellationToken cancellation = default(CancellationToken))
        {
            return new MkkpReportValidationResult(await base.ValidateAsync(context, cancellation));
        }

        public override ValidationResult Validate(ValidationContext<MkkpReport> context)
        {
            return new MkkpReportValidationResult(base.Validate(context));
        }
    }
}
