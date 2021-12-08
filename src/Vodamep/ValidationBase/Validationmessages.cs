using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;

namespace Vodamep.ValidationBase
{
    internal class Validationmessages
    {
        public static string ActivityMoreThenFive => $"Es wurden mehr als 5 gemeldet.";
        public static string ActivityMoreThen350(Person p, int x) => $"Für '{p?.FamilyName} {p?.GivenName}' wurden mehr als 350 LP in einem Monat erfasst. ({x})";
        public static string TraineeMustNotContain06To10(Staff staff) => $"'{staff?.FamilyName} {staff?.GivenName} ({staff?.Id})' darf als Auszubildende/r keine medizinischen Leistungen (6-10) dokumentieren.";
        public static string IdIsNotUnique => "Der Id ist nicht eindeutig.";
        public static string IdIsMissing(string id) => $"Der Id '{id}' fehlt.";
        public static string WithoutEntry(string e, string clientName, string staff, string date) => $"Kein Eintrag '{e}': bei '{clientName}', von '{staff}', am '{date}'.";
        public static string WithoutActivity => $"Keine Aktivitäten.";
        public static string ReportBaseWithoutActivity (string clientOrStaff, string name) => $"Keine Aktivitäten für {clientOrStaff} '{name}' dokumentiert.";
        public static string ReportBaseActivityWithoutPerson (string activityId, string personId) => $"Eine Aktivität mit der ID '{activityId}' ist keiner vorhandenen Person (ID '{personId}') zugeordnet.";
        public static string ReportBaseActivitWithoutStaff (string activityId, string staffId) => $"Eine Aktivität mit der ID '{activityId}' ist keinem vorhandenen Mitarbeiter (ID '{staffId}') zugeordnet.";
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
        public static string SameYear => "Die Meldung darf keinen Jahreswechsel beinhalten.";
        public static string LastDateInMonth => "'{PropertyName}' muss der letzte Tag des Monats sein.";
        public static string LastDateInYear => "'{PropertyName}' muss der letzte Tag des Jahres sein.";
        public static string FirstDateInMonth => "'{PropertyName}' muss der erste Tag des Monats sein.";
        public static string FirstDateInYear => "'{PropertyName}' muss der erste Tag des Jahres sein.";
        public static string InvalidCode => "Für '{PropertyName}' ist '{PropertyValue}' kein gültiger Code.";
        public static string ReportBaseInvalidCode (string client) => $"Für '{{PropertyName}}' von Klient '{client}' ist '{{PropertyValue}}' kein gültiger Code.";
        public static string InvalidPostCode_City => "'{PropertyValue}' ist kein gültiger Ort.";
        public static string ReportBaseInvalidPostCodeCity (string client) => $"'{{PropertyValue}}' ist kein gültiger Ort von Klient '{client}'.";
        public static string ReportBaseReferrerIsOtherRefererrerThenOtherReferrerMustBeSet (string client) => $"Wenn der Zuweiser von Klient '{client}' ein Anderer Zuweiser ist, dann muss Anderer Zuweiser gesetzt sein.";
        public static string DoubledDiagnosisGroups (string client) => $"Es dürfen keine doppelten Diagnosegruppen für Klient '{client}' vorhanden sein.";
        public static string OnlyONePalliativeDiagnosisGroup (string client) => $"Es darf nur eine Palliativ Diagnose Gruppe für Klient '{client}' vorhanden sein.";
        public static string AtLeastOneDiagnosisGroup (string client) => $"Es muss mindestens eine Diagnosegruppe für Klient '{client}' vorhanden sein.";
        public static string ReportBaseMinutesMustBeGreater0 (string propertyName, string clientId) => $"{propertyName} von Klient '{clientId}' muss größer 0 sein.";
        public static string ReportBaseStepWidthWrong (string propertyName, string clientName, int nrOfMinutes) => $"{propertyName} von '{clientName}' darf nur in {nrOfMinutes} Minuten Schritten eingegeben werden.";
        public static string ReportBaseMaxSumOfMinutesPerStaffMemberIs12Hours (string date, string name) => $"Die Leistungsminuten von '{name}' am '{date}' dürfen 12 Stunden nicht überschreiten.";
        public static string MaxSumOfMinutesTravelTimesIs5Hours (string staff, string date) => $"Summe Wegzeiten von Mitarbeiter {staff} am {date} darf 5 Stunden nicht überschreiten.";
        public static string OnlyOneTravelTimeEntryPerStaffMemberAndDay => "Pro Mitarbeiter ist nur ein Eintrag bei den Wegzeiten pro Tag erlaubt.";
        public static string WithinAnActivityThereAreNoDoubledActivityTypesAllowed (string personId) => $"Innerhalb einer Aktivität von Klient '{personId}' dürfen keine doppelten Leistungstypen vorhanden sein.";
        public static string WithinAnActivityTheValuesAreNotAllowedInCombination(string client, string activityType1, string activityType2) => $"Innerhalb einer Aktivität von Klient '{client}' dürfen nicht gleichzeitg die Leistungstypen '{activityType1}' und '{activityType2}' vorhanden sein.";
        public static string InvalidInstitutionNumber => "Ungültige Einrichtungsnummer.";
        public static string NoDoubledValuesAreAllowed => $"Doppelte Angaben bei '{{PropertyName}}'";
        public static string ItemNotValid => "Keine gültige Angabe bei '{PropertyName}'";
        public static string TextAreaEnterAValue => "Bei '{PropertyName}' im Textfeld bitte einen Wert angegeben.";
        public static string PersonIsNotAvailable => "Person '{PropertyValue}' ist nicht in der Personenliste vorhanden.";
        public static string FromMustBeBeforeTo => $"'Von' muss vor 'Bis' liegen.";
        public static string DeadClientNeedsDeadthLocation(string personId) => $"Wenn der Klient '{personId}' gestorben ist, muss eine Angabe zum Sterbeort gemacht werden.";
        public static string DeadClientMustNotContainDischargeLocation(string personId) => $"Wenn der Klient '{personId}' gestorben ist, darf keine Angabe zur Entlassung gemacht werden.";
        public static string LeavingOtherFilledNeedsOther(string personId) => $"Wenn bei der Entlassung von Klient '{personId}' sonstige Angaben gemacht werden, muss 'Sonstige' ausgewählt werden.";
        public static string LeavingClientNeedsLeavingLocation(string personId) => $"Wenn der Klient '{personId}' entlassen worden ist, muss angegeben werden, wohin der Klient entlassen wurde.";
        public static string LeavingClientNeedsLeavingReason(string personId) => $"Wenn der Klient '{personId}' entlassen worden ist, muss angegeben werden, warum der Klient entlassen wurde.";
        public static string LeavingClientDeathMustNotBeFilled(string personId) => $"Wenn der Klient '{personId}' entlassen worden ist, darf keine Angabe zum Sterbefall gemacht werden.";
        public static string DischargedClientNeedsDischargeLocation(string personId) => $"Wenn der Klient '{personId}' entlassen worden ist, muss angegeben werden, wohin der Klient entlassen wurde.";
        public static string InvalidValue(string date, string personId) => $"Ungültiger Wert für '{{PropertyName}}' bei Aufnahme vom '{date}' von Klient '{personId}'.";
        public static string TextTooLong(string date, string personId) => $"Zu langer Text für '{{PropertyName}}' bei Aufnahme vom '{date}' von Klient '{personId}'.";
        public static string ReportBaseIdIsNotUnique (string clientId)=> $"Die Id von Klient '{clientId}' ist nicht eindeutig.";
        public static string ReportBaseActivityDateMustBeWithinReport(string propertyName, string clientPersonName, string dateTime) => $"'{propertyName}' der Aktivität {dateTime} von '{clientPersonName}' muss innerhalb des Meldungszeitraums liegen.";
        public static string ReportBaseBirthdayNotInFuture(string clientId) => $"'Geburtsdatum' von Klient '{clientId}' darf nicht in der Zukunft liegen.";
        public static string ReportBaseBirthdayMustNotBeBefore(string clientId) => $"Der Wert von 'Geburtsdatum' von Klient '{clientId}' muss grösser oder gleich .*.";
        public static string ReportBaseValueMustNotBeEmpty(string clientId) => $"'{{PropertyName}}' von Klient '{clientId}' darf nicht leer sein.";
        public static string ReportBaseValueMustNotBeEmptyWithProperty(string property) => $"'{{PropertyName}}' von '{property}' darf nicht leer sein.";
        public static string ReportBaseValueMustNotBeEmpty(string clientOrStaff, string clientId) => $"'{{PropertyName}}' von {clientOrStaff} '{clientId}' darf nicht leer sein.";
        public static string ReportBaseValueMustNotBeEmptyWithParentProperty(string property, string parentProperty) => $"'{property}' von {parentProperty} darf nicht leer sein.";
        public static string ReportBaseValueMustNotBeEmptyWithString(string property, string clientId) => $"'{property}' von Klient '{clientId}' darf nicht leer sein.";
        public static string ReportBaseValueMustNotBeEmpty(string property, string parentProperty, string clientId) => $"'{property}' von {parentProperty} von Klient '{clientId}' darf nicht leer sein.";
        public static string ReportBaseValueMustBeGreaterThanZero(string property, string parentProperty, string clientOrStaff, string clientId) => $"Der Wert von'{property}' von {parentProperty} von {clientOrStaff} '{clientId}' muss größer 0 sein.";
        public static string ReportBasePropertyInvalidFormat(string clientOrStaff, string clientOrStaffId) => $"'{{PropertyName}}' von {clientOrStaff} '{clientOrStaffId}' weist ein ungültiges Format auf.";
        public static string ReportBaseInvalidLength(string id) => $"'{{PropertyName}}' von Klient '{id}' besitzt eine ungültige Länge'";
        public static string ReportBaseInvalidValue(string id) => $"'{{PropertyName}}' von Klient '{id}' hat einen ungültigen Wert'";
        public static string ReportBaseDateMustNotHaveTime(string id) => $"'{{PropertyName}}' von Klient '{id}' darf keine Uhrzeit beinhalten.";
        public static string ReportBaseActivityWrongValue(string date, string op) => $"Bei der Leistung am {date} wurde ein Wert {op} angegeben.";
        public static string ReportBaseDateMustNotHaveTime(string propertyName, string id) => $"'{propertyName}' von Klient '{id}' darf keine Uhrzeit beinhalten.";
        public static string ReportBasePersonActivityWrongValue(string personId, string op) => $"Bei der Leistung von Person '{personId}' wurde ein Wert {op} angegeben.";
        public static string ReportBasePersonActivityWrongValue(string personId, string date, string op) => $"Bei der Leistung von Person '{personId}' am {date} wurde ein Wert {op} angegeben.";
        public static string ReportBaseActivityWrongStepLength(string personId, string stepLength) => $"Bei der Leistung von Person '{personId}' muss der Wert in {stepLength} Schritten angegeben werden.";
        public static string ReportBaseActivityNoCategory(string date) => $"Bei der Leistung am {date} wurde keine Kategorie angegeben.";
        public static string ReportBasePersonActivityNoCategory(string personId, string date) => $"Bei der Leistung von Person '{personId}' am {date} wurde keine Kategorie angegeben.";
        public static string ReportBaseActivityWrongDate(string date) => $"Bei der Leistung am {date} wurde ein Datum außerhalb des Meldungszeitraums angegeben.";
        public static string ReportBasePersonActivityWrongDate(string personId, string date) => $"Bei der Leistung von Person '{personId}' am {date} wurde ein Datum außerhalb des Meldungszeitraums angegeben.";
        public static string ReportBaseClientActivityUnknownPerson(string date) => $"Unbekannter Klient bei Leistung am {date}.";
        public static string ReportBaseActivityMultipleActivitiesForOnePerson(string personId) => $"Mehrfache Leistungen für Klient '{personId}' vorhanden.";
        public static string MohiActivityContainsNonExistingPerson(string personId) => $"Für Klient '{personId}' wurden keine Personendaten gesendet.";
        public static string ReportBaseItemMustBeInReportPeriod (string clientName) => $"{{PropertyName}} von Person '{clientName}' muss im Meldezeitraum liegen.";

