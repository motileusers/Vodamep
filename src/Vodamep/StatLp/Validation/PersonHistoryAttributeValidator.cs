using System;
using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryAttributeValidator : AbstractValidator<StatLpReportHistory>
    {
        private static DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public PersonHistoryAttributeValidator()
        {
            this.RuleFor(x => x).Custom((a, ctx) =>
            {
                var sendMessage = a.StatLpReport;

                // only one admission type is allowed
                var admissionAttributes = sendMessage.Attributes.Where(x => x.AttributeType == AttributeType.AdmissionType).ToArray();
                if (admissionAttributes.Count() > 1)
                {
                    ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAdmissionAttributeMultipleChanged(admissionAttributes.First().PersonId, admissionAttributes.First().FromD.ToShortDateString()));
                }

                var lastMessage = a.StatLpReports.Where(b => b.FromD <= a.StatLpReport.FromD).OrderByDescending(y => y.FromD).FirstOrDefault();

                if (lastMessage == null)
                {
                    return;
                }

                //attributes must not be resend
                foreach (var sendMessageAttribute in sendMessage.Attributes)
                {
                    var lastMessageAttribute = lastMessage.Attributes.FirstOrDefault(c =>
                        sendMessageAttribute.PersonId == c.PersonId && sendMessageAttribute.AttributeType == c.AttributeType);

                    if (lastMessageAttribute != null && sendMessageAttribute.Value == lastMessageAttribute.Value)
                    {
                        ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAttributeAlreadySent(displayNameResolver.GetDisplayName(sendMessageAttribute.AttributeType.ToString()),
                            sendMessageAttribute.PersonId,
                            displayNameResolver.GetDisplayName(sendMessageAttribute.Value),
                            lastMessageAttribute.FromD.ToShortDateString()));
                    }
                }

            });
        }
    }
}