#language: de-DE
Funktionalität: Agp - Validierung der gemeldeten Personen einer Datenmeldung

Szenario: Es wurde ein ungültiger Ort angegeben.
    Angenommen die Eigenschaft 'postcode' von 'Person' ist auf '6900' gesetzt
    Und die Eigenschaft 'city' von 'Person' ist auf 'Dornbirn' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''6900 Dornbirn' ist kein gültiger Ort.'

Szenario: Das Geburtsdatum darf nicht in der Zukunft liegen.
    Angenommen die Eigenschaft 'birthday' von 'Person' ist auf '2058-04-30' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Geburtsdatum' darf nicht in der Zukunft liegen.'

#todo: >30 Jahre <heute
Szenario: Das Geburtsdatum darf nicht vor 1900 liegen.
    Angenommen die Eigenschaft 'birthday' von 'Person' ist auf '1899-12-31' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Geburtsdatum' muss grösser oder gleich .*'

Szenario: PersonId ist nicht eindeutig.
    Angenommen der Id einer Person ist nicht eindeutig
    Dann enthält das Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Person' ist nicht gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name        | Bezeichnung           |
    | family_name | Geburtsdatum          |
    | given_name  | Versicherung          |
    | birthday    | Geburtsdatum          |
    | insurance   | Versicherung          |
    | postcode    | Plz                   |
    | city        | Ort                   |
    | gender      | Geschlecht            |    

Szenariogrundriss: Eine Eigenschaft vom AgpReport mit einem ungültigen Wert gesetzt.
    Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Für '<Bezeichnung>' ist '<Wert>' kein gültiger Code.'
Beispiele: 
    | Name        | Bezeichnung         | Wert |
    | insurance   | Versicherung        | test |

Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
    Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' weist ein ungültiges Format auf.'
Beispiele: 
    | Name        | Bezeichnung   | Wert |
    | family_name | Familienname  | t@st |
    | given_name  | Vorname       | t@st |

	
Szenariogrundriss: Der Name einer Person enthält ein spezielles, aber gültiges Zeichen
    Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
Beispiele: 
    | Name        | Bezeichnung   | Wert |
    | family_name | Familienname  | Chloé |
    | given_name  | Vorname       | Raphaël |
    | given_name  | Vorname       | Sr. Anna |


Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen die Datums-Eigenschaft '<Name>' von 'Person' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name     | Bezeichnung  |
    | birthday | Geburtsdatum |
    
