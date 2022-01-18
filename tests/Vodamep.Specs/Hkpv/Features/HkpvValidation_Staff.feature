#language: de-DE
Funktionalität: Hkpv - Validierung der gemeldeten Mitarbeiterinnen Datenmeldung

Szenario: Qualifikation ist nicht gesetzt und das Jahr ist 2018.
    Angenommen es ist ein 'HkpvReport'
    Und die Eigenschaft 'to' von 'HkpvReport' ist auf '2018-12-31' gesetzt
	Und die Eigenschaft 'qualification' von 'Staff' ist nicht gesetzt
 	Dann enthält das Validierungsergebnis nicht den Fehler 'Qualifikation' darf nicht leer sein.'

Szenario: Qualifikation ist nicht gesetzt und das Jahr ist 2019.
    Angenommen es ist ein 'HkpvReport'
    Und die Eigenschaft 'to' von 'HkpvReport' ist auf '2019-01-01' gesetzt
	Und die Eigenschaft 'qualification' von 'Staff' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Qualifikation' darf nicht leer sein.'
	
Szenario: StaffId ist nicht eindeutig.
    Angenommen der Id einer Hkpv-Mitarbeiterin ist nicht eindeutig
    Dann enthält das Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen es ist ein 'HkpvReport'
    Und die Eigenschaft '<Name>' von '<Art>' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name          | Bezeichnung         | Art          |    
    | family_name   | Familienname        | Staff        |
    | given_name    | Vorname             | Staff        |
	| qualification | Qualifikation       | Staff        |

Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
    Angenommen es ist ein 'HkpvReport'
    Und die Eigenschaft '<Name>' von '<Art>' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' weist ein ungültiges Format auf.'
Beispiele: 
    | Name        | Bezeichnung         | Art    | Wert |    
    | family_name | Familienname        | Staff  | t@st |

Szenario: Mitarbeiter ohne Beschäftigung.
    Angenommen es ist keine Beschäftigung beim Mitarbeiter vorhanden
	Dann enthält das Validierungsergebnis den Fehler 'Beim Mitarbeiter ist keine Beschäftigung vorhanden'
   

