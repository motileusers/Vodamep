using System;
using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryPersonValidator : AbstractValidator<StatLpReportHistory>
    {
        public PersonHistoryPersonValidator()
        {
            this.RuleFor(x => x).Custom((a, ctx) =>
            {
                var sendMessage = a.StatLpReport;
                var lastMessage = a.StatLpReports.Where(b => b.FromD <= a.StatLpReport.FromD).OrderByDescending(y => y.FromD).FirstOrDefault();

                if (lastMessage == null)
                {
                    return;
                }

                //attributes must not be resend
                foreach (var sendMessagePerson in sendMessage.Persons)
                {
                    var lastMessagePerson = lastMessage.Persons.FirstOrDefault(x => x.Id == sendMessagePerson.Id);

                    if (lastMessagePerson == null)
                    {
                        continue;
                    }

                    if (sendMessagePerson.Gender != lastMessagePerson.Gender)
                    {
                        ctx.AddFailure(nameof(sendMessagePerson.Gender), Validationmessages.StatLpReportPersonHistoryGenderAttributeChanged("Geschlecht", sendMessagePerson.Id, sendMessage.FromD.ToShortDateString()));
                    }

                    if (sendMessagePerson.BirthdayD != lastMessagePerson.BirthdayD)
                    {
                        ctx.AddFailure(nameof(sendMessagePerson.BirthdayD), Validationmessages.StatLpReportPersonHistoryGenderAttributeChanged("Geburtsdatum", sendMessagePerson.Id, sendMessage.FromD.ToShortDateString()));
                    }

                }

            });
        }
    }
}