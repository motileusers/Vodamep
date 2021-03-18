#language: de-DE
Funktionalität: StatLp - Validierung der Einrichtung

Szenariogrundriss: Die Einrichtungsnummer ist falsch.
    Angenommen die Eigenschaft 'id' von 'Institution' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Ungültige Einrichtungsnummer.'
Beispiele:
    | Wert  |
    | A     |
    | 0     |
    | AAAA  |
    | 0000  |

