using System;
using System.Collections.Generic;
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


                var groupedAttributes = sendMessage.Attributes.GroupBy(x => (x.PersonId, x.FromD, x.AttributeType))
                                                    .Select(g => (g.Key.PersonId, g.Key.FromD, g.Key.AttributeType, Count: g.Count()));

                foreach (var group in groupedAttributes)
                {
                    if (group.Count > 1)
                    {
                        ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAdmissionAttributeMultipleChanged(group.PersonId, group.FromD.ToShortDateString(), group.AttributeType.ToString()));
                    }
                }



                // Letzte Nachricht suchen
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