        public static string StatLpAdmissionInvalidValueAdmission(string date, string clientName) => $"Ungültiger Wert für '{{PropertyName}}' bei Aufnahme vom {date} von Klient {clientName}.";
        public static string StatLpAdmissionTextTooLong(string date, string clientName) => $"Zu langer Text für '{{PropertyName}}' bei Aufnahme vom {date} von Klient {clientName}.";
        public static string StatLpAdmissionEmptyPostCode(string date, string clientName) => $"Keine Angabe von Ort/Plz bei Aufnahme vom {date} von Klient {clientName}.";
        public static string StatLpAdmissionWrongPostCode(string date, string clientName) => $"Ungültige Kombination Ort/Plz bei Aufnahme vom {date} von Klient {clientName}.";

        public static string StatLpOriginAdmissionDateMustBeLessThanAdmissionDate(string clientName, string from) => $"Das ursprüngliche Aufnahmedatum von Person '{clientName}' muss kleinergleich dem Aufnahmedatum ({from}) sein.";
        public static string StatLpAdmissionDifferentToValid(string clientName, DateTime from) => $"Das ursprüngliche Aufnahmedatum von Person '{clientName}' unterscheidet sich vom Aufnahmedatum ({from.ToShortDateString()}).";
        public static string StatLpAdmissionAttributeMissing(string clientName, string date, string attributeType) => $"Vor der Aufnahme von Klient '{clientName}' am {date} wurde keine '{attributeType}' gesendet.";
        public static string StatLpAdmissionMultipleAttribute(string clientName, string date, string attributeType) => $"Vor der Aufnahme von Klient '{clientName}' am {date} wurde '{attributeType}' mehrfach gesendet.";
        
