#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Stays einer Datenmeldung

Szenario: Ein Stay enthält eine Person, die nicht in der Personenliste ist
    Angenommen die Eigenschaft 'person_id' von 'Stay' ist auf '2' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Person '2' ist nicht in der Personenliste vorhanden.'

Szenariogrundriss: Datumswerte dürfen keine Zeit beinhalten
    Angenommen die Datums-Eigenschaft '<Name>' von 'Stay' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name          | Bezeichnung   |
    | from          | Von           |
    | to            | Bis           |

# Ein Stay (From,To) muss im akutellen Monat liegen
# From muss <= to sein

Szenario: Bis ist nach Von
    Angenommen Bis ist vor Von bei einem Stay
    Dann enthält das Validierungsergebnis den Fehler ''Von' muss vor 'Bis' liegen.'
