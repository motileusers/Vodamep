#language: de-DE
Funktionalität: Validierung der gemeldeten Beschäftigungen

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von '<Art>' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name				| Bezeichnung			| Art			    |    
    | from				| Von					| Employment        |
    | to				| Bis				    | Employment        |


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

