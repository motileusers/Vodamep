#language: de-DE
Funktionalität: Mohi - Validierung der Datenmeldung

Szenario: Korrekt befüllt
    Angenommen es ist ein 'MohiReport'
	Und eine Meldung ist korrekt befüllt
	Dann enthält das Validierungsergebnis keine Fehler
	Und enthält das Validierungsergebnis keine Warnungen

Szenario: Von-Datum muss der erste Tag des Monats sein.
    Angenommen es ist ein 'MohiReport'
	Und die Eigenschaft 'from' von 'MohiReport' ist auf '2018-04-04' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Von' muss der erste Tag des Monats sein.'

Szenario: Bis-Datum muss der letzte Tag des Monats sein.
    Angenommen es ist ein 'MohiReport'
	Und die Eigenschaft 'to' von 'MohiReport' ist auf '2018-04-04' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Bis' muss der letzte Tag des Monats sein.'

Szenario: Die Meldung muss genau einen Monat beinhalten.
    Angenommen es ist ein 'MohiReport'
	Und die Eigenschaft 'from' von 'MohiReport' ist auf '2018-03-01' gesetzt
	Und die Eigenschaft 'to' von 'MohiReport' ist auf '2018-04-30' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Die Meldung muss genau einen Monat beinhalten.'

Szenario: Die Meldung darf nicht die Zukunft betreffen.
    Angenommen es ist ein 'MohiReport'
	Und die Eigenschaft 'to' von 'MohiReport' ist auf '2058-04-30' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Bis' muss kleiner oder gleich .*'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen es ist ein 'MohiReport'
	Und die Eigenschaft '<Name>' von 'MohiReport' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'

	Beispiele:
		| Name        | Bezeichnung |
		| from        | Von         |
		| to          | Bis         |
		| institution | Einrichtung |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen es ist ein 'MohiReport'
	Und die Datums-Eigenschaft '<Name>' von 'MohiReport' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'

	Beispiele:
		| Name | Bezeichnung |
		| from | Von         |
		| to   | Bis         |

Szenariogrundriss: Das Datum einer Entlassung in einer Meldung muss im Gültigkeitsbereich der Meldungen liegen
    Angenommen es ist ein 'MohiReport'
	Und die Datums-Eigenschaft '<Name>' von 'MohiReport' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'

	Beispiele:
		| Name | Bezeichnung |
		| from | Von         |
		| to   | Bis         |

Szenariogrundriss: Listen sind leer
    Angenommen es ist ein 'MohiReport'
	Und die Liste '<Name>' ist leer
	Dann enthält das Validierungsergebnis keine Fehler
	Und enthält das Validierungsergebnis keine Warnungen

	Beispiele:
		| Name				  |
		| Person,Activity     |
		| Activity			  |