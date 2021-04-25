using System;
using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class AttributeSameValueHistoryValidator : AbstractValidator<StatLpReportHistory>
    {
        public AttributeSameValueHistoryValidator()
        {
            this.RuleFor(x => x).Custom((a, ctx) =>
            {
                var sendMessage = a.StatLpReport;

                var lastMessage = a.StatLpReports.Where(b => b.FromD <= a.StatLpReport.FromD).OrderByDescending(y => y.FromD).FirstOrDefault();

                if (lastMessage == null)
                {
                    return;
                }

                foreach (var sendMessageAttribute in sendMessage.Attributes)
                {
                    var lastMessageAttribute = lastMessage.Attributes.FirstOrDefault(c =>
                        sendMessageAttribute.PersonId == c.PersonId && sendMessageAttribute.AttributeType == c.AttributeType);

                    if (lastMessageAttribute != null && sendMessageAttribute.Value == lastMessageAttribute.Value)
                    {
                        ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAttributeAlreadySent(sendMessageAttribute.AttributeType.ToString(), sendMessageAttribute.PersonId,
                            sendMessageAttribute.Value, lastMessageAttribute.FromD.ToShortDateString()));
                    }
                }
            

            });
        }
    }
}