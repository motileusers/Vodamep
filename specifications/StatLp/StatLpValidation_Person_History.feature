#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Personen einer Datenmeldung


#Szenario: Performance bei großem Datenvolumen
#    Angenommen Im Meldungsbereich für eine Einrichtung befinden sich monatliche Meldungen vom 01.01.2017 bis 31.12.2020
#    Angenommen Im Meldungsbereich für eine Einrichtung befinden sich Meldungen von 100 Klienten
#    Angenommen Im Meldungsbereich für eine Einrichtung wird jeder Klient 5 x aufgenommen und entlassen
#    Angenommen Im Meldungsbereich für eine Einrichtung wird jeder Klient 5 x geändert
#    Dann dauert die Validierung aller Meldungen nicht länger als 5 Sekunden



# Korrekte Meldungsreihenfolgen

#Szenario: Gesamter Aufenthalt korrekt in einer Meldung
#    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 enthält eine Entlassung von Person 1 am 31.12.2020
#    Dann enthält das Validierungsergebnis keinen Fehler
#
#Szenario: Gesamter Aufenthalt korrekt in 3 Meldungen
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Existierende Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 3 gilt vom 01.02.2021 bis 28.02.2021
#    Angenommen Gesendete Meldung 3 enthält eine Aufenthalt von Person 1 vom 01.02.2021 bis 28.02.2021
#    Angenommen Gesendete Meldung 3 enthält eine Entlassung von Person 1 am 28.02.2021
#    Dann enthält das Validierungsergebnis keinen Fehler
#
#

# Fehlende Meldungen

#Szenario: Fehlende Monatsmeldungen mit Leermeldungen
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.03.2021 bis 31.03.2021
#    Dann enthält das Validierungsergebnis den Fehler 'Die Meldungen für den Zeitraum 01.01.2021 bis 28.02.2021 wurden noch nicht übermittelt.'


# Nachsendungen

#Szenario: Nachgesendete Monatsmeldungen zwischen zwei Monaten mit Leermeldungen
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 2 gilt vom 01.03.2021 bis 31.03.2021
#    Angenommen Gesendete Meldung 3 gilt vom 01.01.2021 bis 31.01.2021
#    Dann enthält das Validierungsergebnis keinen Fehler


# Unkorrekte Meldungsreihenfolgen

#Szenario: Doppelte Aufnahme
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 20.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 ist eine Standard Aufnahme Meldung von Person 1 mit 01.01.2021
#    Dann enthält das Validierungsergebnis den Fehler 'Für Klient xx wurde bereits eine Aufnahme am 20.12.2020 gesendet'
#
#Szenario: Fehlende Aufnahme vor Aufenthalt
#    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Dann enthält das Validierungsergebnis den Fehler 'Vor dem Aufenthalt von Klient xx am xx wurden keine Aufnahmedaten gesendet'
#
#Szenario: Fehlende Entlassung, wenn Aufenthalt nicht bis zum Ende des Monats dauert
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 20.12.2020
#    Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Existierende Meldung 2 ist eine Standard Aufenthaltsmeldung Meldung von Person 1
#    Angenommen Gesendete Meldung 3 gilt vom 01.02.2021 bis 28.02.2021
#    Angenommen Gesendete Meldung 3 enthält eine Aufenthalt von Person 1 vom 01.02.2020 bis 16.02.2020
#    Dann enthält das Validierungsergebnis den Fehler 'Zur Aufenthaltsende von Klient xx am xx wurden keine Entlassungsdaten gesendet'
#
#Szenario: Erneute Aufnahme mit fehlender Entlassung
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 20.12.2020
#    Angenommen Existierende Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Existierende Meldung 2 ist eine Standard Aufenthaltsmeldung Meldung von Person 1
#    Angenommen Existierende Meldung 1 gilt vom 01.02.2021 bis 28.02.2021
#    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 01.02.2021
#    Dann enthält das Validierungsergebnis den Fehler 'Aufnahme von Klient xx am xx nicht möglich, weil keine Entlassung gesendet wurde'
#
#

# Fehlende Pflichtfelder für die Aufnahme

#Szenario: Fehlende Aufnahmeart vor der Aufnahme
#    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient xx am xx wurde keine Aufnahmeart gesendet'
#
#Szenario: Fehlende Pflegestufe vor der Aufnahme
#    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient xx am xx wurde keine Pflegestufe gesendet'
#
#Szenario: Fehlende Pflegestufe Arge vor der Aufnahme
#    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient xx am xx wurde keine Pflegestufe (Arge) gesendet'
#
#Szenario: Fehlende Finanzierung vor der Aufnahme
#    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient xx am xx wurde keine Finanzierung gesendet'

