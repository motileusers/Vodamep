using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;
using Person = Vodamep.Hkpv.Model.Person;

namespace Vodamep.ValidationBase
{
    internal class Validationmessages
    {
        public static string ActivityMoreThenFive => $"Es wurden mehr als 5 gemeldet.";
        public static string ActivityMoreThen350(Person p, int x) => $"Für '{p?.FamilyName} {p?.GivenName}' wurden mehr als 350 LP in einem Monat erfasst. ({x})";
        public static string TraineeMustNotContain06To10(Staff staff) => $"'{staff?.FamilyName} {staff?.GivenName} ({staff?.Id})' darf als Auszubildende/r keine medizinischen Leistungen (6-10) dokumentieren.";
        public static string IdIsNotUnique => "Der Id ist nicht eindeutig.";
        public static string IdIsMissing(string id) => $"Der Id '{id}' fehlt.";
        public static string WithoutEntry(string e, string person, string staff, string date) => $"Kein Eintrag '{e}': bei '{person}', von '{staff}', am '{date}'.";
        public static string WithoutActivity => $"Keine Aktivitäten.";
        public static string WithoutPerson => $"Keine Person.";
        public static string WithoutStaff => $"Kein Mitarbeiter.";
        public static string StaffWithoutEmployment => $"Beim Mitarbeiter ist keine Beschäftigung vorhanden";
        public static string EmploymentHoursPerWeekMustBeBetween0And100 => $"Die Stundenanzahl muss größer 0 und kleiner 100 sein";
        public static string InvalidEmploymentFromToReportRange(Staff data) => $"Das Anstellungsverhältniss von {data.GivenName} {data.FamilyName} muss innerhalb des Meldungszeitraums liegen.";
        public static string EmploymentOverlap(Staff data) => $"Die Anstellungsverhälnisse bei {data.GivenName} {data.FamilyName} überschneiden sich.";
        public static string InvalidEmploymentForActivity(Staff staff, int count, DateTime minDate, DateTime maxDate) => $"Für Mitarbeiter {staff.GivenName} {staff.FamilyName} gibt es kein Anstellungsverhältnis für {GetActivityPlural(count)} {GetRange(minDate, maxDate)}.";
        public static string BirthdayNotInFuture => "'Geburtsdatum' darf nicht in der Zukunft liegen.";
        public static string BirthdayNotInSsn(Person data) => $"Das Geburtsdatum {data?.BirthdayD.ToString("dd.MM.yyyy")} unterscheidet sich vom Wert in der Versicherungsnummer {SSNHelper.Format(data?.Ssn, true).Substring(5)}.";
        public static string SsnNotValid => "Die Versicherungsnummer {PropertyValue} ist nicht korrekt.";
        public static string SsnNotUnique(IEnumerable<Person> p) => $"Mehrere Personen haben die selbe Versicherungsnummer {p?.FirstOrDefault()?.Ssn}: {string.Join(", ", p?.Select(x => $"{x.GivenName} {x.FamilyName}({x.Id})"))}";
        public static string DateMustnotHaveTime => "'{PropertyName}' darf keine Uhrzeit beinhalten.";
        public static string OneMonth => "Die Meldung muss genau einen Monat beinhalten.";
        public static string LastDateInMonth => "'{PropertyName}' muss der letzte Tag des Monats sein.";
        public static string FirstDateInMonth => "'{PropertyName}' muss der erste Tag des Monats sein.";
        public static string InvalidCode => "Für '{PropertyName}' ist '{PropertyValue}' kein gültiger Code.";
        public static string InvalidPostCode_City => "'{PropertyValue}' ist kein gültiger Ort.";
        public static string ReferrerIsOtherRefererreThenOtherReferrerMustBeSet => "Wenn der Zuweiser ein Anderer Zuweiser ist, dann muss Anderer Zuweiser gesetzt sein.";
        public static string DoubledDiagnosisGroups => "Es dürfen keine doppelten Diagnosegruppen vorhanden sein.";
        public static string OnlyONePalliativeDiagnosisGroup => "Es darf nur eine Palliativ Diagnose Gruppe vorhanden sein.";
        public static string AtLeastOneDiagnosisGroup => "Es muss mindestens eine Diagnosegruppe vorhanden sein.";
        public static string MinutesHasToBeEnteredInFiveMinuteSteps => "Minutes dürfen nur in 5 Minuten Schritten eingegeben werden.";
        public static string MaxSumOfMinutesPerStaffMemberIs10Hours => "Summe Leistungsminuten pro Tag / pro Mitarbeiter darf 10 Stunden nicht überschreiten.";
        public static string MaxSumOfMinutesTravelTimesIs10Hours => "Summe Reisezeiten darf 5 Stunden nicht überschreiten.";
        public static string OnlyOneTravelTimeEntryPerStaffMemberAndDay => "Pro Mitarbeiter ist nur ein Eintrag bei den Reisezeiten pro Tag erlaubt.";
        public static string WithinAnActivityThereAreNoDoubledActivityTypesAllowed => "Innerhalb einer Aktivität dürfen keine doppelten Leistungstypen vorhanden sein.";
        public static string WithinAnActivityThereIsNotAccompanyingWithContactAndAccompanyingWithoutContactsAllowed => "Innerhalb einer Aktivität dürfen nicht gleichzeitg die Leistungstypen 'AccompanyingWithContact' und 'AccompanyingWithoutContacts' vorhanden sein.";
        public static string InvalidInstitutionNumber => "Ungültige Einrichtungsnummer.";
        public static string InvalidValueAdmission(string date, string personId) => $"Ungültiger Wert für '{{PropertyName}}' bei Aufnahme vom {date} von Klient {personId}.";
        public static string TextTooLongAdmission(string date, string personId) => $"Zu langer Text für '{{PropertyName}}' bei Aufnahme vom {date} von Klient {personId}.";
        public static string WrongPostCodeAdmission(string date, string personId) => $"Ungültige Kombination Ort/Plz bei Aufnahme vom {date} von Klient {personId}.";
        public static string NoDoubledValuesAreAllowed => "Doppelte Angaben bei '{PropertyName}'";
        public static string ItemNotValid => "Keine gültige Angabe bei '{PropertyName}'";
        public static string TextAreaEnterAValue => "Bei '{PropertyName}' im Textfeld bitte einen Wert angegeben.";
        public static string PersonIsNotAvailable => "Person '{PropertyValue}' ist nicht in der Personenliste vorhanden.";
        public static string FromMustBeBeforeTo => $"'Von' muss vor 'Bis' liegen.";
        public static string DeadClientNeedsDeadthLocation(string personId) => $"Wenn der Klient '{personId}' gestorben ist, muss eine Angabe zum Sterbeort gemacht werden.";
        public static string DeadClientMustNotContainDischargeLocation(string personId) => $"Wenn der Klient '{personId}' gestorben ist, darf keine Angabe zur Entlassung gemacht werden.";
        public static string LeavingOtherFilledNeedsOther(string personId) => $"Wenn bei der Entlassung von Klient '{personId}' sonstige Angaben gemacht werden, muss 'Sonstige' ausgewählt werden.";
        public static string LeavingClientNeedsLeavingLocation(string personId) => $"Wenn der Klient '{personId}' entlassen worden ist, muss angegeben werden, wohin der Klient entlassen wurde.";
        public static string LeavingClientDeathMustNotBeFilled(string personId) => $"Wenn der Klient '{personId}' entlassen worden ist, darf keine Angabe zum Sterbefall gemacht werden.";
        public static string DischargedClientNeedsDischargeLocation(string personId) => $"Wenn der Klient '{personId}' entlassen worden ist, muss angegeben werden, wohin der Klient entlassen wurde.";
        public static string InvalidValue(string date, string personId) => $"Ungültiger Wert für '{{PropertyName}}' bei Aufnahme vom '{date}' von Klient '{personId}'.";
        public static string TextTooLong(string date, string personId) => $"Zu langer Text für '{{PropertyName}}' bei Aufnahme vom '{date}' von Klient '{personId}'.";
        public static string ReportBaseIdIsNotUnique (string clientId)=> $"Die Id von Klient '{clientId}' ist nicht eindeutig.";
        public static string ReportBaseBirthdayNotInFuture(string clientId) => $"'Geburtsdatum' von Klient '{clientId}' darf nicht in der Zukunft liegen.";
        public static string ReportBaseBirthdayMustNotBeBefore(string clientId) => $"Der Wert von 'Geburtsdatum' von Klient '{clientId}' muss grösser oder gleich .*.";
        public static string ReportBaseValueMustNotBeEmpty(string clientId) => $"'{{PropertyName}}' von Klient '{clientId}' darf nicht leer sein.";
        public static string ReportBasePropertyInvalidFormat(string clientId) => $"'{{PropertyName}}' von Klient '{clientId}' weist ein ungültiges Format auf.";
        public static string ReportBaseInvalidLength(string id) => $"'{{PropertyName}}' von Klient '{id}' besitzt eine ungültige Länge'";
        public static string ReportBaseInvalidValue(string id) => $"'{{PropertyName}}' von Klient '{id}' hat einen ungülitgen Wert'";
        public static string ReportBaseDateMustNotHaveTime(string id) => $"'{{PropertyName}}' von Klient '{id}' darf keine Uhrzeit beinhalten.";
        public static string ReportBaseActivityWrongValue(string personId, string op) => $"Bei der Leistung von Person '{personId}' wurde ein Wert {op} angegeben.";
        public static string ReportBaseActivityWrongValue(string personId, string date, string op) => $"Bei der Leistung von Person '{personId}' am {date} wurde ein Wert {op} angegeben.";
        public static string ReportBaseActivityWrongStepLength(string personId, string stepLength) => $"Bei der Leistung von Person '{personId}' muss der Wert in {stepLength} Schritten angegeben werden.";
        public static string ReportBaseActivityNoCategory(string personId, string date) => $"Bei der Leistung von Person '{personId}' am {date} wurde keine Kategorie angegeben.";
        public static string ReportBaseActivityWrongDate(string personId, string date) => $"Bei der Leistung von Person '{personId}' am {date} wurde ein Datum außerhalb des Meldungszeitraums angegeben.";
        public static string ReportBaseClientActivityUnknownPerson(string date) => $"Unbekannter Klient bei Leistung am {date}.";
        public static string ReportBaseActivtyMultipleActivitiesForOnePerson(string personId) => $"Mehrfache Leistungen für Klient '{personId}' vorhanden.";
        public static string ReportBaseActivtyContainsNonExistingPerson(string personId) => $"Für Klient '{personId}' wurden keine Personendaten gesendet.";
        public static string StatLpReportEveryPersonMustBeInAStay (string personId) => $"Die Person '{personId}' wird in keinem Aufenthalt erwähnt.";

        public static string GetRange(DateTime minDate, DateTime maxDate)
        {
            if (minDate == maxDate)
            {
                return $"am { minDate.ToShortDateString()}";
            }
            else
            {
                return $"zwischen { minDate.ToShortDateString()} und { maxDate.ToShortDateString()}";
            }
        }


        public static string GetActivityPlural(int count)
        {
            if (count > 1)
            {
                return $"{count} Leistungen";
            }
            else
            {
                return $"{count} Leistung";
            }
        }

    }
}
