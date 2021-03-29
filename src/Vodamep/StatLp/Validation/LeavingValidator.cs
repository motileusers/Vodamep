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

            this.RuleFor(x => x).Must(x =>
                {
                    if (x.DischargeReason != DischargeReason.OtherDr &&
                         !string.IsNullOrWhiteSpace(x.DischargeReasonOther))
                    {
                        return false;
                    }

                    return true;

                }).WithMessage(x => Validationmessages.LeavingOtherFilledNeedsOther(x.PersonId)); ;

             this.RuleFor(x => x).Must(x =>
                {
                    if (x.DischargeLocation != DischargeLocation.OtherDc &&
                        !string.IsNullOrWhiteSpace(x.DischargeLocationOther))
                    {
                        return false;
                    }

                    return true;

                }).WithMessage(x => Validationmessages.LeavingOtherFilledNeedsOther(x.PersonId)); ;


            this.RuleFor(x => x).Must(x =>
            {
                if (x.LeavingReason == LeavingReason.DischargeLr)
                {
                    if (x.DischargeLocation == DischargeLocation.UndefinedDc)
                    {
                        return false;
                    }
                }

                return true;

            }).WithMessage(x => Validationmessages.LeavingClientNeedsLeavingLocation(x.PersonId));

            this.RuleFor(x => x).Must(x =>
            {
                if (x.LeavingReason == LeavingReason.DischargeLr)
                {
                    if (x.DeathLocation != DeathLocation.UndefinedDl)
                    {
                        return false;
                    }
                }

                return true;

            }).WithMessage(x => Validationmessages.LeavingClientDeathMustNotBeFilled(x.PersonId));

            this.RuleFor(x => x.DeathLocation).Empty()
                .When(x => x.LeavingReason != LeavingReason.DeceasedLr)
                .WithMessage(x => Validationmessages.LeavingClientDeathMustNotBeFilled(x.PersonId));

            this.RuleFor(x => x.DischargeLocation).NotEmpty()
                .When(x => x.LeavingReason == LeavingReason.DischargeLr)
                .WithMessage(x => Validationmessages.DischargedClientNeedsDischargeLocation(x.PersonId));
        }
    }
}