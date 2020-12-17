#language: de-DE
Funktionalität: Mkkp - Validierung der gemeldeten Personen einer Datenmeldung

Szenario: Es wurde ein ungültiger Ort angegeben.
    Angenommen Mkkp: die Eigenschaft 'postcode' von 'Person' ist auf '6900' gesetzt
    Und Mkkp: die Eigenschaft 'city' von 'Person' ist auf 'Dornbirn' gesetzt
    Dann enthält das Mkkp Validierungsergebnis genau einen Fehler
    Und die Mkkp Fehlermeldung lautet: ''6900 Dornbirn' ist kein gültiger Ort.'

Szenario: Das Geburtsdatum darf nicht in der Zukunft liegen.
    Angenommen Mkkp: die Eigenschaft 'birthday' von 'Person' ist auf '2058-04-30' gesetzt
    Dann enthält das Mkkp Validierungsergebnis den Fehler ''Geburtsdatum' darf nicht in der Zukunft liegen.'

#todo: >30 Jahre <heute
Szenario: Das Geburtsdatum darf nicht vor 1900 liegen.
    Angenommen Mkkp: die Eigenschaft 'birthday' von 'Person' ist auf '1899-12-31' gesetzt
    Dann enthält das Mkkp Validierungsergebnis den Fehler 'Der Wert von 'Geburtsdatum' muss grösser oder gleich .*'

Szenario: PersonId ist nicht eindeutig.
    Angenommen Mkkp: der Id einer Person ist nicht eindeutig
    Dann enthält das Mkkp Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen Mkkp: die Eigenschaft '<Name>' von 'Person' ist nicht gesetzt
    Dann enthält das Mkkp Validierungsergebnis genau einen Fehler
    Und die Mkkp Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name                  | Bezeichnung                       |
    | family_name           | Geburtsdatum                      |
    | given_name            | Versicherung                      |
    | birthday              | Geburtsdatum                      |
    | insurance             | Versicherung                      |
    | postcode              | Plz                               |
    | city                  | Ort                               |
    | gender                | Geschlecht                        |    
    | referrer              | Zuweiser                          |
    | hospital_doctor       | Arzt Krankenhaus                  |
    | local_doctor          | Arzt Niedergelassener Bereich     |


Szenariogrundriss: Eine Eigenschaft vom AgpReport mit einem ungültigen Wert gesetzt.
    Angenommen Mkkp: die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Mkkp Validierungsergebnis genau einen Fehler
    Und die Mkkp Fehlermeldung lautet: 'Für '<Bezeichnung>' ist '<Wert>' kein gültiger Code.'
Beispiele: 
    | Name        | Bezeichnung         | Wert |
    | insurance   | Versicherung        | test |

Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
    Angenommen Mkkp: die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Mkkp Validierungsergebnis genau einen Fehler
    Und die Mkkp Fehlermeldung lautet: ''<Bezeichnung>' weist ein ungültiges Format auf.'
Beispiele: 
    | Name              | Bezeichnung                   | Wert |
    | family_name       | Familienname                  | t@st |
    | given_name        | Vorname                       | t@st |
    | hospital_doctor   | Arzt Krankenhaus              | t@st |
    | local_doctor      | Arzt Niedergelassener Bereich | t@st |

	
Szenariogrundriss: Der Name einer Person enthält ein spezielles, aber gültiges Zeichen
    Angenommen Mkkp: die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Mkkp Validierungsergebnis keine Fehler
Beispiele: 
    | Name              | Bezeichnung                   | Wert              |
    | family_name       | Familienname                  | Chloé             |
    | given_name        | Vorname                       | Raphaël           |
    | given_name        | Vorname                       | Sr. Anna          |
    | hospital_doctor   | Arzt Krankenhaus              | Dr. Frank         |
    | local_doctor      | Arzt Niedergelassener Bereich | Dr. Dr. Frank     |


Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen Mkkp: die Datums-Eigenschaft '<Name>' von 'Person' hat eine Uhrzeit gesetzt
    Dann enthält das Mkkp Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name     | Bezeichnung  |
    | birthday | Geburtsdatum |




#todo: Referrer muss aus der Liste der referrers kommen
#todo: wenn referer = Other, dann muss other_referrer befüllt sein
    
#todo: Alle Diagnosegruppen aus dem csv müssen als enum vorhanden sein (Normaler Test, ohne Specflow?)
#todo: und umgekehrt


#todo: keine doppelten Diagnosegruppen
#todo: mindestens 1 Diagnosegruppen vorhanden