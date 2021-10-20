#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Personen Geschichte einer Datenmeldung



Szenario: ID Clearing - Unterschiedliche IDs
	Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020 und ist eine Standard Meldung und enthält eine 'Admission' von Person 1 vom 20.12.2020
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '20.12.2020'
	
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'

	Angenommen Gesendete Meldung 2: die Eigenschaft 'source_system_id' von 'StatLpReport' ist auf 'System2' gesetzt
	Angenommen Gesendete Meldung 2: die Id von Person 1 ist auf 99 gesetzt
	Angenommen Gesendete Meldung 1: die Eigenschaft 'person_id' von 'Stay' ist auf '99' gesetzt

	Dann enthält das History Validierungsergebnis keine Fehler


Szenario: ID Clearing Fehler - Unterschiedliche Ids mit Namen + Geburtsdatum
	Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020 und ist eine Standard Meldung und enthält eine 'Admission' von Person 1 vom 20.12.2020
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '20.12.2020'
	
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'

	Angenommen Gesendete Meldung 2: die Eigenschaft 'given_name' von 'Person' ist auf 'Peterle' gesetzt
	Angenommen Gesendete Meldung 2: die Eigenschaft 'source_system_id' von 'StatLpReport' ist auf 'System2' gesetzt
	Angenommen Gesendete Meldung 2: die Id von Person 1 ist auf 99 gesetzt
	Angenommen Gesendete Meldung 1: die Eigenschaft 'person_id' von 'Stay' ist auf '99' gesetzt

	Dann enthält das History Validierungsergebnis den Fehler 'Vor dem Aufenthalt von Klient'
	Und enthält das History Validierungsergebnis den Fehler 'Zum Aufenthaltsende von Klient'



Szenario: ID Clearing - Unterschiedliche Ids mit Namen + Geburtsdatum
	Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020 und ist eine Standard Meldung und enthält eine 'Admission' von Person 1 vom 20.12.2020
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '20.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'

	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2: die Eigenschaft 'given_name' von 'Person' ist auf 'Peterle' gesetzt
	Angenommen Gesendete Meldung 2: die Eigenschaft 'source_system_id' von 'StatLpReport' ist auf 'System2' gesetzt
	Angenommen Gesendete Meldung 2: die Eigenschaft 'id' von 'Person' ist auf '99' gesetzt
	Angenommen Gesendete Meldung 2: die Id von Person 1 ist auf 99 gesetzt

	Angenommen Die History enthält ein Mapping mit Clearing-Id 'gruber.peterle.01011920' auf Clearing-Id 'gruber.peter.01011920'

	Dann enthält das History Validierungsergebnis keine Fehler






Szenario: Performance bei großem Datenvolumen
    Angenommen Im Meldungsbereich für eine Einrichtung befinden sich monatliche Meldungen vom '01.01.2007' bis '31.12.2020'
    Angenommen Im Meldungsbereich für eine Einrichtung befinden sich Meldungen von 100 Klienten
    Angenommen Im Meldungsbereich für eine Einrichtung wird jeder Klient 5 x aufgenommen und entlassen
	Angenommen Im Meldungsbereich für eine Einrichtung wird jeder Klient 5 x geändert
	Angenommen Im Meldungsbereich für eine Einrichtung wird jeder Klient 5 x geändert
    Dann dauert die Validierung aller Meldungen nicht länger als 5 Sekunden

# Korrekte Meldungsreihenfolgen
Szenario: Gesamter Aufenthalt korrekt in einer Meldung
	Angenommen Gesendete Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Gesendete Meldung 1 enthält Standard Attribute von Person 1 vom '01.12.2020'
	Angenommen Gesendete Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 1 enthält eine 'Leaving' von Person 1 vom '31.12.2020'
	Dann enthält das History Validierungsergebnis keine Fehler

Szenario: Gesamter Aufenthalt korrekt in 3 Meldungen
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Existierende Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 3 gilt vom '01.02.2021' bis '28.02.2021'
	Angenommen Gesendete Meldung 3 enthält einen Aufenthalt von Person 1 vom '01.02.2021' bis '28.02.2021'
	Angenommen Gesendete Meldung 3 enthält eine 'Leaving' von Person 1 vom '28.02.2021'
	Dann enthält das History Validierungsergebnis keine Fehler

# Fehlende Meldungen
Szenario: Fehlende Monatsmeldungen mit Leermeldungen
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.03.2021' bis '31.03.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Die Meldungen für den Zeitraum 01.01.2021 bis 28.02.2021 wurden noch nicht übermittelt.'
    Und enthält das History Validierungsergebnis genau einen Fehler

