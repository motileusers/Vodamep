using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Mohi.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mohi.Validation
{

    internal class MohiReportValidator : AbstractValidator<MohiReport>
    {
        static MohiReportValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }
        public MohiReportValidator()
        {
            this.RuleFor(x => x.Institution).NotEmpty();

            this.RuleFor(x => x).SetValidator(new ReportDateValidator());

            this.RuleFor(x => x).SetValidator(new UniqePersonValidator());

            var earliestBirthday = new DateTime(1890, 01, 01);
            var nameRegex = "^[a-zA-ZäöüÄÖÜß][-a-zA-ZäöüÄÖÜß ]*?[a-zA-ZäöüÄÖÜß]$";
            this.RuleForEach(report => report.Persons).SetValidator(new PersonBirthdayValidator(earliestBirthday));
            this.RuleForEach(report => report.Persons).SetValidator(new PersonNameValidator(nameRegex, 2, 30, 2, 50));
            this.RuleForEach(report => report.Persons).SetValidator(new MohiPersonValidator());
          
            this.RuleForEach(report => report.Activities).SetValidator(new MohiActivityValidator());

        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<MohiReport> context, CancellationToken cancellation = default(CancellationToken))
        {
            return new MohiReportValidationResult(await base.ValidateAsync(context, cancellation));
        }

        public override ValidationResult Validate(ValidationContext<MohiReport> context)
        {
            return new MohiReportValidationResult(base.Validate(context));
        }
    }
}
