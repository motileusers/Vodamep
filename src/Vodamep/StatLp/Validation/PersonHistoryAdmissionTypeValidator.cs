using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryAdmissionTypeValidator : AbstractValidator<StatLpReportHistory>
    {
        public PersonHistoryAdmissionTypeValidator()
        {
            this.RuleFor(x => x).Custom((a, ctx) =>
            {
                var sendMessage = a.StatLpReport;

                var lastMessage = a.StatLpReports.Where(b => b.FromD <= a.StatLpReport.FromD).OrderByDescending(y => y.FromD).FirstOrDefault();

                if (lastMessage == null)
                {
                    return;
                }

                var sendMessageAdmissionType = sendMessage.Attributes.FirstOrDefault(c => c.AttributeType == AttributeType.AdmissionType);
                var existingMessageAdmissionType = lastMessage.Attributes.FirstOrDefault(c => c.AttributeType == AttributeType.AdmissionType);

                if (sendMessageAdmissionType != null && existingMessageAdmissionType != null &&
                    existingMessageAdmissionType.Value == "Daueraufnahme" && 
                    (sendMessageAdmissionType.Value == "Urlaub von der Pflege" || sendMessageAdmissionType.Value == "Übergangspflege"))
                {
                    ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAdmissionAttributeNoChangeFromLongTimeCarePossible(sendMessageAdmissionType.PersonId, sendMessageAdmissionType.Value));
                }

            });
        }
    }
}