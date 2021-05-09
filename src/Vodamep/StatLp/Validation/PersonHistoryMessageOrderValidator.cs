using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryMessageOrderValidator : AbstractValidator<StatLpReportHistory>
    {
        public PersonHistoryMessageOrderValidator()
        {
            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                var sendMessage = x.StatLpReport;

                var futureMessages = x.StatLpReports.Where(y => y.FromD >= x.StatLpReport.FromD).OrderBy(y => y.FromD);
                var nextFutureMessage = futureMessages.FirstOrDefault();
                var historyMessages = x.StatLpReports.Where(y => y.FromD <= x.StatLpReport.FromD).OrderByDescending(y => y.FromD);
                var lastHistoryMessage = historyMessages.FirstOrDefault();

                if (lastHistoryMessage != null && nextFutureMessage == null)
                {
                    if (sendMessage.FromD.AddMonths(-1) > lastHistoryMessage.FromD)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportPersonHistoryMissingReports(
                                lastHistoryMessage.ToD.AddDays(1).ToShortDateString(),
                                sendMessage.FromD.AddDays(-1).ToShortDateString())));
                    }
                }

                foreach (var person in sendMessage.Persons)
                {

                    var lastAdmission = lastHistoryMessage?.Admissions.FirstOrDefault(s => s.PersonId == person.Id);
                    var lastStay = lastHistoryMessage?.Admissions.FirstOrDefault(s => s.PersonId == person.Id);
                    var lastLeaving = lastHistoryMessage?.Leavings.FirstOrDefault(s => s.PersonId == person.Id);

                    var currentAdmission = sendMessage.Admissions.FirstOrDefault(s => s.PersonId == person.Id);
                    var currentStay = sendMessage.Stays.FirstOrDefault(s => s.PersonId == person.Id);
                    var currentLeaving = sendMessage.Leavings.FirstOrDefault(l => l.PersonId == person.Id);

                    //Fehlende Aufnahme
                    if (currentStay != null && ((currentStay.FromD == sendMessage.FromD && currentAdmission == null && lastStay == null) ||
                        (currentStay.FromD > sendMessage.FromD && (currentAdmission == null || currentAdmission.ValidD != currentStay.FromD))))
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportNoAdmission(
                                person.Id,
                                currentStay.FromD.ToShortDateString()
                            )));
                    }

                    //doppelte aufnahme
                    if (currentAdmission != null && lastAdmission != null)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportAlreadyExistingAdmission(
                                person.Id,
                                lastHistoryMessage.Admissions.First(b => b.PersonId == person.Id).ValidD.ToShortDateString()
                            )));
                    }

                    //Fehlende Entlassung, wenn Aufenthalt nicht bis zum Ende des Monats dauert
                    if (currentStay != null && currentStay.ToD < sendMessage.ToD &&
                        (currentLeaving == null || currentLeaving.ValidD != currentStay.ToD))
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportNoLeaving(
                                person.Id,
                                currentStay.ToD.ToShortDateString()
                            )));
                    }

                    //keine entlassung
                    if (lastStay != null && currentStay == null && lastLeaving == null && currentLeaving == null)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportNoLeaving(
                                person.Id,
                                sendMessage.ToD.ToShortDateString()
                            )));
                    }
                }

            });
        }
    }
}