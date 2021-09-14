using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class StayValidator : AbstractValidator<Stay>
    {
        private DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public StayValidator(StatLpReport report)
        {
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());
            this.RuleFor(x => x.To).SetValidator(new TimestampWithOutTimeValidator());

            this.RuleFor(x => x.To.AsDate()).GreaterThanOrEqualTo(x => x.From.AsDate()).Unless(x => x.From == null || x.To == null).WithMessage(Validationmessages.FromMustBeBeforeTo);


            this.RuleFor(x => x.FromD).Must(x =>
            {
                if (x < report.FromD)
                {
                    return false;
                }

                if (x > report.ToD)
                {
                    return false;
                }

                return true;

            }).WithName(displayNameResolver.GetDisplayName(nameof(Stay)))
              .WithMessage(x => Validationmessages.ReportBaseItemMustBeInCurrentMonth(report.GetPersonName(x.PersonId)));

            this.RuleFor(x => x.ToD).Must(x =>
            {
                if (x < report.FromD)
                {
                    return false;
                }

                if (x > report.ToD)
                {
                    return false;
                }

                return true;

            }).WithName(displayNameResolver.GetDisplayName(nameof(Stay)))
              .WithMessage(x => Validationmessages.ReportBaseItemMustBeInCurrentMonth(report.GetPersonName(x.PersonId)));


            this.RuleFor(x => x.PersonId)
                .Must((stay, personId) =>
                {
                    return report.Persons.Any(y => y.Id == personId);
                })
                .WithMessage(Validationmessages.PersonIsNotAvailable);
        }
    }
}
