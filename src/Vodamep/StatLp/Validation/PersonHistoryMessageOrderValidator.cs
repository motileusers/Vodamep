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
                List<StatLpReport> nextReports = x.StatLpReports.Where(y => y.FromD >= sentReport.FromD).OrderBy(y => y.FromD).ToList();
                StatLpReport nextReport = nextReports.FirstOrDefault();
                List<StatLpReport> previousReports = x.StatLpReports.Where(y => y.FromD <= sentReport.FromD).OrderByDescending(y => y.FromD).ToList();
                StatLpReport previousReport = previousReports.FirstOrDefault();

                if (previousReport != null && nextReport == null)
                {
                    if (sentReport.FromD.AddMonths(-1) > previousReport.FromD)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportPersonHistoryMissingReports(
                                previousReport.ToD.AddDays(1).ToShortDateString(),
                                sentReport.FromD.AddDays(-1).ToShortDateString())));
                    }
                }



                // Personengeschichten mit monatsübergreifenden Aufenthalten erzeugen
                Dictionary<string, PersonHistory> personHistoryDictionary = CreatePersonHistories(sentReport, x.StatLpReports);



                // Personengeschichte prüfen
                List<PersonHistory> historiesFromSentReport = personHistoryDictionary.Values.Where(h => h.IsFromSentReport == true).ToList();
                foreach (PersonHistory personHistory in historiesFromSentReport)
                {
                    foreach (StayInfo stayInfo in personHistory.StayInfos)
                    {
                        // Die Attribute werden nicht geprüft, wenn es gröbere Probleme im 
                        // Meldungsaufbau gibt. Da gibt's dann nur zu viele verwirrende Fehlermeldungen
                        bool checkAttributes = true;

                        // Mehrfache Aufnahmen
                        if (stayInfo.Admissions.Count > 1)
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                    Validationmessages.StatLpReportMultipleAdmissions(
                                        personHistory.GetPersonName(),
                                        stayInfo.From.ToShortDateString(),
                                        stayInfo.To.ToShortDateString()
                                    ))); ;

                            checkAttributes = false;
                        }

                        // Keine Aufnahme
                        if (stayInfo.Admission == null)
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                Validationmessages.StatLpReportNoAdmission(
                                    personHistory.GetPersonName(),
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
                                        personHistory.GetPersonName(),
                                        stayInfo.To.ToShortDateString()
                                    )));
                            }
                        }



                        // Dauer von Aufenthalten

                        if (checkAttributes)
                        {
                            // Alle Aufnahmetypen im Aufenthalt, der Aufenthalt startet immer mit einem Aufenthaltstyp Attribut
                            List<Model.Attribute> admissionTypeAttributes = stayInfo.Attributes.Where(c => c.AttributeType == AttributeType.AdmissionType).OrderByDescending(a => a.FromD).ToList();

                            foreach (Model.Attribute currentAdmissionTypeAttribute in admissionTypeAttributes)
                            {
                                // Das nächste Aufnahme Attribut liegt nach dem aktuellen
                                Model.Attribute nextAdmissionTypeAttribute = admissionTypeAttributes.Where(e => e.FromD > currentAdmissionTypeAttribute.FromD).FirstOrDefault();


                                DateTime startDate = currentAdmissionTypeAttribute.FromD;
                                AdmissionType admissionType = (AdmissionType)Enum.Parse(typeof(AdmissionType), currentAdmissionTypeAttribute.Value);

                                // Das Ende ist entweder der nächste Aufenthaltstyp oder der das Ende des Aufenthalts
                                DateTime endDate;
                                if (nextAdmissionTypeAttribute != null)
                                {
                                    endDate = nextAdmissionTypeAttribute.FromD;
                                }
                                else
                                {
                                    endDate = stayInfo.To;
                                }


                                TimeSpan span = endDate - startDate;

                                switch (admissionType)
                                {
                                    case AdmissionType.HolidayAt:
                                        if (span.TotalDays > 42)
                                        {
                                            ValidationFailure f = new ValidationFailure(nameof(StatLpReport.FromD),
                                                Validationmessages.StatLpReportPersonPeriodForAdmissionTooLong(personHistory.GetPersonName(),
                                                    displayNameResolver.GetDisplayName(admissionType.ToString()),
                                                    $"mehr als 42 Tage"))
                                            {
                                                Severity = Severity.Warning
                                            };

                                            ctx.AddFailure(f);
                                        }
                                        break;

                                    case AdmissionType.TransitionalAt:
                                        if (span.TotalDays > 365)
                                        {
                                            ValidationFailure f = new ValidationFailure(nameof(StatLpReport.FromD),
                                                Validationmessages.StatLpReportPersonPeriodForAdmissionTooLong(personHistory.GetPersonName(),
                                                    displayNameResolver.GetDisplayName(admissionType.ToString()),
                                                    $"mehr als 365 Tage"))
                                            {
                                                Severity = Severity.Warning
                                            };

                                            ctx.AddFailure(f);
                                        }
                                        break;
                                }

                            }
                        }




                        // Wechsel von Aufnahmearten prüfen

                        if (checkAttributes)
                        {
                            // Alle Aufnahmetypen im Aufenthalt
                            List<Model.Attribute> admissionTypeAttributes = stayInfo.Attributes.Where(c => c.AttributeType == AttributeType.AdmissionType).OrderBy(a => a.FromD).ToList();

                            foreach (Model.Attribute currentAdmissionTypeAttribute in admissionTypeAttributes)
                            {
                                // Das Aufnahme Attribut, das vor dem aktuellen lag
                                Model.Attribute lastAdmissionTypeAttribute = admissionTypeAttributes.Where(e => e.FromD < currentAdmissionTypeAttribute.FromD).FirstOrDefault();

                                if (lastAdmissionTypeAttribute != null)
                                {
                                    AdmissionType sentAdmissionType = (AdmissionType)Enum.Parse(typeof(AdmissionType), currentAdmissionTypeAttribute.Value);
                                    AdmissionType lastAdmissionType = (AdmissionType)Enum.Parse(typeof(AdmissionType), lastAdmissionTypeAttribute.Value);

                                    // Kein Wechsel von Dauer auf Urlaub / Übergang
                                    if (lastAdmissionType == AdmissionType.ContinuousAt &&
                                        (sentAdmissionType == AdmissionType.HolidayAt ||
                                         sentAdmissionType == AdmissionType.TransitionalAt))
                                    {
                                        ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAdmissionAttributeNoChangeFromLongTimeCarePossible(personHistory.GetPersonName(), displayNameResolver.GetDisplayName(currentAdmissionTypeAttribute.Value)));
                                    }
                                }
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
                if (previousReport != null)
                {
                    foreach (Stay previousStay in previousReport.Stays)
                    {
                        if (previousStay.ToD.IsLastDateInMonth())
                        {
                            // History der Person, vom vorherigem Report (vor dem gesendeten) im nächsten (dem akutellen) ermitteln
                            // todo: das machen wir alles, nur um auf die Id vom nächsten Report zu kommen
                            // evtl. eigenes Dictionary, oben?
                            PersonHistory historyFromPreviousStay = null;

                            foreach (PersonHistory personHistory in personHistoryDictionary.Values)
                            {
                                foreach (KeyValuePair<string, string> keyValuePair in personHistory.PersonIdDictionary)
                                {
                                    if (keyValuePair.Key == previousReport.SourceSystemId &&
                                        keyValuePair.Value == previousStay.PersonId)
                                    {
                                        historyFromPreviousStay = personHistory;
                                    }
                                }
                            }

                            // Id im akutellen Report
                            string idFromSentReport = historyFromPreviousStay.PersonIdDictionary[sentReport.SourceSystemId];






                            // Der Aufenthalt in der vorherigen Meldung dauert genau bis num Monatsende
                            // Es konnte also im Vormonat nicht geprüft werden, ob eine Enlasstung notwendig gewesen wäre

                            // Wir ermittlen also in der Meldung des nächsten Monats, ob ein nachfolgender Aufenthalt vorhanden ist
                            Stay firstStayInNextMonth = sentReport.Stays.Where(f => f.PersonId == idFromSentReport)
                                                                        .OrderBy(f => f.ToD)
                                                                        .FirstOrDefault();

                            // Dann brauchen wir eine evtl. Enlassung aus dem Vormonat
                            Leaving previousLeaving = previousReport.Leavings.Where(l => l.LeavingDateD == previousStay.ToD &&
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
                                        historyFromPreviousStay?.GetPersonName(),
                                        previousStay.ToD.ToShortDateString()
                                    )));
                                }
                            }



                            if (firstStayInNextMonth != null)
                            {
                                // Dann brauchen wir auch eine evtl. Neuaufnahme vom Folgemonat
                                Admission nextAdmission = sentReport.Admissions.Where(l => l.AdmissionDateD == firstStayInNextMonth.FromD &&
                                                                                       l.PersonId == idFromSentReport)
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
                                            historyFromPreviousStay?.GetPersonName(),
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
        private Dictionary<string, PersonHistory> CreatePersonHistories(StatLpReport sentReport, IEnumerable<StatLpReport> existingReports)
        {
            Dictionary<string, PersonHistory> personHistories = new Dictionary<string, PersonHistory>();

            // Liste mit Personen aus dem gesendeten und vorherigen Report
            foreach (Person sentPerson in sentReport.Persons)
            {
                var personHistory = new PersonHistory();
                personHistory.ClearingId = sentPerson.ClearingId;
                personHistory.AddPersonId(sentReport.SourceSystemId, sentPerson.Id);
                personHistory.IsFromSentReport = true;


                // Wir sammeln alle Aufnahmen, Aufnahmen und Enlassungen pro Person und sortieren nach Aufnahmedatum
                foreach (StatLpReport existingReport in existingReports.OrderBy(sl => sl.FromD))
                {
                    // Für das Personen ID Mapping zwischen den Reports holen wir uns die Person aus dem Report
                    Person existingPerson = existingReport.Persons.Where(x => x.ClearingId == sentPerson.ClearingId).FirstOrDefault();

                    AddDataToPersonHistory(existingPerson, personHistory, existingReport);
                }

                // Sortieren der Daten in der History
                SortHistory(personHistory);

                personHistories.Add(personHistory.ClearingId, personHistory);
            }


            // Für die gesendeten Personen wird die Historie sowieso geprüft
            // Für die Personen, die nur im vorherigen Report waren (und nicht im gesendeten)
            // wird der Abschluss der Vormeldung geprüft

            StatLpReport previousReport = existingReports.OrderByDescending(sl => sl.FromD).FirstOrDefault();
            if (previousReport != null)
            {
                foreach (Person existingPerson in previousReport.Persons)
                {
                    if (!personHistories.ContainsKey(existingPerson.ClearingId))
                    {
                        var personHistory = new PersonHistory();
                        personHistory.ClearingId = existingPerson.ClearingId;
                        personHistory.AddPersonId(previousReport.SourceSystemId, existingPerson.Id);
                        personHistory.IsFromSentReport = true;

                        // Wir sammeln alle Aufnahmen, Aufnahmen und Enlassungen pro Person und sortieren nach Aufnahmedatum
                        foreach (StatLpReport existingReport in existingReports.OrderBy(sl => sl.FromD))
                        {
                            AddDataToPersonHistory(existingPerson, personHistory, existingReport);
                        }

                        // Sortieren der Daten in der History
                        SortHistory(personHistory);

                        personHistories.Add(personHistory.ClearingId, personHistory);
                    }
                }
            }

            CreateMonthlyStays(personHistories.Values.ToList(), sentReport);

            return personHistories;
        }


        private void SortHistory(PersonHistory history)
        {

            // todo: kann man vermutlich schöner lösen
            PersonHistory temp = new PersonHistory();

            temp.Admissions.AddRange(history.Admissions.OrderByDescending(st => st.AdmissionDateD));
            temp.Stays.AddRange(history.Stays.OrderByDescending(st => st.FromD));
            temp.Leavings.AddRange(history.Leavings.OrderByDescending(st => st.LeavingDateD));
            temp.Attributes.AddRange(history.Attributes.OrderByDescending(st => st.FromD));

            history.Admissions.Clear();
            history.Stays.Clear();
            history.Leavings.Clear();
            history.Attributes.Clear();

            history.Admissions.AddRange(temp.Admissions);
            history.Stays.AddRange(temp.Stays);
            history.Leavings.AddRange(temp.Leavings);
            history.Attributes.AddRange(temp.Attributes);
        }


        private void AddDataToPersonHistory(Person person, PersonHistory personHistory, StatLpReport report)
        {
            // Evtl. neue Personen ID merken
            personHistory.AddPersonId(report.SourceSystemId, person.Id);
            personHistory.Person = person;

            // Alle Daten zur Person in der History speichern
            foreach (KeyValuePair<string, string> personIdValue in personHistory.PersonIdDictionary)
            {
                personHistory.Admissions.AddRange(report.Admissions.Where(ad => ad.PersonId == personIdValue.Value));
                personHistory.Stays.AddRange(report.Stays.Where(ad => ad.PersonId == personIdValue.Value));
                personHistory.Leavings.AddRange(report.Leavings.Where(ad => ad.PersonId == personIdValue.Value));
                personHistory.Attributes.AddRange(report.Attributes.Where(ad => ad.PersonId == personIdValue.Value));
            }
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

            // Das ist die ID, die im gesendeten Report verwendet wird
            string idFromSentReport = personHistory.PersonIdDictionary[sentReport.SourceSystemId];


            // Alle Aufenthalte einer Person ermitteln, die vor dem dem Start Datum lagen
            List<Stay> staysToSearch = new List<Stay>();
            staysToSearch.AddRange(sentReport?.Stays.Where(x => x.PersonId == idFromSentReport));
            staysToSearch.AddRange(personHistory.Stays);

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
            string idFromSentReport = personHistory.PersonIdDictionary[sentReport.SourceSystemId];

            stayInfo.Admissions.AddRange(sentReport.Admissions.Where(x => x.AdmissionDateD >= stayInfo.From &&
                                                                          x.AdmissionDateD <= stayInfo.To &&
                                                                          x.PersonId == idFromSentReport));

            stayInfo.Admissions.AddRange(personHistory.Admissions.Where(x => x.AdmissionDateD >= stayInfo.From &&
                                                                             x.AdmissionDateD <= stayInfo.To));
        }


        /// <summary>
        /// Alle Attribute, die in einem Stay gesendet wurden
        /// </summary>
        private void AddAttributesToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            string idFromSentReport = personHistory.PersonIdDictionary[sentReport.SourceSystemId];

            stayInfo.Attributes.AddRange(sentReport.Attributes.Where(x => x.FromD >= stayInfo.From &&
                                                                          x.FromD <= stayInfo.To &&
                                                                          x.PersonId == idFromSentReport));

            stayInfo.Attributes.AddRange(personHistory.Attributes.Where(x => x.FromD >= stayInfo.From &&
                                                                             x.FromD <= stayInfo.To));
        }


        /// <summary>
        /// Mögliche Entassungen für einen Stay ermitteln, im ganzen Bereich, nur zur Fehlerprüfung
        /// Normalfall: 1 Stay, 1 Leaving
        /// </summary>
        private void AddLeavingsToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            string idFromSentReport = personHistory.PersonIdDictionary[sentReport.SourceSystemId];

            stayInfo.Leavings.AddRange(sentReport.Leavings.Where(x => x.LeavingDateD >= stayInfo.From &&
                                                                      x.LeavingDateD <= stayInfo.To &&
                                                                      x.PersonId == idFromSentReport));

            stayInfo.Leavings.AddRange(personHistory.Leavings.Where(x => x.LeavingDateD >= stayInfo.From &&
                                                                         x.LeavingDateD <= stayInfo.To));
        }

        /// <summary>
        /// Akutelle Aufnahme für einen Stay ermitteln, genau für das Stay Start Datum
        /// </summary>
        private void AddActualAdmissionToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            string idFromSentReport = personHistory.PersonIdDictionary[sentReport.SourceSystemId];

            stayInfo.Admission = sentReport.Admissions.Where(x => x.AdmissionDateD == stayInfo.From &&
                                                                  x.PersonId == idFromSentReport)
                                                      .FirstOrDefault();

            if (stayInfo.Admission == null)
                stayInfo.Admission = personHistory.Admissions.Where(x => x.AdmissionDateD == stayInfo.From).FirstOrDefault();

        }


        /// <summary>
        /// Entlassung für einen Stay ermitteln
        /// </summary>
        private void AddActualLeavingToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            string idFromSentReport = personHistory.PersonIdDictionary[sentReport.SourceSystemId];

            stayInfo.Leaving = sentReport.Leavings.Where(x => x.LeavingDateD == stayInfo.To &&
                                                              x.PersonId == idFromSentReport)
                                                  .FirstOrDefault();

            if (stayInfo.Leaving == null)
                stayInfo.Leaving = personHistory.Leavings.Where(x => x.LeavingDateD == stayInfo.To).FirstOrDefault();

        }


    }
}