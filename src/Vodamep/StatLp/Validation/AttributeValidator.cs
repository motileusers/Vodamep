using FluentValidation;
using System;
using System.Linq;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;
using Attribute = Vodamep.StatLp.Model.Attribute;

namespace Vodamep.StatLp.Validation
{

    internal class AttributeValidator : AbstractValidator<Attribute>
    {
        private static readonly DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public AttributeValidator(StatLpReport report)
        {
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());

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

            }).WithName(displayNameResolver.GetDisplayName(nameof(Attribute)))
              .WithMessage(x => Validationmessages.ReportBaseItemMustBeInReportPeriod(report.GetPersonName(x.PersonId)));


            this.RuleFor(x => x.PersonId)
                .Must((attribute, personId) =>
                {
                    return report.Persons.Any(y => y.Id == personId);
                })
                .WithMessage(Validationmessages.PersonIsNotAvailable);

            this.RuleFor(x => x)
                .Must((attribute) =>
                {
                    var value = attribute.Value;

                    if (string.IsNullOrWhiteSpace(value) ||
                        value == AdmissionType.UndefinedAt.ToString() ||
                        value == CareAllowance.UndefinedAllowance.ToString() ||
                        value == CareAllowanceArge.UndefinedAr.ToString() ||
                        value == Finance.UndefinedFi.ToString())
                    {
                        return false;
                    }

                    return true;
                })
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmptyWithString(displayNameResolver.GetDisplayName(x.AttributeType.ToString()), report.GetPersonName(x.PersonId)));


            this.RuleFor(x => x)
                .Must((attribute, personId) =>
                {
                    switch (attribute.AttributeType)
                    {
                        case AttributeType.UndefinedAttribute:
                            return false;
                        case AttributeType.CareAllowance:
                            return Enum.TryParse(attribute.Value, out CareAllowance careAllowance);
                        case AttributeType.CareAllowanceArge:
                            return Enum.TryParse(attribute.Value, out CareAllowanceArge careAllowanceArge);
                        case AttributeType.Finance:
                            return Enum.TryParse(attribute.Value, out Finance finance);
                        default:
                            return false;
                    }

                })
                .WithMessage(x => Validationmessages.StatLpAttributeWrongValue(
                   displayNameResolver.GetDisplayName(x.AttributeType.ToString()),
                    displayNameResolver.GetDisplayName(x.Value)));

        }
    }
}