#Szenario: Falsches Gültigkeitsdatum einer Aufnahme
#    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.1999
#    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Dann enthält das Validierungsergebnis den Fehler 'Das Gültigkeitsdatum der Aufnahme von Klient xx muss im Meldungszeitraum liegen'


# Entlassung

#Szenario: Falsches Gültigkeitsdatum einer Entlassung
#    Angenommen Gesendete Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Gesendete Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 1 enthält eine Entlassung von Person 1 am 31.12.1999
#    Dann enthält das Validierungsergebnis den Fehler 'Das Gültigkeitsdatum der Entlassung von Klient xx muss im Meldungszeitraum liegen'



# Gleiche Attribute

#Szenario: Gleiche Aufnahmeart
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 2' mit Datum '01.01.2020'
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.01.2020'
#    Dann enthält das Validierungsergebnis den Fehler 'Die Änderung der Aufnahmeart von Klient xx auf Daueraufnahme wurde bereits mit der Meldung am 01.12.2020 gesendet'
#
#Szenario: Gleiche Pflegestufe
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Übergangspflege' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.01.2020'
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.01.2020'
#    Dann enthält das Validierungsergebnis den Fehler 'Die Änderung der Pflegestufe von Klient xx auf Pflegestufe 1 wurde bereits mit der Meldung am 01.12.2020 gesendet'
#
#Szenario: Gleiche Pflegestufe Arge
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 2' mit Datum '01.12.2020'
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.01.2020'
#    Dann enthält das Validierungsergebnis den Fehler 'Die Änderung der Pflegestufe Arge von Klient xx auf Pflegestufe 1 wurde bereits mit der Meldung am 01.12.2020 gesendet'
#
#Szenario: Gleiche Finanzierung
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 2' mit Datum '01.01.2020'
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Dann enthält das Validierungsergebnis den Fehler 'Die Änderung der Finanzierung von Klient xx auf Selbst/Angehörige 100 % wurde bereits mit der Meldung am 01.12.2020 gesendet'




# Mehrere Aufnahmearten am gleichen Tag

#Szenario: Mehrere Aufnahmearten am gleichen Tag
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Übergangspflege' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.01.2020'
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Überleitungspflege' mit Datum '01.01.2020'
#    Dann enthält das Validierungsergebnis den Fehler 'Die Aufnahmeart von Klient xx am xx wurde mehrfach am gleichen Tag geändert.'
#



# Änderung Aufnahmeart

#Szenario: Keine Änderung von Daueraufnahme auf Urlaub
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Urlaub von der Pflege' mit Datum '01.01.2020'
#    Dann enthält das Validierungsergebnis den Fehler 'Bei Klient xx ist kein Wechsel von einer Daueraufname auf xx möglich'
#
#Szenario: Keine Änderung von Daueraufnahme auf Übergangspflege
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Übergangspflege' mit Datum '01.01.2020'
#    Dann enthält das Validierungsergebnis den Fehler 'Bei Klient xx ist kein Wechsel von einer Daueraufname auf xx möglich'






# Zeitlich limitierte Aufnahmearten


#Szenario: Urlaub von der Pflege maximal 42 Tage
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Urlaub von der Pflege' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.01.2021'
#    Dann enthält das Validierungsergebnis keinen Fehler


#Szenario: Urlaub von der Pflege maximal 42 Tage
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Urlaub von der Pflege' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '15.01.2021'
#    Dann enthält das Validierungsergebnis den Fehler 'Bei Klient xx wurde der Zeitraum für die Aufnahmeart {0} überschritten (mehr als 42 Tage).'


#Szenario: Übergangspflege maximal 365 Tage
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Übergangspflege' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Daueraufnahme' mit Datum '01.01.2021'
#    Dann enthält das Validierungsergebnis keinen Fehler


