using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class StatLpReportValidator : AbstractValidator<StatLpReport>
    {
        static StatLpReportValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }


        public StatLpReportValidator()
        {

            this.RuleFor(x => x.Institution).NotEmpty();
            this.RuleFor(x => x.SourceSystemId).NotEmpty();
            this.RuleFor(x => x.Institution).SetValidator(new InstitutionValidator());
            this.RuleFor(x => x.From).NotEmpty();
            this.RuleFor(x => x.To).NotEmpty();
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());
            this.RuleFor(x => x.To).SetValidator(new TimestampWithOutTimeValidator());
            this.RuleFor(x => x.ToD).LessThanOrEqualTo(x => new DateTime(DateTime.Today.Year, 12, 31));
            this.RuleFor(x => x.ToD).GreaterThan(x => x.FromD).Unless(x => x.From == null || x.To == null);

            this.RuleForEach(report => report.Persons).SetValidator(new PersonValidator());
            
            this.RuleFor(x => x).SetValidator(new UniqePersonIdValidator());

            this.RuleFor(x => x).SetValidator(new PersonHasUniqueIdValidator());
            

            this.RuleFor(x => new { x.FromD, x.ToD })
                .Must(x => x.FromD.Year == x.ToD.Year)
                .Unless(x => x.From == null || x.To == null)
                .WithMessage(Validationmessages.SameYear);

            this.RuleFor(x => x.ToD)
                .Must(x => x.Day == 31 && x.Month == 12)
                .Unless(x => x.To == null)
                .WithMessage(Validationmessages.LastDateInYear);

            this.RuleFor(x => x.FromD)
                .Must(x => x.Day == 1 && x.Month == 1)
                .Unless(x => x.From == null)
                .WithMessage(Validationmessages.FirstDateInYear);

            this.RuleFor(x => x).SetValidator(report => new AdmissionsValidator());
            this.RuleForEach(report => report.Admissions).SetValidator(report => new AdmissionValidator(report));

            this.RuleFor(x => x).SetValidator(report => new AttributesValidator());
            this.RuleForEach(report => report.Attributes).SetValidator(report => new AttributeValidator(report));

            this.RuleFor(x => x).SetValidator(report => new LeavingsValidator());
            this.RuleForEach(report => report.Leavings).SetValidator(report => new LeavingValidator(report));

            this.RuleForEach(report => report.Stays).SetValidator(report => new StayValidator(report));
            this.RuleFor(report => report).SetValidator(new PersonStayValidator());
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<StatLpReport> context, CancellationToken cancellation = default(CancellationToken))
        {
            return new StatLpReportValidationResult(await base.ValidateAsync(context, cancellation));
        }

        public override ValidationResult Validate(ValidationContext<StatLpReport> context)
        {
            return new StatLpReportValidationResult(base.Validate(context));
        }
    }
}
