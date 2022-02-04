using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.Data;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class AdmissionValidator : AbstractValidator<Admission>
    {
        private static readonly DisplayNameResolver DisplayNameResolver = new DisplayNameResolver();


        private Dictionary<string, DateTime> institutionAdmissionValid = new Dictionary<string, DateTime>()
        {
            {"0*", new DateTime(1995, 01, 01) },
        };

        public AdmissionValidator(StatLpReport report)
        {
            this.RuleFor(x => x.AdmissionDate).NotEmpty();

            this.RuleFor(x => x.Gender).NotEmpty();

            this.RuleFor(x => x.Nationality).NotEmpty();

            this.RuleFor(x => x.Nationality)
                .Must((person, country) => CountryCodeProvider.Instance.IsValid(country))
                .Unless(x => string.IsNullOrEmpty(x.Nationality))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.Nationality)))
                .WithMessage(x => Validationmessages.ReportBaseInvalidValue(this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.AdmissionDate).SetValidator(new TimestampWithOutTimeValidator());

            this.RuleFor(x => x.HousingTypeBeforeAdmission).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(DisplayNameResolver.GetDisplayName(nameof(Person)), this.GetPersonName(x.PersonId, report)));
            this.RuleFor(x => x.MainAttendanceRelation).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(DisplayNameResolver.GetDisplayName(nameof(Person)), this.GetPersonName(x.PersonId, report)));
            this.RuleFor(x => x.MainAttendanceCloseness).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(DisplayNameResolver.GetDisplayName(nameof(Person)), this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.HousingReason)
                .Must((x) =>
                {
                    // Vor diesem Datum war nicht immer gewährleistet, dass dieser Wert befüllt ist
                    if (report.FromD >= new DateTime(2011, 01, 01))
                    {
                        if (x == HousingReason.UndefinedHr)
                            return false;
                    }

                    return true;
                })
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(DisplayNameResolver.GetDisplayName(nameof(Person)), this.GetPersonName(x.PersonId, report)));



            //ungültige werte
            var regex0 = new Regex(@"^[-,.a-zA-Z0-9äöüÄÖÜß\(\) ][-,.a-zA-Z0-9äöüÄÖÜß\(\) ]*[-,.a-zA-Z0-9äöüÄÖÜß\(\) ]$");

            this.RuleFor(x => x.OtherHousingType).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.OtherHousingType))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.OtherHousingType)))
                .WithMessage(x => Validationmessages.StatLpAdmissionInvalidValueAdmission(report.FromD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.PersonalChangeOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.PersonalChangeOther))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.PersonalChangeOther)))
                .WithMessage(x => Validationmessages.StatLpAdmissionInvalidValueAdmission(report.FromD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.SocialChangeOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.SocialChangeOther))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.SocialChangeOther)))
                .WithMessage(x => Validationmessages.StatLpAdmissionInvalidValueAdmission(report.FromD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.HousingReasonOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.HousingReasonOther))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.HousingReasonOther)))
                .WithMessage(x => Validationmessages.StatLpAdmissionInvalidValueAdmission(report.FromD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.OtherHousingType).MaximumLength(100)
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.OtherHousingType)))
                .WithMessage(x => Validationmessages.StatLpAdmissionTextTooLong(report.FromD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.PersonalChangeOther).MaximumLength(100)
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.PersonalChangeOther)))
                .WithMessage(x => Validationmessages.StatLpAdmissionTextTooLong(report.FromD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.SocialChangeOther).MaximumLength(100)
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.SocialChangeOther)))
                .WithMessage(x => Validationmessages.StatLpAdmissionTextTooLong(report.FromD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));

            this.RuleFor(x => x.HousingReasonOther).MaximumLength(100)
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.HousingReasonOther)))
                .WithMessage(x => Validationmessages.StatLpAdmissionTextTooLong(report.FromD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));


            this.RuleFor(x => x)
                .Must((x) =>
                {
                    // Ort / PLZ dürfen nie leer sein
                    if (String.IsNullOrWhiteSpace(x.LastPostcode) ||
                        String.IsNullOrWhiteSpace(x.LastCity))
                    {
                        return false;
                    }

                    return true;
                })
                .WithMessage(x => Validationmessages.StatLpAdmissionEmptyPostCode(x.AdmissionDateD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));


            this.RuleFor(x => x)
                .Must((x) =>
                {
                    bool result = true;

                    // Erst ab 2019 wurden von allen díe Gemeinde Kennzahlen übermittelt
                    // Davor war nicht sichergestellt, dass in PLZ/Ort ein definiertes Wertepaar enthält
                    if (x.AdmissionDateD >= new DateTime(2019, 01, 01))
                    {
                        if (!String.IsNullOrWhiteSpace(x.LastPostcode) &&
                            !String.IsNullOrWhiteSpace(x.LastCity))
                        {
                            result = Postcode_CityProvider.Instance.IsValid($"{x.LastPostcode} {x.LastCity}");
                        }
                    }

                    return result;
                })
                .WithMessage(x => Validationmessages.StatLpAdmissionWrongPostCode(x.AdmissionDateD.ToShortDateString(), this.GetPersonName(x.PersonId, report)));


            this.RuleFor(x => x.PersonalChanges).NotEmpty().Unless(x => !string.IsNullOrEmpty(x.PersonalChangeOther))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.PersonalChangeOther)))
                .WithMessage(Validationmessages.ItemNotValid);

            this.RuleFor(x => x.PersonalChanges)
                .Must((admission, ctx) => !admission.PersonalChanges.Any(x => x == PersonalChange.UndefinedPc))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.PersonalChanges)))
                .WithMessage(Validationmessages.ItemNotValid);

            this.RuleFor(x => x.PersonalChanges)
                .Must((admission, ctx) => ContainsDoubledValues(admission.PersonalChanges))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.PersonalChanges)))
                .WithMessage(Validationmessages.NoDoubledValuesAreAllowed);

            this.RuleFor(x => x.SocialChanges).NotEmpty().Unless(x => !string.IsNullOrEmpty(x.SocialChangeOther))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.SocialChanges)))
                .WithMessage(Validationmessages.ItemNotValid);

            this.RuleFor(x => x.SocialChanges)
                .Must((admission, ctx) => !admission.SocialChanges.Any(x => x == SocialChange.UndefinedSc))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.SocialChanges)))
                .WithMessage(Validationmessages.ItemNotValid);

            this.RuleFor(x => x.SocialChanges)
                .Must((admission, ctx) => ContainsDoubledValues(admission.SocialChanges))
                .WithName(x => DisplayNameResolver.GetDisplayName(nameof(x.SocialChanges)))
                .WithMessage(Validationmessages.NoDoubledValuesAreAllowed);

            this.RuleFor(x => x.HousingTypeBeforeAdmission)
                .Must((admission, ctx) => !(admission.HousingTypeBeforeAdmission == AdmissionLocation.OtherAl &&
                                            string.IsNullOrEmpty(admission.OtherHousingType)))
                .WithMessage(Validationmessages.TextAreaEnterAValue);

            this.RuleFor(x => x.HousingReason)
                .Must((admission, ctx) => !(admission.HousingReason == HousingReason.OtherHr &&
                                            string.IsNullOrEmpty(admission.HousingReasonOther)))
                .WithMessage(Validationmessages.TextAreaEnterAValue);
        }


        private string GetPersonName(string id, StatLpReport report)
        {
            Person person = report.Persons.Where(x => x.Id == id).FirstOrDefault();
            if (person != null)
            {
                return person.GetDisplayName();
            }

            return "Unbekannt";
        }



        private bool ContainsDoubledValues<T>(IEnumerable<T> values)
        {
            var doubledQuery = values.GroupBy(x => x)
                .Where(x => x.Count() > 1)
                .Select(group => group.Key);

            return !doubledQuery.Any();
        }
    }


}
