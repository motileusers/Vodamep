#language: de-DE
Funktionalität: Mkkp - Validierung der gemeldeten Aktivitäten der Datenmeldung

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Activity' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name              | Bezeichnung     |
    | date              | Datum           |
    | person_id         | Personen-ID     |
    | staff_id          | Mitarbeiter-ID  |
    | minutes           | Leistungszeit   |
    | place_of_Action   | Einsatzort      |


Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen die Datums-Eigenschaft '<Name>' von 'Activity' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name | Bezeichnung |
    | date | Datum       |

#todo: Alle Aktivitätstypen aus dem csv müssen als enum vorhanden sein (Normaler Test, ohne Specflow?)
#todo: und umgekehrt
#todo: Alle Einsatzorte aus dem csv müssen als enum vorhanden sein (Normaler Test, ohne Specflow?)
#todo: und umgekehrt

#todo: PlaceOfAction muss aus der Liste der places_of_action kommen


#todo: minutes Werte Bereich: 10 - 210 Minuten
#todo: minutes dürfen nur in 5 Minuten Schritten eingegeben werden


#todo: Mehrfache Leistungstypen pro Leistung --> kein Fehler
#todo: Mehrfache Leistungen pro Klient am gleichen Tag
#todo: keine doppelten Leistungstypen innerhalb einer Aktivität
#todo: mindestens 1 Leistungstyp pro Leistung vorhanden

#todo: PatientContact nur erlaubt, wenn nur ein Leistungstyp (und nur dieser) mit Accompanying vorhanden ist


#todo: Alle folgenden Tests müssen für diesen Report angepasst werden
Szenario: Eine Aktivität ist nach dem Meldungszeitraum.
    Angenommen die Meldung enthält am '2058-04-30'
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Datum' muss kleiner oder gleich (.*) sein'

#todo: Test anpassen Anpassen
Szenario: Eine Aktivität ist vor dem Meldungszeitraum.
    Angenommen die Meldung enthält am '2008-04-30'
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Datum' muss grösser oder gleich (.*) sein.'

#todo: Test anpassen Anpassen
Szenario: Eine Aktivität ohne entsprechenden Eintrag in Persons.
    Angenommen die Meldung enthält bei der Person 'unbekannteId'
    Dann enthält das Validierungsergebnis den Fehler 'Der Id 'unbekannteId' fehlt'

#todo: Test anpassen Anpassen
Szenario: Eine Aktivität ohne entsprechenden Eintrag in Staffs.
    Angenommen die Meldung enthält von der Mitarbeiterin 'unbekannteId' die Aktivitäten '02,15'
    Dann enthält das Validierungsergebnis den Fehler 'Der Id 'unbekannteId' fehlt'

Szenario: Eine Person ohne Aktivität.
    Angenommen zu einer Person sind keine Aktivitäten dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'Keine Aktivitäten'

Szenario: Eine Mitarbeiterin ohne Aktivität.
    Angenommen zu einer Mitarbeiterin sind keine Aktivitäten dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'Keine Aktivitäten'
