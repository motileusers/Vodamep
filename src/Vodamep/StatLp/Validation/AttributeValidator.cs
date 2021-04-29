using System;
using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;
using Attribute = Vodamep.StatLp.Model.Attribute;

namespace Vodamep.StatLp.Validation
{

    internal class AttributeValidator : AbstractValidator<Attribute>
    {
        private static DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public AttributeValidator(StatLpReport parentReport)
        {
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());

            this.RuleFor(x => x.From)
                .Must(x => parentReport.From <= x && x <= parentReport.To)
                .WithMessage(x => Validationmessages.ReportBaseItemMustBeInCurrentMonth("Das Attribut", x.PersonId));


            this.RuleFor(x => x.PersonId)
                .Must((attribute, personId) =>
                {
                    return parentReport.Persons.Any(y => y.Id == personId);
                })
                .WithMessage(Validationmessages.PersonIsNotAvailable);

            this.RuleFor(x => x)
                .Must((attribute, personId) =>
                {
                    switch (attribute.AttributeType)
                    {
                        case AttributeType.UndefinedAttribute:
                            return false;
                        case AttributeType.AdmissionType:
                            return Enum.TryParse(attribute.Value, out AdmissionType admissionType);
                        case AttributeType.Careallowance:
                            return Enum.TryParse(attribute.Value, out CareAllowance careAllowance);
                        case AttributeType.Careallowancearge:
                            return Enum.TryParse(attribute.Value, out CareAllowanceArge careAllowanceArge);
                        case AttributeType.Finance:
                            return Enum.TryParse(attribute.Value, out Finance finance);
                        default:
                            return false;
                    }

                })
                .WithMessage(x => Validationmessages.StatLpReportAttributeWrongValue(
                   displayNameResolver.GetDisplayName(x.AttributeType.ToString()),
                    displayNameResolver.GetDisplayName(x.Value)));

        }
    }
}
