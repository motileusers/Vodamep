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
              .WithMessage(x => Validationmessages.ReportBaseItemMustBeInCurrentMonth(report.GetPersonName(x.PersonId)));


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


            // Ungültige Aufnahmeart 'Probe'
            this.RuleFor(x => x)
                .Must((attribute) =>
                {
                    if (attribute.AttributeType == AttributeType.AdmissionType)
                    {
                        var value = attribute.Value;

                        if (value == AdmissionType.TrialAt.ToString() &&
                            report.FromD > new DateTime(2014, 08, 01))
                        {
                            return false;
                        }
                    }

                    return true;
                })
                .WithMessage(x => Validationmessages.StatLpReportInvalidAdmissionType(report.GetPersonName(x.PersonId), displayNameResolver.GetDisplayName(x.Value), x.FromD.ToShortDateString()));


            // Ungültige Aufnahmeart 'Krisenintervention'
            this.RuleFor(x => x)
                .Must((attribute) =>
                {
                    if (attribute.AttributeType == AttributeType.AdmissionType)
                    {
                        var value = attribute.Value;

                        if (value == AdmissionType.CrisisInterventionAt.ToString() &&
                            report.FromD > new DateTime(2019, 11, 30))
                        {
                            return false;
                        }
                    }

                    return true;
                })
                .WithMessage(x => Validationmessages.StatLpReportInvalidAdmissionType(report.GetPersonName(x.PersonId), displayNameResolver.GetDisplayName(x.Value), x.FromD.ToShortDateString()));



            this.RuleFor(x => x)
                .Must((attribute, personId) =>
                {
                    switch (attribute.AttributeType)
                    {
                        case AttributeType.UndefinedAttribute:
                            return false;
                        case AttributeType.AdmissionType:
                            return Enum.TryParse(attribute.Value, out AdmissionType admissionType);
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
                .WithMessage(x => Validationmessages.StatLpReportAttributeWrongValue(
                   displayNameResolver.GetDisplayName(x.AttributeType.ToString()),
                    displayNameResolver.GetDisplayName(x.Value)));

        }
    }
}
