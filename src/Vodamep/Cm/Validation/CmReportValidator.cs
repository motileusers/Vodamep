using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Cm.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Cm.Validation
{

    internal class CmReportValidator : AbstractValidator<CmReport>
    {
        static CmReportValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }
        public CmReportValidator()
        {
            this.RuleFor(x => x.Institution).NotEmpty();
            //this.RuleFor(x => x.Institution).SetValidator(new InstitutionValidator());
            
            this.RuleFor(x => x).SetValidator(new ReportDateValidator());

            this.RuleFor(x => x).SetValidator(new UniqePersonValidator());

            var earliestBirthday = new DateTime(1890, 01, 01);
            var nameRegex = "^[a-zA-ZäöüÄÖÜß][-a-zA-ZäöüÄÖÜß ]*?[a-zA-ZäöüÄÖÜß]$";
            this.RuleForEach(report => report.Persons).SetValidator(new PersonBirthdayValidator(earliestBirthday));
            this.RuleForEach(report => report.Persons).SetValidator(new PersonNameValidator(nameRegex, 2, 30, 2, 50));
            this.RuleForEach(report => report.Persons).SetValidator(new CmPersonValidator());


            //this.RuleForEach(report => report.Activities).SetValidator(r => new ActivityValidator(r.FromD, r.ToD));
            //this.RuleForEach(report => report.Activities).SetValidator(r => new ActivityValidator4141617Without123(r.Persons, r.Staffs));

            //// Nur für neu gesendete Daten
            //this.RuleForEach(report => report.Activities).SetValidator(r => new ActivityValidator23Without417(r.Persons, r.Staffs)).Unless(x => x.ToD < new DateTime(2019, 01, 01));


            //this.RuleForEach(report => report.Staffs).SetValidator(r => new StaffValidator(r.FromD, r.ToD));

            //this.Include(new ActivityMedicalByQualificationTraineeValidator());

            //this.Include(new ActivityWarningIfMoreThan5Validator());

            //this.Include(new ActivityWarningIfMoreThan350Validator());            

            //this.Include(new HkpvReportPersonIdValidator());

            //this.Include(new HkpvReportStaffIdValidator());

            //this.Include(new PersonSsnIsUniqueValidator());

            //this.Include(new EmploymentActivityValidator());
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<CmReport> context, CancellationToken cancellation = default(CancellationToken))
        {
            return new CmReportValidationResult(await base.ValidateAsync(context, cancellation));
        }

        public override ValidationResult Validate(ValidationContext<CmReport> context)
        {
            return new CmReportValidationResult(base.Validate(context));
        }
    }
}