        public static string StatLpStayEveryPersonMustBeInAStay(string clientName) => $"Die Person '{clientName}' wird in keinem Aufenthalt erwähnt.";
        public static string StatLpStayStartsAfterReportEnd(string clientName, string fromDate) => $"Der Aufenthalt am '{fromDate}' von '{clientName}' darf nicht nach dem Meldunszeitraum beginnen.";

        public static string StatLpStayAheadOfPeriod(string clientName, string fromDate, string toDate) => $"Der Aufenthalt vom '{fromDate}' bis zum '{toDate}' von '{clientName}' liegt vor dem Meldunszeitraum.";

        public static string StatLpStayAfterPeriod(string clientName, string fromDate, string toDate) => $"Der Aufenthalt vom '{fromDate}' bis zum '{toDate}' von '{clientName}' liegt nach dem Meldunszeitraum.";

        public static string StatLpStayToLong(string type, string clientName, string fromDate, string toDate, int maxDays) => $"Der Aufenthalt '{type}' vom '{fromDate}' bis zum '{toDate}' von '{clientName}' darf nicht länger als {maxDays} Tage dauern.";

        public static string StatLpInvalidAdmissionTyeChange(string fromType, string toType, string clientName, string fromDate, string toDate) => $"Nach dem Aufenthalt '{fromType}' vom '{fromDate}' bis zum '{toDate}' von '{clientName}' darf kein '{toType}' folgen.";

