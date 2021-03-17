#language: de-DE
Funktionalität: StatLp - Validierung der Einrichtung



# Einrichtungsnummer: Numerisch, genau 4 Zeichen, nicht 0000
# RegEx "^[0-9]{3,4"
Szenariogrundriss: Die Einrichtungsnummer ist falsch
    Angenommen die Eigenschaft '<Name>' von 'StatLpReport' enhält den Wert '<Wert>'
    Dann enthält das Validierungsergebnis den Fehler 'Ungültige Einrichtungsnummer'
Beispiele:
    | Name     | Wert  |
    | id       | A     |
    | id       | 0     |
    | id       | AAAA  |
    | id       | 0000  |
