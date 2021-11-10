#language: de-DE
Funktionalität: Mkkp - Validierung der gemeldeten Mitarbeiterinnen Datenmeldung

Szenario: StaffId ist nicht eindeutig.
    Angenommen der Id einer Mitarbeiterin ist nicht eindeutig
    Dann enthält das Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von '<Art>' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Mitarbeiter '<Staff>' darf nicht leer sein.'

Beispiele:
    | Name          | Bezeichnung         | Art          | Staff        |   
    | id            | ID                  | Staff        | Peter Gruber |
    | family_name   | Familienname        | Staff        | 1            |
    | given_name    | Vorname             | Staff        | 1            |

Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
    Angenommen die Eigenschaft '<Name>' von '<Art>' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: ''<Bezeichnung>' von Mitarbeiter '<FullName>' weist ein ungültiges Format auf.'
Beispiele: 
    | Name        | Bezeichnung  | Art   | Wert | FullName      |    
    | family_name | Familienname | Staff | t@st | Peter t@st    |
    | given_name  | Vorname      | Staff | abc% | abc% Gruber   |


