#language: de-DE
Funktionalität: Validierung der gemeldeten Beschäftigungen

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Employment' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name				| Bezeichnung			|  
    | from				| Von					| 
    | to				| Bis				    | 


Szenariogrundriss: Die Stundenanzahl ist zu hoch oder zu niedrig.
    Angenommen die Eigenschaft 'hours_per_week' von 'Employment' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Die Stundenanzahl muss größer 0 und kleiner 100 sein'
Beispiele:
    | Wert	| 
    | 0		| 
    | 100	| 

Szenario: Stundenanzahl ist korrekt
    Angenommen die Eigenschaft 'hours_per_week' von 'Employment' ist auf '25' gesetzt
    Dann enthält das Validierungsergebnis nicht den Fehler 'Die Stundenanzahl muss größer 0 und kleiner 100 sein''


Szenariogrundriss: Anstellungszeitraum ist falsch
    Angenommen die Eigenschaft 'from' von 'Employment' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Das Anstellungsverhältniss von '
Beispiele:
    | Wert			| 
    | 2018-03-01	| 


Szenariogrundriss: Falsche Anstellungsverhältnisse
    Angenommen die Meldung enthält die Anstellungen '<Anstellungen>' und die Leistungstage '<Leistungstage>'
    Dann enthält das Validierungsergebnis den Fehler 'überschneiden sich'
Beispiele:
    | Anstellungen  | Leistungstage | 
    | 1-2,2-28		| 1				| 

Szenariogrundriss: Richtige Anstellungsverhältnisse
    Angenommen die Meldung enthält die Anstellungen '<Anstellungen>' und die Leistungstage '<Leistungstage>'
    Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
    | Anstellungen  | Leistungstage | 
    | 1-2,5-28		| 1,5			| 

Szenariogrundriss: Leistungen nicht im Anstellungsverhältnis
    Angenommen die Meldung enthält die Anstellungen '<Anstellungen>' und die Leistungstage '<Leistungstage>'
    Dann enthält das Validierungsergebnis den Fehler 'liegt nicht im Anstellungszeitraum'
Beispiele:
    | Anstellungen  | Leistungstage | 
    | 1-2,5-28		| 1,4			| 

