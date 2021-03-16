#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Personen einer Datenmeldung

# Korrekte Meldungsreihenfolgen

Szenario: Gesamter Aufenthalt korrekt in einer Meldung
    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
    Angenommen Gesendete Meldung 1 enthält eine Entlassung von Person 1 am 31.12.2020
    Dann enthält das Validierungsergebnis keinen Fehler

Szenario: Gesamter Aufenthalt korrekt in 3 Meldungen
    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
    Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
    Angenommen Existierende Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
    Angenommen Gesendete Meldung 3 gilt vom 01.02.2021 bis 28.02.2021
    Angenommen Gesendete Meldung 3 enthält eine Aufenthalt von Person 1 vom 01.02.2021 bis 28.02.2021
    Angenommen Gesendete Meldung 3 enthält eine Entlassung von Person 1 am 28.02.2021
    Dann enthält das Validierungsergebnis keinen Fehler



# Fehlenden Sendungen

Szenario: Fehlende Monatsmeldungen mit Leermeldungen
    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Gesendete Meldung 2 gilt vom 01.03.2021 bis 31.03.2021
    Dann enthält das Validierungsergebnis den Fehler 'Die Meldungen für den Zeitraum 01.01.2021 bis 28.02.2021 wurden noch nicht übermittelt.'


# Nachsendungen

Szenario: Nachgesendete Monatsmeldungen zwischen zwei Monaten mit Leermeldungen
    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Existierende Meldung 2 gilt vom 01.03.2021 bis 31.03.2021
    Angenommen Gesendete Meldung 3 gilt vom 01.01.2021 bis 31.01.2021
    Dann enthält das Validierungsergebnis keinen Fehler


# Unkorrekte Meldungsreihenfolgen

Szenario: Fehlende Aufnahme vor Aufenthalt
    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
    Dann enthält das Validierungsergebnis den Fehler 'Vor dem Aufenthalt von Klient xx am xx wurden keine Aufnahmedaten gesendet'

Szenario: Fehlende Entlassung
    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 20.12.2020
    Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
    Angenommen Existierende Meldung 2 ist eine Standard Aufenthaltsmeldung Meldung von Person 1
    Angenommen Gesendete Meldung 3 gilt vom 01.02.2021 bis 28.02.2021
    Angenommen Gesendete Meldung 3 ist eine Standard Entlassungsmeldung Meldung von Person 1 mit 15.02.2021
    Dann enthält das Validierungsergebnis den Fehler 'Zur Aufenthaltsende von Klient xx am xx wurden keine Entlassungsdaten gesendet'

Szenario: Erneute Aufnahme mit fehlender Entlassung
    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 20.12.2020
    Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
    Angenommen Existierende Meldung 2 ist eine Standard Aufenthaltsmeldung Meldung von Person 1
    Angenommen Existierende Meldung 1 gilt vom 01.02.2021 bis 28.02.2021
    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 01.02.2021
    Dann enthält das Validierungsergebnis den Fehler 'Aufnahme von Klient xx am xx nicht möglich, weil keine Entlassung gesendet wurde'



# Fehlende Pflichtfelder für die Aufnahme

Szenario: Fehlende Aufnahmeart vor der Aufnahme
    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient xx am xx wurde keine Aufnahmeart gesendet'

Szenario: Fehlende Pflegestufe vor der Aufnahme
    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient xx am xx wurde keine Pflegestufe gesendet'

Szenario: Fehlende Pflegestufe Arge vor der Aufnahme
    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient xx am xx wurde keine Pflegestufe (Arge) gesendet'

Szenario: Fehlende Finanzierung vor der Aufnahme
    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient xx am xx wurde keine Finanzierung gesendet'