# Nachsendungen
Szenario: Nachgesendete Monatsmeldungen zwischen zwei Monaten mit Leermeldungen
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1: die Liste 'Person' ist leer
	Angenommen Existierende Meldung 2 gilt vom '01.03.2021' bis '31.03.2021'
	Angenommen Existierende Meldung 2: die Liste 'Person' ist leer
	Angenommen Gesendete Meldung 3 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung: die Liste 'Person' ist leer
	Dann enthält das History Validierungsergebnis keine Fehler

# Inkorrekte Meldungsreihenfolgen
Szenario: Doppelte Aufnahme
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '20.12.2020'
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '20.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Für den Aufenthalt von Klient '(.*)' wurden mehrere Aufnahmen gesendet.'
    Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Fehlende Aufnahme vor Aufenthalt
	Angenommen Gesendete Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 1 enthält Standard Attribute von Person 1 vom '01.12.2020'
	Dann enthält das History Validierungsergebnis den Fehler 'Vor dem Aufenthalt von Klient '(.*)' am '01.12.2020' wurden keine Aufnahmedaten gesendet.'
    Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Fehlende Entlassung, wenn Aufenthalt nicht bis zum Ende des Monats dauert
	Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020 und ist eine Standard Meldung und enthält eine 'Admission' von Person 1 vom 01.12.2020
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021 und ist eine Standard Meldung und enthält einen Aufenthalt
	Angenommen Gesendete Meldung 3 gilt vom '01.02.2021' bis '28.02.2021'
	Angenommen Gesendete Meldung 3 enthält einen Aufenthalt von Person 1 vom '01.02.2021' bis '16.02.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Zum Aufenthaltsende von Klient '(.*)' am '16.02.2021' wurden keine Entlassungsdaten gesendet.'
    Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Fehlende Entlassung, weil Person im nächsten Monat nicht mehr aufscheint, sie hätte entlassen werden müssen
	Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020 und ist eine Standard Meldung und enthält eine 'Admission' von Person 1 vom 20.12.2020
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '20.12.2020'
	Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021 und ist eine Standard Meldung und enthält einen Aufenthalt
	Angenommen Gesendete Meldung 3 gilt vom '01.02.2021' bis '28.02.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Zum Aufenthaltsende von Klient '(.*)' am '31.01.2021' wurden keine Entlassungsdaten gesendet.'
    Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Erneute Aufnahme mit fehlender Entlassung
   	Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020 und ist eine Standard Meldung und enthält eine 'Admission' von Person 1 vom 20.12.2020
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '20.12.2020'
	Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021 und ist eine Standard Meldung und enthält einen Aufenthalt
	Angenommen Gesendete Meldung 3 gilt vom '01.02.2021' bis '28.02.2021'
	Angenommen Gesendete Meldung 3 enthält eine 'Admission' von Person 1 vom '01.02.2021'
	Angenommen Gesendete Meldung 3 enthält Standard Attribute von Person 1 vom '01.02.2021'
	Angenommen Gesendete Meldung 3 enthält einen Aufenthalt von Person 1 vom '01.02.2021' bis '28.02.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Zum Aufenthaltsende von Klient '(.*)' am '31.01.2021' wurden keine Entlassungsdaten gesendet.'
    Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Aufenthalt von Mitte Monat bis Mitte Monat
	Angenommen Existierende Meldung 1 gilt vom '01.10.2016' bis '31.10.2016'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '04.10.2016'
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '04.10.2016'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '04.10.2016' bis '31.10.2016'

	Angenommen Existierende Meldung 2 gilt vom '01.11.2016' bis '30.11.2016'
	Angenommen Existierende Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.11.2016' bis '30.11.2016'

	Angenommen Existierende Meldung 3 gilt vom '01.12.2016' bis '31.12.2016'
	Angenommen Existierende Meldung 3 enthält einen Aufenthalt von Person 1 vom '01.12.2016' bis '31.12.2016'

	Angenommen Existierende Meldung 4 gilt vom '01.01.2017' bis '31.01.2017'
	Angenommen Existierende Meldung 4 enthält einen Aufenthalt von Person 1 vom '01.01.2017' bis '31.01.2017'

	Angenommen Gesendete Meldung 5 gilt vom '01.02.2017' bis '28.02.2017'
	Angenommen Gesendete Meldung 5 enthält einen Aufenthalt von Person 1 vom '01.02.2017' bis '04.02.2017'
    Angenommen Gesendete Meldung 5 enthält eine 'Leaving' von Person 1 vom '04.02.2017'

	Dann enthält das History Validierungsergebnis keine Fehler



