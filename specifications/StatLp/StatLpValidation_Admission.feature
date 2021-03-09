#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Aufenthalte einer Datenmeldung


# Eine Admission enthält eine Person, die nicht in der Personenliste ist -> Fehler

#todo Abfrage auf andere Ortsliste als HKPV (noch nicht in der Definition)
Szenario: Es wurde ein ungültiger Ort angegeben.
    Angenommen die Eigenschaft 'postcode' von 'Person' ist auf '6900' gesetzt
    Und die Eigenschaft 'city' von 'Person' ist auf 'Dornbirn' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''6900 Dornbirn' ist kein gültiger Ort.'