#Szenario: Übergangspflege maximal 365 Tage
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2019 bis 31.12.2019
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Aufnahmeart' mit dem Wert 'Urlaub von der Pflege' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Pflegestufe Arge' mit dem Wert 'Pflegestufe 1' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 von Person 1 enthält das Attribut 'Finanzierung' mit dem Wert 'Selbst/Angehörige 100 %' mit Datum '01.12.2020'
#    Angenommen Existierende Meldung 1 enthält eine Aufnahme von Person 1 vom 01.12.2020
#    Angenommen Existierende Meldung 1 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 2 gilt vom 01.01.2020 bis 31.01.2020
#    Angenommen Existierende Meldung 2 enthält eine Aufenthalt von Person 1 vom 01.01.2020 bis 31.01.2020
#    Angenommen Existierende Meldung 3 gilt vom 01.02.2020 bis 28.02.2020
#    Angenommen Existierende Meldung 3 enthält eine Aufenthalt von Person 1 vom 01.02.2020 bis 28.02.2020
#    Angenommen Existierende Meldung 4 gilt vom 01.03.2020 bis 31.03.2020
#    Angenommen Existierende Meldung 4 enthält eine Aufenthalt von Person 1 vom 01.03.2020 bis 31.03.2020
#    Angenommen Existierende Meldung 5 gilt vom 01.04.2020 bis 30.04.2020
#    Angenommen Existierende Meldung 5 enthält eine Aufenthalt von Person 1 vom 01.04.2020 bis 30.04.2020
#    Angenommen Existierende Meldung 6 gilt vom 01.05.2020 bis 31.05.2020
#    Angenommen Existierende Meldung 6 enthält eine Aufenthalt von Person 1 vom 01.05.2020 bis 31.05.2020
#    Angenommen Existierende Meldung 7 gilt vom 01.06.2020 bis 30.06.2020
#    Angenommen Existierende Meldung 7 enthält eine Aufenthalt von Person 1 vom 01.06.2020 bis 30.06.2020
#    Angenommen Existierende Meldung 8 gilt vom 01.07.2020 bis 31.07.2020
#    Angenommen Existierende Meldung 8 enthält eine Aufenthalt von Person 1 vom 01.07.2020 bis 31.07.2020
#    Angenommen Existierende Meldung 9 gilt vom 01.08.2020 bis 31.08.2020
#    Angenommen Existierende Meldung 9 enthält eine Aufenthalt von Person 1 vom 01.08.2020 bis 31.08.2020
#    Angenommen Existierende Meldung 10 gilt vom 01.09.2020 bis 30.09.2020
#    Angenommen Existierende Meldung 10 enthält eine Aufenthalt von Person 1 vom 01.09.2020 bis 30.09.2020
#    Angenommen Existierende Meldung 11 gilt vom 01.10.2020 bis 31.10.2020
#    Angenommen Existierende Meldung 11 enthält eine Aufenthalt von Person 1 vom 01.10.2020 bis 31.10.2020
#    Angenommen Existierende Meldung 12 gilt vom 01.10.2020 bis 31.10.2020
#    Angenommen Existierende Meldung 12 enthält eine Aufenthalt von Person 1 vom 01.11.2020 bis 30.11.2020
#    Angenommen Gesendete Meldung 13 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Gesendete Meldung 13 enthält eine Aufenthalt von Person 1 vom 01.12.2020 bis 31.12.2020
#    Dann enthält das Validierungsergebnis den Fehler 'Bei Klient xx wurde der Zeitraum für die Aufnahmeart {0} überschritten (mehr als 365 Tage).'






# Änderung an den Personendaten 

#Szenario: Änderung bei Geschlecht
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 20.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 ist eine Standard Aufenthaltsmeldung Meldung von Person 1
#    Angenommen In gesendeter Meldung 2 wird das Geschlecht von Person 1 auf 'weiblich' geändert
#    Dann enthält das Validierungsergebnis den Fehler 'Unterschied bei Geschlecht von Klient xx bei Meldung vom xx'
#
#Szenario: Änderung bei Geburtsdatum
#    Angenommen Existierende Meldung 1 gilt vom 01.12.2020 bis 31.12.2020
#    Angenommen Existierende Meldung 1 ist eine Standard Aufnahme Meldung von Person 1 mit 20.12.2020
#    Angenommen Gesendete Meldung 2 gilt vom 01.01.2021 bis 31.01.2021
#    Angenommen Gesendete Meldung 2 ist eine Standard Aufenthaltsmeldung Meldung von Person 1
#    Angenommen In gesendeter Meldung 2 wird das Geburtsdatum von Person 1 auf '01.01.1934' geändert
#    Dann enthält das Validierungsergebnis den Fehler 'Unterschied bei Geburtsdatum von Klient xx bei Meldung vom xx'

