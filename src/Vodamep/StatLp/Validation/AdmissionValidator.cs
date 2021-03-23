using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Data;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class AdmissionValidator : AbstractValidator<Admission>
    {
        private readonly StatLpReport _parentReport;
 
        public AdmissionValidator(StatLpReport parentReport)
        {
            _parentReport = parentReport;

            this.RuleFor(x => x.HousingTypeBeforeAdmission).NotEmpty();
            this.RuleFor(x => x.MainAttendanceRelation).NotEmpty();
            this.RuleFor(x => x.MainAttendanceCloseness).NotEmpty();
            this.RuleFor(x => x.HousingReason).NotEmpty();

            //ungültige werte
            var regex0 = new Regex(@"^[-,.a-zA-ZäöüÄÖÜß\(\) ][-,.a-zA-ZäöüÄÖÜß\(\) ]*[-,.a-zA-ZäöüÄÖÜß\(\) ]$");
            this.RuleFor(x => x.OtherHousingType).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.OtherHousingType)).WithMessage(x => Validationmessages.InvalidValueAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));
            this.RuleFor(x => x.PersonalChangeOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.PersonalChangeOther)).WithMessage(x => Validationmessages.InvalidValueAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));
            this.RuleFor(x => x.SocialChangeOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.SocialChangeOther)).WithMessage(x => Validationmessages.InvalidValueAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));
            this.RuleFor(x => x.HousingReasonOther).Matches(regex0).Unless(x => string.IsNullOrEmpty(x.HousingReasonOther)).WithMessage(x => Validationmessages.InvalidValueAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.OtherHousingType).MaximumLength(30).WithMessage(x => Validationmessages.TextTooLongAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));
            this.RuleFor(x => x.PersonalChangeOther).MaximumLength(30).WithMessage(x => Validationmessages.TextTooLongAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));
            this.RuleFor(x => x.SocialChangeOther).MaximumLength(30).WithMessage(x => Validationmessages.TextTooLongAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));
            this.RuleFor(x => x.HousingReasonOther).MaximumLength(30).WithMessage(x => Validationmessages.TextTooLongAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));
            
            this.RuleFor(x => new { x.LastPostcode, x.LastCity }).Must(x => Postcode_CityProvider.Instance.IsValid($"{x.LastPostcode} {x.LastCity}")).WithMessage(x => Validationmessages.WrongPostCodeAdmission(_parentReport.FromD.ToShortDateString(), x.PersonId));

            this.RuleFor(x => x.PersonalChanges)
                .Must((admission, ctx) => ContainsDoubledValues(admission.PersonalChanges))
                .WithMessage(Validationmessages.NoDoubledValuesAreAllowed);

            this.RuleFor(x => x.SocialChanges)
                .Must((admission, ctx) => ContainsDoubledValues(admission.SocialChanges))
                .WithMessage(Validationmessages.NoDoubledValuesAreAllowed);
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
