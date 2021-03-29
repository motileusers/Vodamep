using System.Text.RegularExpressions;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class LeavingValidator : AbstractValidator<Leaving>
    {
        public LeavingValidator()
        {
            this.RuleFor(x => x.LeavingReason).NotEmpty();
           
            this.RuleFor(x => x.DeathLocation).NotEmpty()
                .Unless(x => x.LeavingReason != LeavingReason.DeceasedLr)
                .Unless(x => x.LeavingReason == LeavingReason.UndefinedLr)
                .WithMessage(x => Validationmessages.DeadClientNeedsDeadthLocation(x.PersonId));
            
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

            this.RuleFor(x => x.DeathLocation).Empty()
                .When(x => x.LeavingReason != LeavingReason.DeceasedLr)
                .WithMessage(x => Validationmessages.LeavingClientDeathMustNotBeFilled(x.PersonId));

            this.RuleFor(x => x.DischargeLocation).NotEmpty()
                .When(x => x.LeavingReason == LeavingReason.DischargeLr)
                .WithMessage(x => Validationmessages.DischargedClientNeedsDischargeLocation(x.PersonId));
        }
    }
}