# Gleiche Attribute
Szenario: Gleiche Aufnahmeart
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L2' mit Datum '01.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.01.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Die Änderung von 'Aufnahmeart' von Klient '(.*)' am '01.01.2021' auf 'Daueraufnahme' wurde bereits mit der Meldung am '01.12.2020' gesendet.' 
    Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Gleiche Pflegestufe
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'TransitionalAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.01.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Die Änderung von 'Pflegestufe' von Klient '(.*)' am '01.01.2021' auf 'Stufe 1' wurde bereits mit der Meldung am '01.12.2020' gesendet.'
	Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Gleiche Pflegestufe Arge
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L2' mit Datum '01.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.01.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Die Änderung von 'Pflegestufe Arge' von Klient '(.*)' am '01.01.2021' auf 'Stufe 1' wurde bereits mit der Meldung am '01.12.2020' gesendet.'
	Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Gleiche Finanzierung
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L2Ar' mit Datum '01.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.01.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Die Änderung von 'Finanzierung' von Klient '(.*)' am '01.01.2021' auf 'Selbst/Angehörige 100 %' wurde bereits mit der Meldung am '01.12.2020' gesendet.'
	Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Mehrere Aufnahmearten am gleichen Tag
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'TransitionalAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'CareTransitionAt' mit Datum '01.01.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'Die Eigenschaft AdmissionType von (.*) am 01.01.2021 wurde mehrfach am gleichen Tag geändert.'
	Und enthält das History Validierungsergebnis genau einen Fehler

# Änderung Aufnahmeart
Szenario: Keine Änderung von DauerAufnahme auf Urlaub
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'HolidayAt' mit Datum '01.01.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'ist kein Wechsel von einer Daueraufname auf 'Urlaub von der Pflege' möglich.'
	Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Keine Änderung von DauerAufnahme auf Übergangspflege
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'TransitionalAt' mit Datum '01.01.2021'
	Dann enthält das History Validierungsergebnis den Fehler 'ist kein Wechsel von einer Daueraufname auf 'Übergangspflege' möglich'
	Und enthält das History Validierungsergebnis genau einen Fehler

# Zeitlich limitierte Aufnahmearten
Szenario: Urlaub von der Pflege maximal 42 Tage 1
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'HolidayAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält eine 'Leaving' von Person 1 vom '31.01.2021'
	Dann enthält das History Validierungsergebnis keine Fehler

Szenario: Urlaub von der Pflege maximal 42 Tage 2
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'HolidayAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '15.01.2021'
	Dann enthält das escapte History Validierungsergebnis die Warnung 'wurde der Zeitraum für die Aufnahmeart 'Urlaub von der Pflege' überschritten (mehr als 42 Tage).'
	Und enthält das History Validierungsergebnis genau eine Warnung
	Und enthält das History Validierungsergebnis keine Fehler

Szenario: Übergangspflege maximal 365 Tage 1
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'TransitionalAt' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'ContinuousAt' mit Datum '01.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält eine 'Leaving' von Person 1 vom '31.01.2021'
	Dann enthält das History Validierungsergebnis keine Fehler

