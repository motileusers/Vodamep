using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

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
            this.RuleFor(x => x.Institution).NotEmpty();
            this.RuleFor(x => x.Institution).SetValidator(new InstitutionValidator());
            this.RuleFor(x => x.From).NotEmpty();
            this.RuleFor(x => x.To).NotEmpty();
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());
            this.RuleFor(x => x.To).SetValidator(new TimestampWithOutTimeValidator());
            this.RuleFor(x => x.ToD).LessThanOrEqualTo(x => DateTime.Today);
            this.RuleFor(x => x.ToD).GreaterThan(x => x.FromD).Unless(x => x.From == null || x.To == null);

            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: this.RuleFor(x => new { x.From, x.To })
            this.RuleFor(x => new Tuple<DateTime, DateTime>(x.FromD, x.ToD))
                .Must(x => x.Item2 == x.Item1.LastDateInMonth())
                .Unless(x => x.From == null || x.To == null)
                .WithMessage(Validationmessages.OneMonth);

            this.RuleFor(x => x.ToD)
                .Must(x => x == x.LastDateInMonth())
                .Unless(x => x.To == null)
                .WithMessage(Validationmessages.LastDateInMonth);

            this.RuleFor(x => x.FromD)
                .Must(x => x.Day == 1)
                .Unless(x => x.From == null)
                .WithMessage(Validationmessages.FirstDateInMonth);

            this.RuleForEach(report => report.Persons).SetValidator(new PersonValidator());

            this.RuleForEach(report => report.Activities).SetValidator(r => new ActivityValidator(r, r.FromD, r.ToD));

            this.Include(new SumOfActivtiesMinutesPerStaffMustBeLowerThan10HoursValidator());

            this.RuleForEach(report => report.TravelTimes).SetValidator(r => new TravelTimeValidator(r));

            this.Include(new SumOfTravelTimesMustBeLowerThan5HoursValidator());

            this.Include(new OnlyOneTravelTimesEntryPerStaffMemberAndDayValidator());

            this.RuleForEach(report => report.Staffs).SetValidator(r => new StaffValidator());

            this.Include(new MkkpReportPersonIdValidator());

            this.Include(new MkkpReportStaffIdValidator());
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
