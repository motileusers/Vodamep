using System;
using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryAdmissionTypeValidator : AbstractValidator<StatLpReportHistory>
    {
        private static DisplayNameResolver displayNameResolver = new DisplayNameResolver();

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


                if (sendMessageAdmissionType != null && existingMessageAdmissionType != null)
                {
                    var sendMessageAdmissionTypeValue = (AdmissionType)Enum.Parse(typeof(AdmissionType), sendMessageAdmissionType.Value);
                    var existingMessageAdmissionTypeValue = (AdmissionType)Enum.Parse(typeof(AdmissionType), existingMessageAdmissionType.Value);

                    if (existingMessageAdmissionTypeValue == AdmissionType.ContinuousAt &&
                        (sendMessageAdmissionTypeValue == AdmissionType.HolidayAt ||
                         sendMessageAdmissionTypeValue == AdmissionType.TransitionalAt))
                    {
                        ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAdmissionAttributeNoChangeFromLongTimeCarePossible(sendMessageAdmissionType.PersonId, displayNameResolver.GetDisplayName(sendMessageAdmissionType.Value)));
                    }
                }

            });
        }
    }
}