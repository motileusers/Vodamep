#language: de-DE
Funktionalität: Mkkp - Validierung der gemeldeten Aktivitäten der Datenmeldung

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Activity' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler '<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name              | Bezeichnung     |
    | date              | Datum           |
    | person_id         | Personen-ID     |
    | staff_id          | Mitarbeiter-ID  |
    | place_of_Action   | Einsatzort      |

Szenario: Leistungszeit muss > 0 sein 
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Leistungszeit' muss grösser sein als '0'.'

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

Szenariogrundriss: Minuten Werte müssen > 0 sein
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Der Wert von 'Leistungszeit' muss grösser sein als '0'.'
Beispiele: 
    | Wert |
    | 0 |
    | -1 |

Szenariogrundriss: Minuten  dürfen nur in 5 Minuten Schritten eingegeben werden
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Minutes dürfen nur in 5 Minuten Schritten eingegeben werden.'
Beispiele: 
    | Wert |
    | 1 |
    | 3 |
    | 17 |

Szenariogrundriss: Minuten  dürfen nur in 5 Minuten Schritten eingegeben werden - korrekt
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
    Und es enthält keine Warnungen
Beispiele: 
    | Wert |
    | 5 |
    | 10 |
    | 15 |

Szenario: Summe Leistungsminuten pro Tag / pro Mitarbeiter darf 10 Stunden nicht überschreiten
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist auf '605' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Summe Leistungsminuten pro Tag / pro Mitarbeiter darf 10 Stunden nicht überschreiten.'

Szenariogrundriss: TravelTime Werte müssen > 0 sein
    Angenommen die Eigenschaft 'minutes' von 'TravelTime' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Der Wert von 'Leistungszeit' muss grösser sein als '0'.'
Beispiele: 
    | Wert |
    | 0 |
    | -1 |

Szenario: Summe TravelTimes darf 5 Stunden nicht überschreiten
    Angenommen die Eigenschaft 'minutes' von 'TravelTimes' ist auf '305' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Summe Reisezeiten darf 5 Stunden nicht überschreiten.'

Szenario: Traveltimes Nur 1 Eintrag pro Mitarbeiter pro Tag
    Angenommen es werden zusätzliche Reisezeiten für einen Mitarbeiter eingetragen
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Pro Mitarbeiter ist nur ein Eintrag bei den Reisezeite pro Tag erlaubt.'
 
Szenario: Mehrfache Leistungen pro Klient pro Tag
    Angenommen es werden zusätzliche Leistungen pro Klient an einem Tag eingetragen
    Dann enthält das Validierungsergebnis keine Fehler
    Und es enthält keine Warnungen

Szenario: Mehrfache Leistungstypen pro Leistung
    Angenommen die Leistungstypen 'Body,MedicalDiet' sind für eine Aktivität gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
    Und es enthält keine Warnungen

Szenario: Doppelte Leistungen innerhalb einer Aktivität
    Angenommen die Leistungstypen 'MedicalDiet,MedicalDiet' sind für eine Aktivität gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Innerhalb einer Aktivität dürfen keine doppelten Leistungstypen vorhanden sein.'

Szenario: Es muss mindestens ein Leistungstyp pro Leistung vorhanden sein
   Angenommen die Leistungstypen '' sind für eine Aktivität gesetzt
   Dann enthält das Validierungsergebnis den Fehler 'Leistungsbereiche' darf nicht leer sein.'

Szenario: Eine Aktivität ist nach dem Meldungszeitraum.
    Angenommen die Eigenschaft 'date' von 'Activity' ist auf '2058-09-29' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Datum' muss kleiner oder gleich (.*) sein'
    
Szenario: Eine Aktivität ist vor dem Meldungszeitraum.
    Angenommen die Eigenschaft 'date' von 'Activity' ist auf '2008-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Datum' muss grösser oder gleich (.*) sein.'

#todo: Test anpassen Anpassen
#Szenario: Eine Aktivität ohne entsprechenden Eintrag in Persons.
#    Angenommen die Meldung enthält bei der Person 'unbekannteId'
#    Dann enthält das Validierungsergebnis den Fehler 'Der Id 'unbekannteId' fehlt'

#todo: Test anpassen Anpassen
#Szenario: Eine Aktivität ohne entsprechenden Eintrag in Staffs.
#    Angenommen die Meldung enthält von der Mitarbeiterin 'unbekannteId' die Aktivitäten '02,15'
#    Dann enthält das Validierungsergebnis den Fehler 'Der Id 'unbekannteId' fehlt'

Szenario: Eine Person ohne Aktivität.
    Angenommen zu einer Person sind keine Aktivitäten dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'Keine Aktivitäten'

Szenario: Eine Mitarbeiterin ohne Aktivität.
    Angenommen zu einer Mitarbeiterin sind keine Aktivitäten dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'Keine Aktivitäten'