        public static string StatLpStayMustnotOverlap(string clientName) => $"Die Aufenthalte von '{clientName}' dürfen sich nicht überschneiden.";
        public static string StatLpLeavingReasonMustnotBeEmpty (string clientName) => $"Beim Abgang von Klient '{clientName}' muss eine Abgang Art angegeben werden.";
        public static string StatLpAttributeInvalidAdmissionType(string clientName, string admissionType, string date) => $"Die Aufnahmeart {admissionType} bei der Aufnahme vom {date} von Klient '{clientName}' ist nicht mehr erlaubt.";
        public static string StatLpAttributeMultipleChanged(string clientName, string date, string attributeType) => $"Die Eigenschaft {attributeType} von Klient {clientName} am {date} wurde mehrfach am gleichen Tag geändert.";
        public static string StatLpAttributeWrongValue(string attributeName, string value) => $"Der Wert des Attributs mit dem Typen '{attributeName}' kann nicht auf den Wert '{value}' gesetzt werden.";


        public static string StatLpHistoryMissingReports (string date1, string date2) => $"Die Meldungen für den Zeitraum {date1} bis {date2} wurden noch nicht übermittelt.";
        public static string StatLpHistoryAttributeAlreadySent (string attributeType, string clientName, string attributeValue, string dateCurrent, string dateLast) => $"Die Änderung von '{attributeType}' von Klient '{clientName}' am '{dateCurrent}' auf '{attributeValue}' wurde bereits mit der Meldung am '{dateLast}' gesendet.";
        public static string StatLpHistoryNoChangeFromLongTimeCarePossible (string clientName,  string newAdmissionTypeValue) => $"Bei Klient '{clientName}' ist kein Wechsel von einer Daueraufname auf '{newAdmissionTypeValue}' möglich.";
        public static string StatLpHistoryPeriodForAdmissionTooLong (string clientName, string admissionType, string maxPersiod) => $"Bei Klient '{clientName}' wurde der Zeitraum für die Aufnahmeart '{admissionType}' überschritten ({maxPersiod}).";
        public static string StatLpHistoryPersonChanged (string propertyName, string clientName, string date) => $"Unterschied bei '{propertyName}' von Klient '{clientName}' bei Meldung vom {date}.";
        public static string StatLpHistoryMultipleAdmissions (string clientName, string datefrom, string dateto) => $"Für den Aufenthalt von Klient '{clientName}' vom '{datefrom}' bis '{dateto}' wurden mehrere Aufnahmen gesendet.";
        public static string StatLpHistoryNoAdmission(string clientName, string date) => $"Vor dem Aufenthalt von Klient '{clientName}' am '{date}' wurden keine Aufnahmedaten gesendet.";
        public static string StatLpHistoryNoLeaving (string clientName, string date) => $"Zum Aufenthaltsende von Klient '{clientName}' am '{date}' wurden keine Entlassungsdaten gesendet.";


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
