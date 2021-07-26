using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class LeavingValidator : AbstractValidator<Leaving>
    {
        private DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public LeavingValidator(StatLpReport parentReport)
        {
            this.RuleFor(x => x.LeavingReason).NotEmpty().WithMessage(x => Validationmessages.StatLpReportLeavingReasonMustnotBeEmpty(x.PersonId));
           
            this.RuleFor(x => x.DeathLocation).NotEmpty()
                .Unless(x => x.LeavingReason != LeavingReason.DeceasedLr)
                .Unless(x => x.LeavingReason == LeavingReason.UndefinedLr)
                .WithMessage(x => Validationmessages.DeadClientNeedsDeadthLocation(x.PersonId));

            this.RuleFor(x => x.LeavingDate)
                .Must(x => parentReport.From <= x && x <= parentReport.To)
                .WithName(displayNameResolver.GetDisplayName(nameof(Leaving)))
                .WithMessage(x => Validationmessages.ReportBaseItemMustBeInCurrentMonth(x.PersonId));

            this.RuleFor(x => x).Must(x =>
            {
                if (x.LeavingReason == LeavingReason.DeceasedLr)
                {
                    if (x.DischargeLocation != DischargeLocation.UndefinedDc)
                        return false;

                    if (!string.IsNullOrEmpty(x.DischargeLocationOther))
                        return false;

                    if (x.DischargeReason != DischargeReason.UndefinedDr)
                        return false;

                    if (!string.IsNullOrEmpty(x.DischargeReasonOther))
                        return false;
                }

                return true; 

            }).WithMessage(x => Validationmessages.DeadClientMustNotContainDischargeLocation(x.PersonId));

            this.RuleFor(x => x.DischargeReasonOther).Empty()
                .Unless(x => x.DischargeReason == DischargeReason.OtherDr)
                .WithMessage(x => Validationmessages.LeavingOtherFilledNeedsOther(x.PersonId));

            this.RuleFor(x => x.DischargeLocationOther).Empty()
                 .Unless(x => x.DischargeLocation == DischargeLocation.OtherDc)
                 .WithMessage(x => Validationmessages.LeavingOtherFilledNeedsOther(x.PersonId));

             this.RuleFor(x => x.DischargeLocation).NotEmpty()
                .When(x => x.LeavingReason == LeavingReason.DischargeLr)
                .WithMessage(x => Validationmessages.LeavingClientNeedsLeavingLocation(x.PersonId));

             this.RuleFor(x => x.DischargeReason).NotEmpty()
                 .When(x => x.LeavingReason == LeavingReason.DischargeLr)
                 .WithMessage(x => Validationmessages.LeavingClientNeedsLeavingReason(x.PersonId));

            this.RuleFor(x => x.DeathLocation).Empty()
                .When(x => x.LeavingReason != LeavingReason.DeceasedLr)
                .WithMessage(x => Validationmessages.LeavingClientDeathMustNotBeFilled(x.PersonId));

            this.RuleFor(x => x.DischargeLocation).NotEmpty()
                .When(x => x.LeavingReason == LeavingReason.DischargeLr)
                .WithMessage(x => Validationmessages.DischargedClientNeedsDischargeLocation(x.PersonId));

            var r = new Regex(@"^[-,.a-zA-Z0-9äöüÄÖÜß\(\) ][-,.a-zA-Z0-9äöüÄÖÜß\(\) ]*[-,.a-zA-Z0-9äöüÄÖÜß\(\) ]$");

            this.RuleFor(x => x.DischargeLocationOther)
                .Matches(r).Unless(x => string.IsNullOrEmpty(x.DischargeLocationOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.DischargeLocationOther)))
                .WithMessage(x => Validationmessages.InvalidValue(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.DischargeLocationOther)
                .MaximumLength(30).Unless(x => string.IsNullOrEmpty(x.DischargeLocationOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.DischargeLocationOther)))
                .WithMessage(x => Validationmessages.TextTooLong(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.DischargeReasonOther)
                .Matches(r).Unless(x => string.IsNullOrEmpty(x.DischargeReasonOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.DischargeReasonOther)))
                .WithMessage(x => Validationmessages.InvalidValue(parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.DischargeReasonOther)
                .MaximumLength(30).Unless(x => string.IsNullOrEmpty(x.DischargeReasonOther))
                .WithName(x => displayNameResolver.GetDisplayName(nameof(x.DischargeReasonOther)))
                .WithMessage(x => Validationmessages.TextTooLong(parentReport.FromD.ToShortDateString(), x.PersonId));

        }
    }
}