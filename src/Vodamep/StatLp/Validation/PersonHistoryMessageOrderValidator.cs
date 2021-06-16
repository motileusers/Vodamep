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
                    if (person.Id == "1097070658")
                    {
                    }

                    var personHistory = new PersonHistory();
                    personHistory.PersonId = person.Id;

                    var admissions = new List<Admission>();
                    var stays = new List<Stay>();
                    var leavings = new List<Leaving>();

                    // Wir sammeln alle Aufnahmen, Aufnahmen und Enlassungen pro Person und sortieren nach Aufnahmedatum
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


                // Wir prüfen die Personengeschichte pro Person
                foreach (PersonHistory personHistory in personHistories)
                {
                    if (personHistory.PersonId == "1097070658")
                    {
                    }


                    // Zuerst schauen wir uns die Aufnahmen, in der aktuellen Meldung an
                    Admission sentMessageAdmission = sentMessage.Admissions.FirstOrDefault(a => a.PersonId == personHistory.PersonId);
                    Stay sentMessageStay = sentMessage.Stays.FirstOrDefault(a => a.PersonId == personHistory.PersonId);
                    Leaving sentMessageLeaving = sentMessage.Leavings.FirstOrDefault(a => a.PersonId == personHistory.PersonId);

                    Admission admission = null;
                    Leaving leaving = null;

                    if (sentMessageStay != null)
                    {
                        // Die Aufnahme erfolgte in diesem Monat
                        if (sentMessageAdmission != null &&
                            sentMessageAdmission.ValidD == sentMessageStay.FromD)
                        {
                            admission = sentMessageAdmission;
                        }

                        // Ganzen Aufenhaltszeitraum für den letzten Aufenthalt ermitteln
                        Stay enduringStay = GetOverallStay(sentMessageStay.ToD, personHistory, sentMessageStay);

                        if (enduringStay != null)
                        {
                            if (sentMessageLeaving != null && sentMessageLeaving.ValidD == enduringStay.ToD)
                            {
                                leaving = sentMessageLeaving;
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
                                    // Das Aufnahmedatum entspricht dem Stay, akuteller Ansatz,
                                    // für alle neuen, ab jetzt gemeldeten Nachrichten
                                    admission = currentAdmission;
                                }

                            }
                        }

                        // Doppelte Aufnahmen
                        if (sentMessageAdmission != null && personHistory.Admissions.Any())
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                    Validationmessages.StatLpReportAlreadyExistingAdmission(
                                        personHistory.PersonId,
                                        personHistory.Admissions.First().ValidD.ToShortDateString()
                                    )));

                        }

                        // Erneute Aufnahmen
                        if (sentMessageAdmission != null)
                        {
                            if (personHistory.PersonId == "1097070658")
                            {
                            }


                            Stay previousEnduringStay = GetOverallStay(enduringStay.FromD.AddDays(-1), personHistory, null);

                            if (previousEnduringStay != null && sentMessageAdmission.ValidD > previousEnduringStay.ToD)
                            {
                                Leaving leavingResentMessage = sentMessageLeaving;

                                if (leavingResentMessage == null || leavingResentMessage.ValidD != previousEnduringStay.ToD)
                                {
                                    leavingResentMessage = personHistory.Leavings.FirstOrDefault(l => l.ValidD == previousEnduringStay.ToD);
                                }

                                if (leavingResentMessage == null)
                                {
                                    // fehlende Aufnahme
                                    ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                        Validationmessages.StatLpReportNoLeavingWhenAdmissionIsResent(
                                            personHistory.PersonId,
                                            sentMessageAdmission.ValidD.ToShortDateString()
                                        )));
                                }
                            }

                        }

                        if (admission == null)
                        {
                            // Leermeldungen ignorieren
                            if ((sentMessageAdmission != null || personHistory.Admissions.Any()) ||
                                (sentMessageStay != null || personHistory.Stays.Any()) ||
                                (sentMessageLeaving != null || personHistory.Leavings.Any()))
                            {
                                DateTime date = enduringStay != null ? enduringStay.FromD :
                                    sentMessageStay != null ? sentMessageStay.FromD : DateTime.MinValue;

                                // Fehlende Aufnahme
                                ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                    Validationmessages.StatLpReportNoAdmission(
                                        personHistory.PersonId,
                                        date.ToShortDateString()
                                    )));
                            }
                        }

                        // Entlassungen
                        if (enduringStay != null && enduringStay.ToD != sentMessage.ToD && leaving == null)
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                Validationmessages.StatLpReportNoLeaving(
                                    personHistory.PersonId,
                                    enduringStay.ToD.ToShortDateString()
                                )));
                        }
                    }
                }
            });
        }



        /// <summary>
        /// Ganzen Aufenhaltszeitraum für den einen Monatsaufenthalt ermitteln
        /// </summary>
        private Stay GetOverallStay(DateTime startDate, PersonHistory personHistory, Stay currentStay)
        {
            Stay result = null;

            List<Stay> staysToSearch = new List<Stay>();


            // Liste für die Suche aufbauen
            if (currentStay != null)
                staysToSearch.Add(currentStay);

            staysToSearch.AddRange(personHistory.Stays);


            if (personHistory.PersonId == "1097070658")
            {
            }

            // Vorherige Aufnahmen ermitteln
            List<Stay> tempStays = staysToSearch.Where(x => x.ToD <= startDate)
                                                .OrderByDescending(x => x.FromD)
                                                .ToList();

            for (int i = 0; i < tempStays.Count; i++)
            {
                // Ok, wir können einen aufbauen
                if (result == null)
                {
                    result = new Stay()
                    {
                        ToD = tempStays[i].ToD
                    };
                }

                Stay recentStay = tempStays[i];
                Stay nextStay;

                // Nachfolgenden Aufenthalt ermittlen
                if (tempStays.Count > i + 2)
                {
                    nextStay = tempStays[i + 1];
                }
                else
                {
                    nextStay = null;
                }



                if (nextStay == null)
                {
                    // Kein nachfolgender Aufenthalt mehr, Aufenthalt Start ist das akutelle Start Datum vom Aufenthalt 
                    result.FromD = recentStay.FromD;
                }
                else
                {
                    // Es kommt noch ein Aufenthalt, wir schauen, ob der aktuelle Aufenthalt lt. Datum nahtlos anschließt
                    if (nextStay.ToD == recentStay.FromD.AddDays(-1))
                    {
                        // Es kann sein, dass ein Aufenhalt genau am Monatsende wechselt, und davor bis Monatsende ebenfalls ein Aufenthalt vorhanden war
                        // Das erkennen wir, wenn eine Aufnahme zum Start vom Aufenthalt vorhanden war
                        Admission admission = personHistory.Admissions.Where(x => x.ValidD == result.FromD).FirstOrDefault();

                        if (admission == null)
                        {
                            // Es gibt keine Aufnahme und der Aufenthalt schließt nahtlos an
                            // Wir verlängern den aktuellen Aufenthalt
                            result.FromD = nextStay.FromD;
                        }
                        else
                        {
                            // Neuaufnahme zu Monatsbeginn, wir beenden den Aufenthalt
                            break;
                        }
                    }
                    else
                    {
                        // Es fehlen Tage zwischen den Aufenthalten
                        // Wir gehen davon aus, dass der Aufenthalt unterbrochen wurde
                        result.FromD = recentStay.FromD;
                        break;
                    }
                }
            }

            return result;
        }

    }
}