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
        private DisplayNameResolver displayNameResolver = new DisplayNameResolver();



        /// <summary>
        /// Prüfung der Reihenfolge und Datumsabhängikeiten von Aufnahmen, Entlassungen, Attributen, ...
        /// </summary>
        public PersonHistoryMessageOrderValidator()
        {
            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                var sentReport = x.StatLpReport;


                // Fehlende Meldungen in der Historie ermitteln
                var futureMessages = x.StatLpReports.Where(y => y.FromD >= x.StatLpReport.FromD).OrderBy(y => y.FromD);
                var nextFutureMessage = futureMessages.FirstOrDefault();
                var historyMessages = x.StatLpReports.Where(y => y.FromD <= x.StatLpReport.FromD)
                    .OrderByDescending(y => y.FromD);
                var lastHistoryMessage = historyMessages.FirstOrDefault();

                if (lastHistoryMessage != null && nextFutureMessage == null)
                {
                    if (sentReport.FromD.AddMonths(-1) > lastHistoryMessage.FromD)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportPersonHistoryMissingReports(
                                lastHistoryMessage.ToD.AddDays(1).ToShortDateString(),
                                sentReport.FromD.AddDays(-1).ToShortDateString())));
                    }
                }



                // Personengeschichten mit monatsübergreifenden Aufenthalten erzeugen
                List<PersonHistory> personHistories = CreatePersonHistories(x.StatLpReport, x.StatLpReports);



                // Personengeschichte prüfen
                foreach (PersonHistory personHistory in personHistories)
                {
                    foreach (StayInfo stayInfo in personHistory.StayInfos)
                    {
                        // Die Attribute werden nicht geprüft, wenn es gröberer Probleme im 
                        // Meldungsaufbau gibt. Da gibt's dann nur zu viele verwirrende Fehlermeldungen
                        bool checkAttributes = true;

                        // Mehrfache Aufnahmen
                        if (stayInfo.Admissions.Count > 1)
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                    Validationmessages.StatLpReportMultipleAdmissions(
                                        personHistory.PersonId,
                                        stayInfo.From.ToShortDateString(),
                                        stayInfo.To.ToShortDateString()
                                    )));

                            checkAttributes = false;
                        }

                        // Keine Aufnahme
                        if (stayInfo.Admission == null)
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                Validationmessages.StatLpReportNoAdmission(
                                    personHistory.PersonId,
                                    stayInfo.From.ToShortDateString()
                                )));
                        }


                        // Fehlende Entlassung, wenn der Aufenthalt vor dem Monatsende endet
                        if (!stayInfo.To.IsLastDateInMonth())
                        {
                            if (stayInfo.Leaving == null)
                            {
                                // fehlende Aufnahme
                                ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                    Validationmessages.StatLpReportNoLeaving(
                                        personHistory.PersonId,
                                        stayInfo.To.ToShortDateString()
                                    )));
                            }
                        }


                        // Gleiche Attribut-Meldungen innerhalb eines Aufenthalts

                        if (checkAttributes)
                        {
                            Dictionary<AttributeType, Model.Attribute> lastValues = new Dictionary<AttributeType, Model.Attribute>();

                            foreach (Model.Attribute currentAttribute in stayInfo.Attributes)
                            {
                                if (!lastValues.ContainsKey(currentAttribute.AttributeType))
                                {
                                    lastValues.Add(currentAttribute.AttributeType, currentAttribute);
                                }
                                else
                                {
                                    Model.Attribute lastAttribute = lastValues[currentAttribute.AttributeType];
                                    if (lastAttribute.Value == currentAttribute.Value)
                                    {
                                        ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAttributeAlreadySent(displayNameResolver.GetDisplayName(currentAttribute.AttributeType.ToString()),
                                            currentAttribute.PersonId,
                                            displayNameResolver.GetDisplayName(currentAttribute.Value),
                                            lastAttribute.FromD.ToShortDateString(),
                                            currentAttribute.FromD.ToShortDateString()));
                                    }

                                    lastValues[currentAttribute.AttributeType] = currentAttribute;
                                }
                            }
                        }
                    }
                }


                // Die letzte Meldung (vor der akutell gesendeten) wird noch speziell geprüft
                StatLpReport previousReport = historyMessages.FirstOrDefault();
                if (previousReport != null)
                {
                    foreach (Stay previousStay in previousReport.Stays)
                    {
                        if (previousStay.ToD.IsLastDateInMonth())
                        {
                            // Der Aufenthalt in der vorherigen Meldung dauert genau bis num Monatsende
                            // Es konnte also im Vormonat nicht geprüft werden, ob eine Enlasstung notwendig gewesen wäre
                            
                            // Wir ermittlen also in der Meldung des nächsten Monats, ob ein nachfolgender Aufenthalt vorhanden ist
                            Stay firstStayInNextMonth = sentReport.Stays.Where(f => f.PersonId == previousStay.PersonId)
                                                                        .OrderBy(f => f.ToD)
                                                                        .FirstOrDefault();

                            // Dann brauchen wir eine evtl. Enlassung aus dem Vormonat
                            Leaving previousLeaving = previousReport.Leavings.Where(l => l.ValidD == previousStay.ToD &&
                                                                                         l.PersonId == previousStay.PersonId)
                                                                             .FirstOrDefault();

                            if (firstStayInNextMonth == null)
                            {
                                if (previousLeaving == null)
                                {
                                    // Wenn es keinen Aufenthalt im Folgemonat gibt,
                                    // hätte der Aufenthalt vom Vormonat abgeschlossen werden müssen

                                    ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                    Validationmessages.StatLpReportNoLeaving(
                                        previousStay.PersonId,
                                        previousStay.ToD.ToShortDateString()
                                    )));
                                }
                            }



                            if (firstStayInNextMonth != null)
                            {
                                // Dann brauchen wir auch eine evtl. Neuaufnahme vom Folgemonat
                                Admission nextAdmission = sentReport.Admissions.Where(l => l.ValidD == firstStayInNextMonth.FromD &&
                                                                                       l.PersonId == previousStay.PersonId)
                                                                           .FirstOrDefault();
                                if (nextAdmission != null)
                                {
                                    // Wenn es einen Aufenthalt im Folgemonat gibt, dann muss jedoch sichergestellt sein,
                                    // dass es keine Neuaufnahme ist.
                                    // Denn wenn die Person im nächsten Monat neu aufgenommen worden ist, hätte der Aufenthalt im
                                    // Vormonat ebenso abgeschlossen werden müssen.

                                    if (previousLeaving == null)
                                    {
                                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                        Validationmessages.StatLpReportNoLeaving(
                                            previousStay.PersonId,
                                            previousStay.ToD.ToShortDateString()
                                        )));
                                    }
                                }
                            }
                        }
                    }
                }

            });
        }



        /// <summary>
        /// Objekt mit Personengeschichten erstellen
        /// </summary>
        private List<PersonHistory> CreatePersonHistories(StatLpReport sentReport, IEnumerable<StatLpReport> existingReports)
        {
            List<PersonHistory> personHistories = new List<PersonHistory>();

            // Wir schauen nur die Personen aus dem aktuellen Report an
            foreach (var person in sentReport.Persons)
            {
                var personHistory = new PersonHistory();
                personHistory.PersonId = person.Id;

                var admissions = new List<Admission>();
                var stays = new List<Stay>();
                var leavings = new List<Leaving>();
                var attributes = new List<Model.Attribute>();

                // Wir sammeln alle Aufnahmen, Aufnahmen und Enlassungen pro Person und sortieren nach Aufnahmedatum
                foreach (var existingReport in existingReports.OrderBy(sl => sl.FromD))
                {
                    admissions.AddRange(existingReport.Admissions.Where(ad => ad.PersonId == person.Id));
                    stays.AddRange(existingReport.Stays.Where(ad => ad.PersonId == person.Id));
                    leavings.AddRange(existingReport.Leavings.Where(ad => ad.PersonId == person.Id));
                    attributes.AddRange(existingReport.Attributes.Where(ad => ad.PersonId == person.Id));
                }

                personHistory.Admissions.AddRange(admissions.OrderByDescending(st => st.ValidD));
                personHistory.Stays.AddRange(stays.OrderByDescending(st => st.FromD));
                personHistory.Leavings.AddRange(leavings.OrderByDescending(st => st.ValidD));
                personHistory.Attributes.AddRange(attributes.OrderByDescending(st => st.FromD));

                personHistories.Add(personHistory);
            }

            CreateMonthlyStays(personHistories, sentReport);


            return personHistories;
        }



        /// <summary>
        /// Monatsübergreifende Aufenhalte erzeugen
        /// </summary>
        private void CreateMonthlyStays(List<PersonHistory> personHistories, StatLpReport sentReport)
        {
            // Monatsübergreifende Aufenthalte erzeugen
            foreach (PersonHistory personHistory in personHistories)
            {
                DateTime currentDate = sentReport.ToD;

                while (true)
                {
                    // Liefert eine monatsübergreifenden Aufenthalt
                    StayInfo stayInfo = GetStayInfo(currentDate, personHistory, sentReport);

                    if (stayInfo == null)
                        // Kein weiterer Stay mehr
                        break;
                    else
                    {
                        // Alles ok, hinzufügen
                        personHistory.StayInfos.Add(stayInfo);

                        // Nächsten Stay
                        currentDate = stayInfo.From.AddDays(-1);

                    }

                    // Sicherheitsbreak
                    if (personHistory.StayInfos.Count > 1000)
                        throw new Exception("Problem im Loop");
                }


                // Aufenthalte verketten, damit auf den vorherigen Aufenthalt zugegriffen werden kann
                // Zusatzinformationen ermitteln
                for (int i = 0; i < personHistory.StayInfos.Count; i++)
                {
                    StayInfo currentStay = personHistory.StayInfos[i];

                    AddAdmissionsToStayInfo(currentStay, personHistory, sentReport);
                    AddLeavingsToStayInfo(currentStay, personHistory, sentReport);

                    // Wenn ein vorheriger Aufenthalt existiert
                    // = nächster in der Liste
                    if (i < personHistory.StayInfos.Count - 1)
                    {
                        // Nur beim letzten nicht
                        StayInfo previousStay = personHistory.StayInfos[i + 1];
                        currentStay.PreviousStay = previousStay;
                    }
                }

                // Vorherigen Aufenthalt zuweisen
                for (int i = 0; i < personHistory.StayInfos.Count; i++)
                {
                    if (i > 0)
                    {
                        StayInfo currentStay = personHistory.StayInfos[i];
                        currentStay.NextStay = personHistory.StayInfos[i - 1];
                    }

                }
            }


        }



        /// <summary>
        /// Ganzen Aufenhaltszeitraum mit Start, Ende, Aufnahmen und Entlassungen ermmitteln
        /// </summary>
        /// <param name="startDate">Start Datum, ab dem gesucht werden soll</param>
        private StayInfo GetStayInfo(DateTime startDate, PersonHistory personHistory, StatLpReport sentReport)
        {
            StayInfo result = null;

            List<Stay> staysToSearch = new List<Stay>();

            staysToSearch.AddRange(sentReport?.Stays.Where(x => x.PersonId == personHistory.PersonId));
            staysToSearch.AddRange(personHistory.Stays);

            // Alle Aufenthalte, die vor dem dem Start Datum lagen
            List<Stay> tempStays = staysToSearch.Where(x => x.ToD <= startDate)
                                                .OrderByDescending(x => x.FromD)
                                                .ToList();

            for (int i = 0; i < tempStays.Count; i++)
            {
                // Zeitraum für den ersten Aufenthalt
                if (result == null)
                {
                    result = new StayInfo()
                    {
                        To = tempStays[i].ToD,
                        From = tempStays[i].FromD

                    };
                }

                Stay currentStay = tempStays[i];
                Stay nextStay;

                // Nachfolgenden Aufenthalt ermittlen
                if (tempStays.Count > i + 1)
                {
                    nextStay = tempStays[i + 1];
                }
                else
                {
                    nextStay = null;
                }


                AddActualAdmissionToStayInfo(result, personHistory, sentReport);
                AddActualLeavingToStayInfo(result, personHistory, sentReport);

                if (nextStay == null)
                {
                    // Kein nachfolgender Aufenthalt mehr, Aufenthalt Start ist das akutelle Start Datum vom Aufenthalt 
                    result.From = currentStay.FromD;

                }
                else
                {
                    // Es kommt noch ein Aufenthalt, wir schauen, ob der aktuelle Aufenthalt lt. Datum nahtlos anschließt
                    if (nextStay.ToD == currentStay.FromD.AddDays(-1))
                    {
                        // Es kann sein, dass ein Aufenhalt genau am Monatsende wechselt, und davor bis Monatsende ebenfalls ein Aufenthalt vorhanden war
                        // Das erkennen wir, wenn eine Aufnahme zum Start vom Aufenthalt vorhanden war

                        if (result.Admission == null)
                        {
                            // Es gibt keine Aufnahme und der nächste Aufenthalt schließt nahtlos an
                            // Wir verlängern den aktuellen Aufenthalt
                            result.From = nextStay.FromD;
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
                        result.From = currentStay.FromD;

                        break;
                    }
                }
            }


            // Schauen, ob es sich um einen aktiven Aufenthalt handelt
            if (result != null)
            {
                if (result.To == sentReport.ToD)
                {
                    if (result.Leaving == null)
                    {
                        result.IsActualStay = true;
                    }
                }

                // Attribute dem aktuellen Stay hinzufügen
                AddAttributesToStayInfo(result, personHistory, sentReport);

            }





            return result;
        }



        /// <summary>
        /// Mögliche Aufnahmen für einen Stay ermitteln, im ganzen Bereich, nur zur Fehlerprüfung
        /// Normalfall: 1 Stay, 1 Admission
        /// </summary>
        private void AddAdmissionsToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            stayInfo.Admissions.AddRange(sentReport.Admissions.Where(x => x.ValidD >= stayInfo.From &&
                                                                          x.ValidD <= stayInfo.To &&
                                                                          x.PersonId == personHistory.PersonId));

            stayInfo.Admissions.AddRange(personHistory.Admissions.Where(x => x.ValidD >= stayInfo.From &&
                                                                             x.ValidD <= stayInfo.To));
        }


        /// <summary>
        /// Alle Attribute, die in einem Stay gesendet wurden
        /// </summary>
        private void AddAttributesToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            stayInfo.Attributes.AddRange(sentReport.Attributes.Where(x => x.FromD >= stayInfo.From &&
                                                                          x.FromD <= stayInfo.To &&
                                                                          x.PersonId == personHistory.PersonId));

            stayInfo.Attributes.AddRange(personHistory.Attributes.Where(x => x.FromD >= stayInfo.From &&
                                                                             x.FromD <= stayInfo.To));
        }


        /// <summary>
        /// Mögliche Entassungen für einen Stay ermitteln, im ganzen Bereich, nur zur Fehlerprüfung
        /// Normalfall: 1 Stay, 1 Leaving
        /// </summary>
        private void AddLeavingsToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            stayInfo.Leavings.AddRange(sentReport.Leavings.Where(x => x.ValidD >= stayInfo.From &&
                                                                      x.ValidD <= stayInfo.To &&
                                                                      x.PersonId == personHistory.PersonId));

            stayInfo.Leavings.AddRange(personHistory.Leavings.Where(x => x.ValidD >= stayInfo.From &&
                                                                         x.ValidD <= stayInfo.To));
        }

        /// <summary>
        /// Akutelle Aufnahme für einen Stay ermitteln, genau für das Stay Start Datum
        /// </summary>
        private void AddActualAdmissionToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            stayInfo.Admission = sentReport.Admissions.Where(x => x.ValidD == stayInfo.From &&
                                                                  x.PersonId == personHistory.PersonId)
                                                      .FirstOrDefault();

            if (stayInfo.Admission == null)
                stayInfo.Admission = personHistory.Admissions.Where(x => x.ValidD == stayInfo.From).FirstOrDefault();

        }


        /// <summary>
        /// Entlassung für einen Stay ermitteln
        /// </summary>
        private void AddActualLeavingToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            stayInfo.Leaving = sentReport.Leavings.Where(x => x.ValidD == stayInfo.To &&
                                                              x.PersonId == personHistory.PersonId)
                                                  .FirstOrDefault();

            if (stayInfo.Leaving == null)
                stayInfo.Leaving = personHistory.Leavings.Where(x => x.ValidD == stayInfo.To).FirstOrDefault();

        }


    }
}