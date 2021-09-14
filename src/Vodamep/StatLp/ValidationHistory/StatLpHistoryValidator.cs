using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{
    internal class StatLpHistoryValidator : AbstractValidator<StatLpReportHistory>
    {
        static StatLpHistoryValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }

        protected override bool PreValidate(ValidationContext<StatLpReportHistory> context, ValidationResult result)
        {
            IdMapper mapper = new IdMapper();
            mapper.Map(context.InstanceToValidate);

            return base.PreValidate(context, result);
        }


        public StatLpHistoryValidator()
        {
            this.RuleFor(x => x).SetValidator(new PersonHistoryValidator());
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
