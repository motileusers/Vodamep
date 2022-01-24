#language: de-DE
Funktionalität: Cm - Validierung der Datenmeldung

Szenario: Korrekt befüllt
    Angenommen es ist ein 'CmReport'
    Und eine Meldung ist korrekt befüllt
    Dann enthält das Validierungsergebnis keine Fehler
    Und enthält das Validierungsergebnis keine Warnungen
   
Szenario: Von-Datum muss der erste Tag des Monats sein.
    Angenommen es ist ein 'CmReport'
    Und die Eigenschaft 'from' von 'CmReport' ist auf '2018-04-04' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Von' muss der erste Tag des Monats sein.'

Szenario: Bis-Datum muss der letzte Tag des Monats sein.
    Angenommen es ist ein 'CmReport'
    Und die Eigenschaft 'to' von 'CmReport' ist auf '2018-04-04' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Bis' muss der letzte Tag des Monats sein.'

Szenario: Die Meldung muss genau einen Monat beinhalten.
    Angenommen es ist ein 'CmReport'
    Und die Eigenschaft 'from' von 'CmReport' ist auf '2018-03-01' gesetzt
    Und die Eigenschaft 'to' von 'CmReport' ist auf '2018-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Die Meldung muss genau einen Monat beinhalten.'

Szenario: Die Meldung darf nicht die Zukunft betreffen.
    Angenommen es ist ein 'CmReport'
    Und die Eigenschaft 'to' von 'CmReport' ist auf '2058-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Bis' muss kleiner oder gleich .*'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen es ist ein 'CmReport'
    Und die Eigenschaft '<Name>' von 'CmReport' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name        | Bezeichnung |
    | from        | Von         |
    | to          | Bis         |
    | institution | Einrichtung |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen es ist ein 'CmReport'
    Und die Datums-Eigenschaft '<Name>' von 'CmReport' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name     | Bezeichnung |
    | from     | Von         |
    | to       | Bis         |

Szenariogrundriss: Listen sind leer
    Angenommen es ist ein 'CmReport'
    Und die Liste '<Name>' ist leer 
    Dann enthält das Validierungsergebnis keine Fehler
    Und enthält das Validierungsergebnis keine Warnungen
Beispiele:
    | Name              |
    | Activity          |    
    | ClientActivity    |
    | Person,ClientActivity   |
    