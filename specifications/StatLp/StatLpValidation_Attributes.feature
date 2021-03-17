#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Aufenthalte einer Datenmeldung


# Ein Attribute enthält eine Person, die nicht in der Personenliste ist -> Fehler

# Ein Attribute (From) muss im akutellen Monat liegen
# From darf keine Zeit beinhalten



Szenario: Geschlecht ist undefiniert
    Angenommen die Eigenschaft 'gender' von 'Person' ist auf 'UndefinedGender' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Geschlecht' darf nicht leer sein.'

Szenario: Pflegegeld ist undefiniert
    Angenommen die Eigenschaft 'care_allowance' von 'Person' ist auf 'UndefinedAllowance' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Pflegegeld' darf nicht leer sein.'


