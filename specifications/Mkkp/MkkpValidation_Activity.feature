﻿#language: de-DE
Funktionalität: Mkkp - Validierung der gemeldeten Aktivitäten der Datenmeldung

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Activity' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Aktivität von Klient 'Peter Gruber' darf nicht leer sein.'
Beispiele:
    | Name              | Bezeichnung     |
    | date              | Datum           |
    | staff_id          | Mitarbeiter-ID  |
    | place_of_Action   | Einsatzort      |
 
Szenario: Pesonen ID ist nicht gesetzt
    Angenommen die Eigenschaft 'person_id' von 'Activity' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Personen-ID' von Aktivität darf nicht leer sein.'

Szenario: Leistungszeit muss > 0 sein 
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Leistungszeit von Klient 'Peter Gruber' muss größer 0 sein.'

#Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
#    Angenommen die Datums-Eigenschaft '<Name>' von 'Activity' hat eine Uhrzeit gesetzt
#    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient 'Peter Gruber' darf keine Uhrzeit beinhalten.'
#Beispiele:
#    | Name | Bezeichnung |
#    | date | Datum       |

Szenariogrundriss: Minuten Werte müssen > 0 sein
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Leistungszeit von Klient 'Peter Gruber' muss größer 0 sein.'
Beispiele: 
    | Wert |
    | 0 |
    | -1 |

Szenariogrundriss: Minuten  dürfen nur in 5 Minuten Schritten eingegeben werden
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Leistungszeit von 'Peter Gruber' darf nur in 5 Minuten Schritten eingegeben werden.'
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

Szenario: Summe Leistungsminuten pro Tag / pro Mitarbeiter darf 12 Stunden nicht überschreiten
    Angenommen die Eigenschaft 'minutes' von 'Activity' ist auf '725' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Die Leistungsminuten von '(.*)' am '(.*)' dürfen 12 Stunden nicht überschreiten.'

Szenariogrundriss: TravelTime Werte müssen > 0 sein
    Angenommen die Eigenschaft 'minutes' von 'TravelTime' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Der Wert von'Leistungszeit' von Wegzeit von Mitarbeiter 'Peter Gruber' muss größer 0 sein.'
Beispiele: 
    | Wert |
    | 0 |
    | -1 |

Szenario: Summe TravelTimes darf 5 Stunden nicht überschreiten
    Angenommen die Eigenschaft 'minutes' von 'TravelTime' ist auf '305' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Summe Wegzeiten von Mitarbeiter (.*) am (.*) darf 5 Stunden nicht überschreiten.'

Szenario: Traveltimes Nur 1 Eintrag pro Mitarbeiter pro Tag
    Angenommen es werden zusätzliche Wegzeiten für einen Mitarbeiter eingetragen
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Pro Mitarbeiter ist nur ein Eintrag bei den Wegzeiten pro Tag erlaubt.'
 
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
    Dann enthält das Validierungsergebnis den Fehler 'Innerhalb einer Aktivität von Klient '(.*)' dürfen keine doppelten Leistungstypen vorhanden sein.'
    
Szenario: Leistung darf bestimmte Kombinationen nicht enhalten
    Angenommen  die Leistungstypen 'AccompanyingWithContact,AccompanyingWithoutContact' sind für eine Aktivität gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Innerhalb einer Aktivität (.*) dürfen nicht gleichzeitg die Leistungstypen 'Begleitende Maßnahmen mit Patientenkontakt' und 'Begleitende Maßnahmen ohne Patientenkontakt' vorhanden sein.'

Szenario: Es muss mindestens ein Leistungstyp pro Leistung vorhanden sein
   Angenommen die Leistungstypen '' sind für eine Aktivität gesetzt
   Dann enthält das Validierungsergebnis den Fehler ''Leistungsbereiche' von Aktivität von Klient 'Peter Gruber' darf nicht leer sein.'

Szenario: Eine Aktivität ist nach dem Meldungszeitraum.
    Angenommen die Eigenschaft 'date' von 'Activity' ist auf '2058-09-29' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Datum' der Aktivität (.*) muss innerhalb des Meldungszeitraums liegen.'
    
Szenario: Eine Aktivität ist vor dem Meldungszeitraum.
    Angenommen die Eigenschaft 'date' von 'Activity' ist auf '2008-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Datum' der Aktivität (.*) muss innerhalb des Meldungszeitraums liegen.'

Szenario: Eine Aktivität ohne entsprechenden Eintrag in Persons
    Angenommen die Eigenschaft 'person_id' von 'Activity' ist auf '-1' gesetzt
    Dann enthält das escapte Validierungsergebnis den Fehler 'Eine Aktivität mit der ID '1' ist keiner vorhandenen Person (ID '-1') zugeordnet.'

Szenario: Eine Aktivität ohne entsprechenden Eintrag in Mitarbeiter
    Angenommen die Eigenschaft 'staff_id' von 'Activity' ist auf '-1' gesetzt
    Dann enthält das escapte Validierungsergebnis den Fehler 'Eine Aktivität mit der ID '1' ist keinem vorhandenen Mitarbeiter (ID '-1') zugeordnet.'

Szenario: Eine Person ohne Aktivität.
    Angenommen zu einer Person sind keine Aktivitäten dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'Keine Aktivitäten'

Szenario: Eine Mitarbeiterin ohne Aktivität.
    Angenommen zu einer Mitarbeiterin sind keine Aktivitäten dokumentiert
    Dann enthält das Validierungsergebnis den Fehler 'Keine Aktivitäten'

Szenario: Einsatzort ist undefiniert
    Angenommen die Eigenschaft 'place_of_Action' von 'Activity' ist auf 'UndefinedPlace' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Einsatzort' von Aktivität von Klient 'Peter Gruber' darf nicht leer sein.'