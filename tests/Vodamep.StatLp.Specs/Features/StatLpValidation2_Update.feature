#language: de-DE
Funktionalität: StatLp - Validierung der Aktualisierung einer Meldung

#todo: besser lesbar machen, den ursprünglichen Report ebenfalls bearbeitbar machen
#derzeit Careallowanc von StatLp-Datenmeldungen sind L3.
Szenario: Die Pflegestufe eine Person vermindert sich deutlich
Angenommen es gibt eine aktualisierte StatLp-Datenmeldungen
Und das Attribut mit dem  Typ 'CareAllowance' ist auf den Wert 'L7' gesetzt
Dann enthält das Validierungsergebnis die Warnung 'Die Pflegestufe von '(.*)' hat sich deutlich (.*) verändert.'