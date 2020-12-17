#language: de-DE
Funktionalität: Mkkp - Validierung der Datenmeldung

Szenario: Korrekt befüllt
    Angenommen Mkkp: eine Meldung ist korrekt befüllt
    Dann enthält das Mkkp Validierungsergebnis keine Fehler
    Und enthält das Mkkp Validierungsergebnis keine Warnungen
    
Szenario: Von-Datum muss der erste Tag des Monats sein.
    Angenommen Mkkp: die Eigenschaft 'from' von 'MkkpReport' ist auf '2018-04-04' gesetzt
    Dann enthält das Mkkp Validierungsergebnis den Fehler ''Von' muss der erste Tag des Monats sein.'

Szenario: Bis-Datum muss der letzte Tag des Monats sein.
    Angenommen Mkkp: die Eigenschaft 'to' von 'MkkpReport' ist auf '2018-04-04' gesetzt
    Dann enthält das Mkkp Validierungsergebnis den Fehler ''Bis' muss der letzte Tag des Monats sein.'

Szenario: Die Meldung muss genau einen Monat beinhalten.
    Angenommen Mkkp: die Eigenschaft 'from' von 'MkkpReport' ist auf '2018-03-01' gesetzt
        Und Mkkp: die Eigenschaft 'to' von 'MkkpReport' ist auf '2018-04-30' gesetzt
    Dann enthält das Mkkp Validierungsergebnis den Fehler 'Die Meldung muss genau einen Monat beinhalten.'

Szenario: Die Meldung darf nicht die Zukunft betreffen.
    Angenommen Mkkp: die Eigenschaft 'to' von 'MkkpReport' ist auf '2058-04-30' gesetzt
    Dann enthält das Mkkp Validierungsergebnis den Fehler 'Der Wert von 'Bis' muss kleiner oder gleich .*'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen Mkkp: die Eigenschaft '<Name>' von 'MkkpReport' ist nicht gesetzt
    Dann enthält das Mkkp Validierungsergebnis genau einen Fehler
    Und die Mkkp Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name        | Bezeichnung |
    | from        | Von         |
    | to          | Bis         |
    | institution | Einrichtung |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen Mkkp: die Datums-Eigenschaft '<Name>' von 'MkkpReport' hat eine Uhrzeit gesetzt
    Dann enthält das Mkkp Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name     | Bezeichnung |
    | from     | Von         |
    | to       | Bis         |
    