Szenario: Übergangspflege maximal 365 Tage 2
	Angenommen Existierende Meldung 1 gilt vom '01.12.2019' bis '31.12.2019'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2019'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2019' bis '31.12.2019'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'AdmissionType' mit dem Wert 'TransitionalAt' mit Datum '01.12.2019'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowance' mit dem Wert 'L1' mit Datum '01.12.2019'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'CareAllowanceArge' mit dem Wert 'L0Ar' mit Datum '01.12.2019'
	Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finance' mit dem Wert 'SelfFi' mit Datum '01.12.2019'
	Angenommen Existierende Meldung 2 gilt vom '01.01.2020' bis '31.01.2020'
	Angenommen Existierende Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2020' bis '31.01.2020'
	Angenommen Existierende Meldung 3 gilt vom '01.02.2020' bis '29.02.2020'
	Angenommen Existierende Meldung 3 enthält einen Aufenthalt von Person 1 vom '01.02.2020' bis '29.02.2020'
	Angenommen Existierende Meldung 4 gilt vom '01.03.2020' bis '31.03.2020'
	Angenommen Existierende Meldung 4 enthält einen Aufenthalt von Person 1 vom '01.03.2020' bis '31.03.2020'
	Angenommen Existierende Meldung 5 gilt vom '01.04.2020' bis '30.04.2020'
	Angenommen Existierende Meldung 5 enthält einen Aufenthalt von Person 1 vom '01.04.2020' bis '30.04.2020'
	Angenommen Existierende Meldung 6 gilt vom '01.05.2020' bis '31.05.2020'
	Angenommen Existierende Meldung 6 enthält einen Aufenthalt von Person 1 vom '01.05.2020' bis '31.05.2020'
	Angenommen Existierende Meldung 7 gilt vom '01.06.2020' bis '30.06.2020'
	Angenommen Existierende Meldung 7 enthält einen Aufenthalt von Person 1 vom '01.06.2020' bis '30.06.2020'
	Angenommen Existierende Meldung 8 gilt vom '01.07.2020' bis '31.07.2020'
	Angenommen Existierende Meldung 8 enthält einen Aufenthalt von Person 1 vom '01.07.2020' bis '31.07.2020'
	Angenommen Existierende Meldung 9 gilt vom '01.08.2020' bis '31.08.2020'
	Angenommen Existierende Meldung 9 enthält einen Aufenthalt von Person 1 vom '01.08.2020' bis '31.08.2020'
	Angenommen Existierende Meldung 10 gilt vom '01.09.2020' bis '30.09.2020'
	Angenommen Existierende Meldung 10 enthält einen Aufenthalt von Person 1 vom '01.09.2020' bis '30.09.2020'
	Angenommen Existierende Meldung 11 gilt vom '01.10.2020' bis '31.10.2020'
	Angenommen Existierende Meldung 11 enthält einen Aufenthalt von Person 1 vom '01.10.2020' bis '31.10.2020'
	Angenommen Existierende Meldung 12 gilt vom '01.11.2020' bis '30.11.2020'
	Angenommen Existierende Meldung 12 enthält einen Aufenthalt von Person 1 vom '01.11.2020' bis '30.11.2020'
	Angenommen Gesendete Meldung 13 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Gesendete Meldung 13 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Dann enthält das escapte History Validierungsergebnis die Warnung 'wurde der Zeitraum für die Aufnahmeart 'Übergangspflege' überschritten (mehr als 365 Tage).'
	Und enthält das History Validierungsergebnis genau eine Warnung
	Und enthält das History Validierungsergebnis keine Fehler

# Änderung an den Personendaten
Szenario: Änderung bei Geschlecht
	Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020 und ist eine Standard Meldung und enthält eine 'Admission' von Person 1 vom 20.12.2020
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '20.12.2020'
	Angenommen Existierende Meldung 1: die Eigenschaft 'gender' von 'Admission' ist auf 'MaleGe' gesetzt
	Angenommen Existierende Meldung 1 enthält eine 'Leaving' von Person 1 vom '31.12.2020'

	Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021 und ist eine Standard Meldung und enthält eine 'Admission' von Person 1 vom 01.01.2021
	Angenommen Gesendete Meldung 2 enthält Standard Attribute von Person 1 vom '01.01.2021'
	Angenommen Gesendete Meldung 2: die Eigenschaft 'gender' von 'Admission' ist auf 'FemaleGe' gesetzt
	Angenommen Gesendete Meldung 2: die Eigenschaft 'source_system_id' von 'StatLpReport' ist auf 'System2' gesetzt
	Angenommen Gesendete Meldung 2: die Id von Person 1 ist auf 99 gesetzt

	Dann enthält das History Validierungsergebnis den Fehler 'Unterschied bei 'Geschlecht' von Klient '(.*)' bei Meldung vom 01.01.2021.'
	Und enthält das History Validierungsergebnis genau einen Fehler

Szenario: Änderung bei Geburtsdatum
	Angenommen Existierende Meldung 1 gilt vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1 enthält eine 'Admission' von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält Standard Attribute von Person 1 vom '01.12.2020'
	Angenommen Existierende Meldung 1 enthält einen Aufenthalt von Person 1 vom '01.12.2020' bis '31.12.2020'
	Angenommen Existierende Meldung 1: die Eigenschaft 'birthday' von 'Person' ist auf '01.01.1935' gesetzt

	Angenommen Gesendete Meldung 2 gilt vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2 enthält einen Aufenthalt von Person 1 vom '01.01.2021' bis '31.01.2021'
	Angenommen Gesendete Meldung 2: die Eigenschaft 'birthday' von 'Person' ist auf '01.01.1934' gesetzt

	Angenommen Gesendete Meldung 2: die Eigenschaft 'source_system_id' von 'StatLpReport' ist auf 'System2' gesetzt
	Angenommen Gesendete Meldung 2: die Id von Person 1 ist auf 99 gesetzt

	Angenommen Die History enthält ein Mapping mit Clearing-Id 'gruber.peter.01011934' auf Clearing-Id 'gruber.peter.01011935'

	Dann enthält das History Validierungsergebnis den Fehler 'Unterschied bei 'Geburtsdatum' von Klient '(.*)' bei Meldung vom 01.01.2021'
	Und enthält das History Validierungsergebnis genau einen Fehler

