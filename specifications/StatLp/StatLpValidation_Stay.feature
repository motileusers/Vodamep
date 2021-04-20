#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Aufenthalte einer Datenmeldung

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

Szenariogrundriss: Ein Stay muss im aktuellen Monat liegenn
    Angenommen die Eigenschaft '<Name>' von 'Stay' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Ein Aufenthalt von Person '1' muss im aktuellen Monat liegen.'
Beispiele:
    | Name     | Wert       |
    | from     | 2000-01-01 |
    | to       | 2050-01-01 |

Szenario: Bis ist nach Von
    Angenommen Bis ist vor Von bei einem Stay
    Dann enthält das Validierungsergebnis den Fehler ''Von' muss vor 'Bis' liegen.'
