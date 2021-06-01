using System;
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
                var sentMessage = x.StatLpReport;

                var futureMessages = x.StatLpReports.Where(y => y.FromD >= x.StatLpReport.FromD).OrderBy(y => y.FromD);
                var nextFutureMessage = futureMessages.FirstOrDefault();
                var historyMessages = x.StatLpReports.Where(y => y.FromD <= x.StatLpReport.FromD)
                    .OrderByDescending(y => y.FromD);
                var lastHistoryMessage = historyMessages.FirstOrDefault();

                if (lastHistoryMessage != null && nextFutureMessage == null)
                {
                    if (sentMessage.FromD.AddMonths(-1) > lastHistoryMessage.FromD)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportPersonHistoryMissingReports(
                                lastHistoryMessage.ToD.AddDays(1).ToShortDateString(),
                                sentMessage.FromD.AddDays(-1).ToShortDateString())));
                    }
                }

                var personHistories = new List<PersonHistory>();

                foreach (var person in sentMessage.Persons)
                {
                    var personHistory = new PersonHistory();
                    personHistory.PersonId = person.Id;

                    var admissions = new List<Admission>();
                    var stays = new List<Stay>();
                    var leavings = new List<Leaving>();

                    // Die von den History Reports
                    foreach (var statLpReport in x.StatLpReports.OrderBy(sl => sl.FromD))
                    {
                        admissions.AddRange(statLpReport.Admissions.Where(ad => ad.PersonId == person.Id));
                        stays.AddRange(statLpReport.Stays.Where(ad => ad.PersonId == person.Id));
                        leavings.AddRange(statLpReport.Leavings.Where(ad => ad.PersonId == person.Id));
                    }

                    personHistory.Admissions.AddRange(admissions.OrderByDescending(st => st.ValidD));
                    personHistory.Stays.AddRange(stays.OrderByDescending(st => st.FromD));
                    personHistory.Leavings.AddRange(leavings.OrderByDescending(st => st.ValidD));

                    personHistories.Add(personHistory);
                }

                foreach (var personHistory in personHistories)
                {
                    var sendMessageAdmission = sentMessage.Admissions.FirstOrDefault(a => a.PersonId == personHistory.PersonId);
                    var sendMessageStay = sentMessage.Stays.FirstOrDefault(a => a.PersonId == personHistory.PersonId);
                    var sendMessageLeaving = sentMessage.Leavings.FirstOrDefault(a => a.PersonId == personHistory.PersonId);

                    Admission admission = null;
                    Leaving leaving = null;
                    Stay enduringStay = sendMessageStay != null ? new Stay(sendMessageStay) : null;



                    // #Vorherige Aufnahmen:
                    // Das Aufnahmedatum entspricht dem Stay, akuteller Ansatz,
                    // für alle neuen, ab jetzt gemeldeten Nachrichten
                    if (sendMessageAdmission != null && 
                        sendMessageStay != null && 
                        sendMessageAdmission.ValidD == sendMessageStay.FromD)
                    {
                        admission = sendMessageAdmission;
                    }

                    // #Vorherige Aufnahmen:
                    // Die Aufnahme lag vor dem Tag, als überhaupt Meldungen an connexia gesendet wurden,
                    // dieser Ansatz wird noch diskutiert. Soll das überhaupt möglich sein?
                    if (admission == null &&
                        sendMessageAdmission != null &&
                        sendMessageStay != null &&
                        sendMessageAdmission.ValidD <= sendMessageStay.FromD)
                    {
                        admission = sendMessageAdmission;
                    }



                    enduringStay = GetOverallStay(personHistory, enduringStay, sendMessageStay);

                    if (enduringStay != null)
                    {
                        if (sendMessageLeaving != null && sendMessageLeaving.ValidD == enduringStay.ToD)
                        {
                            leaving = sendMessageLeaving;
                        }

                        if (leaving == null)
                        {
                            leaving = personHistory.Leavings.FirstOrDefault(l => l.ValidD == enduringStay.ToD);
                        }
                    }


                    if (admission == null && enduringStay != null)
                    {
                        // Wir suchen die letzte Admission, vor dem eigentlichen Aufenthaltstart
                        List<Admission> admissions = personHistory.Admissions.OrderBy(a => a.ValidD).ToList();
                        foreach (Admission currentAdmission in admissions)
                        {
                            if (currentAdmission.ValidD == enduringStay.FromD)
                            {
                                // #Vorherige Aufnahmen:
                                // Das Aufnahmedatum entspricht dem Stay, akuteller Ansatz,
                                // für alle neuen, ab jetzt gemeldeten Nachrichten
                                admission = currentAdmission;
                            }

                            if (currentAdmission.ValidD < enduringStay.FromD)
                            {
                                // #Vorherige Aufnahmen:
                                // Die Aufnahme lag vor dem Tag, als überhaupt Meldungen an connexia gesendet wurden,
                                // dieser Ansatz wird noch diskutiert. Soll das überhaupt möglich sein?
                                admission = currentAdmission;
                            }

                        }
                    }

                    //doppelte aufnahmen
                    if (enduringStay == null && sendMessageAdmission != null && personHistory.Admissions.Any())
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                Validationmessages.StatLpReportAlreadyExistingAdmission(
                                    personHistory.PersonId,
                                    personHistory.Admissions.First().ValidD.ToShortDateString()
                                )));

                    }

                    //erneute aufnahme
                    if (sendMessageAdmission != null)
                    {
                       var previousEnduringStay = GetOverallStay(personHistory, null, sendMessageStay);

                       if (previousEnduringStay != null && sendMessageAdmission.ValidD > previousEnduringStay.ToD)
                       {
                           Leaving leavingResentMessage = sendMessageLeaving;

                           if (leavingResentMessage == null || leavingResentMessage.ValidD != previousEnduringStay.ToD)
                           {
                               leavingResentMessage = personHistory.Leavings.FirstOrDefault(l =>l.ValidD == previousEnduringStay.ToD);
                           }

                           if (leavingResentMessage == null)
                           {
                               // fehlende Aufnahme
                               ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                   Validationmessages.StatLpReportNoLeavingWhenAdmissionIsResent(
                                       personHistory.PersonId,
                                       sendMessageAdmission.ValidD.ToShortDateString()
                                   )));
                           }
                       }

                    }

                    if (admission == null)
                    {
                        //leermeldungen ignorieren
                        if ((sendMessageAdmission != null || personHistory.Admissions.Any()) ||
                            (sendMessageStay != null || personHistory.Stays.Any()) ||
                            (sendMessageLeaving != null || personHistory.Leavings.Any()))
                        {
                            DateTime date = enduringStay != null ? enduringStay.FromD :
                                sendMessageStay != null ? sendMessageStay.FromD : DateTime.MinValue;

                            // fehlende Aufnahme
                            ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                Validationmessages.StatLpReportNoAdmission(
                                    personHistory.PersonId,
                                    date.ToShortDateString()
                                )));
                        }
                    }

                    //leavings
                    if (enduringStay != null && enduringStay.ToD != sentMessage.ToD && leaving == null)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportNoLeaving(
                                personHistory.PersonId,
                                enduringStay.ToD.ToShortDateString()
                            )));
                    }
                }
            });
        }

        private Stay GetOverallStay(PersonHistory personHistory, Stay enduringStay, Stay sendMessageStay)
        {
            var admissionDate = GetAdmissionDate(sendMessageStay, personHistory);

            //find first stay 
            var stayIndex = 0;
            foreach (var stay in personHistory.Stays)
            {
                if (enduringStay == null)
                {
                    enduringStay = stay;
                }

                if (stay.ToD != admissionDate)
                {
                    break;
                }

                if (stay.FromD <= admissionDate)
                {
                    enduringStay.FromD = stay.FromD;
                }

                if (stayIndex++ < personHistory.Stays.Count - 1)
                {
                    admissionDate = enduringStay.FromD.AddDays(-1);
                }
            }

            return enduringStay;
        }

        private static DateTime GetAdmissionDate(Stay sendMessageStay, PersonHistory personHistory)
        {
            DateTime admissionDate = DateTime.MinValue;
            if (sendMessageStay != null)
            {
                admissionDate = sendMessageStay.FromD;
            }

            if (personHistory.Stays.Any())
            {
                if (admissionDate == DateTime.MinValue)
                {
                    if (personHistory.Stays.Count == 1)
                    {
                        admissionDate = personHistory.Stays.First().FromD;
                    }
                    else
                    {
                        admissionDate = personHistory.Stays.First().FromD.AddDays(-1);
                    }
                }
                else
                {
                    admissionDate = admissionDate.AddDays(-1);
                }
            }

            return admissionDate;
        }
    }
}