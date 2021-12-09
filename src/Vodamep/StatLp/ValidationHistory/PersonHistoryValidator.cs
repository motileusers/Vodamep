using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryValidator : AbstractValidator<StatLpReportHistory>
    {
        private DisplayNameResolver displayNameResolver = new DisplayNameResolver();



        /// <summary>
        /// Prüfung der Reihenfolge und Datumsabhängikeiten von Aufnahmen, Entlassungen, Attributen, ...
        /// </summary>
        public PersonHistoryValidator()
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
                            Validationmessages.StatLpHistoryMissingReports(
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

                    // Durchgängiges Geburtsdatum prüfen
                    DateTime birthday = personHistory.Person.BirthdayD;
                    foreach (PersonInfo personInfo in personHistory.PersonInfos)
                    {
                        if (personInfo.Person.BirthdayD != birthday)
                        {
                            ctx.AddFailure(nameof(Person.BirthdayD),
                                Validationmessages.StatLpHistoryPersonChanged(
                                    displayNameResolver.GetDisplayName(nameof(Person.BirthdayD)), personHistory.GetPersonName(),
                                    sentReport.FromD.ToShortDateString()));
                        }
                    }


                    // Durchgängiges Geschlecht prüfen
                    // Admission im gesendeten Report suchen
                    Admission lastAdmissionFromCurrentReport = sentReport.Admissions.Where(a => a.AdmissionDateD >= sentReport.FromD &&
                                                                                                a.PersonId == personHistory.Person.Id)
                                                                                    .OrderByDescending(a => a.AdmissionDateD)
                                                                                    .FirstOrDefault();
                    if (lastAdmissionFromCurrentReport != null)
                    {
                        Gender gender = lastAdmissionFromCurrentReport.Gender;
                        foreach (Admission admission in personHistory.Admissions.OrderByDescending(a => a.AdmissionDateD))
                        {
                            if (admission.Gender != gender)
                            {
                                ctx.AddFailure(nameof(Admission.Gender),
                                    Validationmessages.StatLpHistoryPersonChanged(
                                        displayNameResolver.GetDisplayName(nameof(Admission.Gender)), personHistory.GetPersonName(),
                                        sentReport.FromD.ToShortDateString())); ;
                            }
                        }
                    }



                    foreach (StayInfo stayInfo in personHistory.StayInfos)
                    {
                        // Die Attribute werden nicht geprüft, wenn es gröbere Probleme im 
                        // Meldungsaufbau gibt. Da gibt's dann nur zu viele verwirrende Fehlermeldungen
                        bool checkAttributes = true;

                        // Mehrfache Aufnahmen
                        if (stayInfo.Admissions.Count > 1)
                        {
                            ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                    Validationmessages.StatLpHistoryMultipleAdmissions(
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
                                Validationmessages.StatLpHistoryNoAdmission(
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
                                    Validationmessages.StatLpHistoryNoLeaving(
                                        personHistory.GetPersonName(),
                                        stayInfo.To.ToShortDateString()
                                    )));
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
                personHistory.Person = sentPerson;


                // Wir sammeln alle Aufnahmen, Aufnahmen und Enlassungen pro Person und sortieren nach Aufnahmedatum
                foreach (StatLpReport existingReport in existingReports.OrderBy(sl => sl.FromD))
                {
                    // Für das Personen ID Mapping zwischen den Reports holen wir uns die Person aus dem Report
                    Person existingPerson = existingReport.Persons.Where(x => x.ClearingId == sentPerson.ClearingId).FirstOrDefault();

                    if (existingPerson != null)
                    {
                        AddDataToPersonHistory(existingPerson, personHistory, existingReport);
                    }
                }

                // Sortieren der Daten in der History
                SortHistory(personHistory);

                personHistories.Add(personHistory.ClearingId, personHistory);
            }


            // Für die gesendeten Personen wird die Historie sowieso geprüft
            // Für die Personen, die nur im vorherigen Report waren (und nicht im gesendeten)
            // wird der Abschluss der Vormeldung geprüft
            // Die History wird gekennzeichnet mit: IsFromSentReport = false

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
                        personHistory.IsFromSentReport = false;
                        personHistory.Person = existingPerson;

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

            // Alle Daten zur Person in der History speichern
            foreach (KeyValuePair<string, string> personIdValue in personHistory.PersonIdDictionary)
            {
                personHistory.Admissions.AddRange(report.Admissions.Where(x => x.PersonId == personIdValue.Value));
                personHistory.Stays.AddRange(report.Stays.Where(x => x.PersonId == personIdValue.Value));
                personHistory.Leavings.AddRange(report.Leavings.Where(x => x.PersonId == personIdValue.Value));
                personHistory.Attributes.AddRange(report.Attributes.Where(x => x.PersonId == personIdValue.Value));

                Person personFromReport = report.Persons.Where(x => x.Id == personIdValue.Value).FirstOrDefault();
                if (personFromReport != null)
                {
                    personHistory.PersonInfos.Add(new PersonInfo() { Person = personFromReport, Report = report });
                }
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

            // Alle Aufenthalte einer Person ermitteln, die vor dem dem Start Datum lagen
            List<Stay> staysToSearch = new List<Stay>();

            // Aufenthalte des aktuellen Monats hinzufügen
            if (personHistory.ContainsSourceSystemPersonId(sentReport.SourceSystemId))
            {
                // Das ist die ID, die im gesendeten Report verwendet wird
                string idFromSentReport = personHistory.GetSourceSystemPersonId(sentReport.SourceSystemId);
                staysToSearch.AddRange(sentReport?.Stays.Where(x => x.PersonId == idFromSentReport));
            }

            // Aufenthalte der Vormonate hinzufügen
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
                        //To = tempStays[i].ToD,
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
            // Aufnahmen des aktuellen Monats hinzufügen
            if (personHistory.ContainsSourceSystemPersonId(sentReport.SourceSystemId))
            {
                string idFromSentReport = personHistory.GetSourceSystemPersonId(sentReport.SourceSystemId);

                stayInfo.Admissions.AddRange(sentReport.Admissions.Where(x => x.AdmissionDateD >= stayInfo.From &&
                                                                              x.AdmissionDateD <= stayInfo.To &&
                                                                              x.PersonId == idFromSentReport));

            }

            // Aufnahmen der Vormonate hinzufügen
            stayInfo.Admissions.AddRange(personHistory.Admissions.Where(x => x.AdmissionDateD >= stayInfo.From &&
                                                                             x.AdmissionDateD <= stayInfo.To));
        }


        /// <summary>
        /// Alle Attribute, die in einem Stay gesendet wurden
        /// </summary>
        private void AddAttributesToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            // Attribute des aktuellen Monats hinzufügen
            if (personHistory.ContainsSourceSystemPersonId(sentReport.SourceSystemId))
            {
                string idFromSentReport = personHistory.GetSourceSystemPersonId(sentReport.SourceSystemId);

                stayInfo.Attributes.AddRange(sentReport.Attributes.Where(x => x.FromD >= stayInfo.From &&
                                                                              x.FromD <= stayInfo.To &&
                                                                              x.PersonId == idFromSentReport));

            }

            // Attribute der Vormonate hinzufügen
            stayInfo.Attributes.AddRange(personHistory.Attributes.Where(x => x.FromD >= stayInfo.From &&
                                                                             x.FromD <= stayInfo.To));
        }


        /// <summary>
        /// Mögliche Entlassungen für einen Stay ermitteln, im ganzen Bereich, nur zur Fehlerprüfung
        /// Normalfall: 1 Stay, 1 Leaving
        /// </summary>
        private void AddLeavingsToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            // Entlassungen des aktuellen Monats hinzufügen
            if (personHistory.ContainsSourceSystemPersonId(sentReport.SourceSystemId))
            {
                string idFromSentReport = personHistory.GetSourceSystemPersonId(sentReport.SourceSystemId);

                stayInfo.Leavings.AddRange(sentReport.Leavings.Where(x => x.LeavingDateD >= stayInfo.From &&
                                                                          x.LeavingDateD <= stayInfo.To &&
                                                                          x.PersonId == idFromSentReport));

            }

            // Entlassungen der Vormonate hinzufügen
            stayInfo.Leavings.AddRange(personHistory.Leavings.Where(x => x.LeavingDateD >= stayInfo.From &&
                                                                         x.LeavingDateD <= stayInfo.To));
        }

        /// <summary>
        /// Akutelle Aufnahme für einen Stay ermitteln, genau für das Stay Start Datum
        /// </summary>
        private void AddActualAdmissionToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            // Aufnahme des aktuellen Monats hinzufügen
            if (personHistory.ContainsSourceSystemPersonId(sentReport.SourceSystemId))
            {
                string idFromSentReport = personHistory.GetSourceSystemPersonId(sentReport.SourceSystemId);

                stayInfo.Admission = sentReport.Admissions.Where(x => x.AdmissionDateD == stayInfo.From &&
                                                                      x.PersonId == idFromSentReport)
                                                          .FirstOrDefault();
            }

            // Aufnahme der Vormonate hinzufügen
            if (stayInfo.Admission == null)
                stayInfo.Admission = personHistory.Admissions.Where(x => x.AdmissionDateD == stayInfo.From).FirstOrDefault();
        }


        /// <summary>
        /// Entlassung für einen Stay ermitteln
        /// </summary>
        private void AddActualLeavingToStayInfo(StayInfo stayInfo, PersonHistory personHistory, StatLpReport sentReport)
        {
            // Entlassung des aktuellen Monats hinzufügen
            if (personHistory.ContainsSourceSystemPersonId(sentReport.SourceSystemId))
            {
                string idFromSentReport = personHistory.GetSourceSystemPersonId(sentReport.SourceSystemId);

                stayInfo.Leaving = sentReport.Leavings.Where(x => x.LeavingDateD == stayInfo.To &&
                                                                  x.PersonId == idFromSentReport)
                                                      .FirstOrDefault();

            }

            // Entlassung der Vormonate hinzufügen
            if (stayInfo.Leaving == null)
                stayInfo.Leaving = personHistory.Leavings.Where(x => x.LeavingDateD == stayInfo.To).FirstOrDefault();
        }


    }
}