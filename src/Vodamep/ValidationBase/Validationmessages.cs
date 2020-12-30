using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;

namespace Vodamep.ValidationBase
{
    internal class Validationmessages
    {
        public static string ActivityIsNotUnique => $"Die Einträge sind nicht kumuliert.";
        public static string ActivityMoreThenFive => $"Es wurden mehr als 5 gemeldet.";
        public static string ActivityMoreThen350(Person p, int x) => $"Für '{p?.FamilyName} {p?.GivenName}' wurden mehr als 350 LP in einem Monat erfasst. ({x})";
        public static string TraineeMustNotContain06To10(Staff staff) => $"'{staff?.FamilyName} {staff?.GivenName} ({staff?.Id})' darf als Auszubildende/r keine medizinischen Leistungen (6-10) dokumentieren.";
        public static string IdIsNotUnique => "Der Id ist nicht eindeutig.";
        public static string IdIsMissing(string id) => $"Der Id '{id}' fehlt.";
        public static string PersonWithoutPersonalDate => "Kein Personenbezug vorhanden";
        public static string PersonWithoutData => "Keine Stammdaten vorhanden";
        public static string WithoutEntry(string e, string person, string staff, string date) => $"Kein Eintrag '{e}': bei '{person}', von '{staff}', am '{date}'.";
        public static string WithoutActivity => $"Keine Aktivitäten.";
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
        public static string FirstDateInMOnth => "'{PropertyName}' muss der erste Tag des Monats sein.";
        public static string InvalidCode => "Für '{PropertyName}' ist '{PropertyValue}' kein gültiger Code.";
        public static string InvalidPostCode_City => "'{PropertyValue}' ist kein gültiger Ort.";
        public static string ReferrerIsOtherRefererreThenOtherReferrerMustBeSet => "Wenn der Zuweiser ein Anderer Zuweiser ist, dann muss Anderer Zuweiser gesetzt sein.";
        public static string DoubledDiagnosisGroups => "Es dürfen keine doppelten Diagnosegruppen vorhanden sein.";
        public static string AtLeastOneDiagnosisGroup => "Es muss mindestens eine Diagnosegruppe vorhanden sein.";
        public static string MinutesHasToBeEnteredInFiveMinuteSteps => "Minutes dürfen nur in 5 Minuten Schritten eingegeben werden.";
        public static string MaxSumOfMinutesPerStaffMemberIs10Hours => "Summe Leistungsminuten pro Tag / pro Mitarbeiter darf 10 Stunden nicht überschreiten.";
        public static string MaxSumOfMinutesTravelTimesIs10Hours => "Summe Reisezeiten darf 5 Stunden nicht überschreiten.";


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
