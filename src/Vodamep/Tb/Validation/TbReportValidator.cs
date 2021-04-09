using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Tb.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Tb.Validation
{

    internal class TbReportValidator : AbstractValidator<TbReport>
    {
        static TbReportValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }
        public TbReportValidator()
        {
            this.RuleFor(x => x.Institution).NotEmpty();
            
            this.RuleFor(x => x).SetValidator(new ReportDateValidator());
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<TbReport> context, CancellationToken cancellation = default(CancellationToken))
        {
            return new TbReportValidationResult(await base.ValidateAsync(context, cancellation));
        }

        public override ValidationResult Validate(ValidationContext<TbReport> context)
        {
            return new TbReportValidationResult(base.Validate(context));
        }
    }
}
