#language: de-DE
Funktionalität: StatLp - Validierung der Datenmeldung

Szenario: Korrekt befüllt
    Angenommen es ist ein 'StatLpReport'
    Und eine Meldung ist korrekt befüllt
    Dann enthält das Validierungsergebnis keine Fehler
    Und enthält das Validierungsergebnis keine Warnungen
    
Szenario: Von-Datum muss der erste Tag des Jahres sein.
    Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft 'from' von 'StatLpReport' ist auf '2021-04-04' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Von' muss der erste Tag des Jahres sein.'

Szenario: Bis-Datum muss der letzte Tag des Jahres sein.
    Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft 'to' von 'StatLpReport' ist auf '2021-04-04' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Bis' muss der letzte Tag des Monats sein.'

Szenario: Die Meldung darf nicht die Zukunft betreffen.
    Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft 'to' von 'StatLpReport' ist auf '2058-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Bis' muss kleiner oder gleich .*'

Szenario: Die Meldung darf keinen Jahreswechsel beinhalten
    Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft 'from' von 'StatLpReport' ist auf '2020-01-01' gesetzt
    Und die Eigenschaft 'to' von 'StatLpReport' ist auf '2021-12-31' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Die Meldung darf keinen Jahreswechsel beinhalten'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft '<Name>' von 'StatLpReport' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name        | Bezeichnung |
    | from        | Von         |
    | to          | Bis         |
    | institution | Einrichtung |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen es ist ein 'StatLpReport'
    Und die Datums-Eigenschaft '<Name>' von 'StatLpReport' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name     | Bezeichnung |
    | from     | Von         |
    | to       | Bis         |

Szenariogrundriss: Listen sind leer
    Angenommen es ist ein 'StatLpReport'
    Und alle Listen sind leer
    Dann enthält das Validierungsergebnis keine Fehler
    Und enthält das Validierungsergebnis keine Warnungen