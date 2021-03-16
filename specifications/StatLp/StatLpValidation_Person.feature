#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Personen einer Datenmeldung

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
    Dann enthält das escapte Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name                  | Bezeichnung                                   |
    | family_name           | Familienname                                  |
    | given_name            | Vorname                                       |
    | birthday              | Geburtsdatum                                  |
    | gender                | Geschlecht                                    |    
    | country               | Land                                          |



# die Liste enthält eine Person, die nicht in mindestens einem stay ist --> fehler








#here

Szenariogrundriss: Eine Eigenschaft vom Report mit einem ungültigen Wert gesetzt.
    Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis genau einen Fehler
    Und die Fehlermeldung lautet: 'Für '<Bezeichnung>' ist '<Wert>' kein gültiger Code.'
Beispiele: 
    | Name        | Bezeichnung         | Wert |
    | insurance   | Versicherung        | test |

Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
    Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das escapte Validierungsergebnis den Fehler ''<Bezeichnung>' weist ein ungültiges Format auf.'
Beispiele: 
    | Name              | Bezeichnung                                   | Wert |
    | family_name       | Familienname                                  | t@st |
    | given_name        | Vorname                                       | t@st |
    | hospital_doctor   | Betreuender Arzt (Krankenhaus)                | t@st |
    | local_doctor      | Betreuender Arzt (Niedergelassener Bereich)   | t@st |

	
Szenariogrundriss: Der Name einer Person enthält ein spezielles, aber gültiges Zeichen
    Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
Beispiele: 
    | Name              | Bezeichnung                                   | Wert              |
    | family_name       | Familienname                                  | Chloé             |
    | given_name        | Vorname                                       | Raphaël           |
    | given_name        | Vorname                                       | Sr. Anna          |
    | hospital_doctor   | Betreuender Arzt (Krankenhaus)                | Dr. Frank         |
    | local_doctor      | Betreuender Arzt (Niedergelassener Bereich)   | Dr. Dr. Frank     |


Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
    Angenommen die Datums-Eigenschaft '<Name>' von 'Person' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
    | Name     | Bezeichnung  |
    | birthday | Geburtsdatum |
   
Szenario: Der Zuweiser ist 'Anderer', dann muss 'Anderer Zuweiser' befüllt sein
    Angenommen die Eigenschaft 'referrer' von 'Person' ist auf 'OtherReferrer' gesetzt
    Und die Eigenschaft 'other_referrer' von 'Person' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Wenn der Zuweiser ein Anderer Zuweiser ist, dann muss Anderer Zuweiser gesetzt sein.'

Szenario: Zuweiser ist undefiniert
    Angenommen die Eigenschaft 'referrer' von 'Person' ist auf 'UndefinedReferrer' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Zuweiser' darf nicht leer sein.'

Szenario: Geschlecht ist undefiniert
    Angenommen die Eigenschaft 'gender' von 'Person' ist auf 'UndefinedGender' gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Geschlecht' darf nicht leer sein.'

Szenario: Pflegegeld ist undefiniert
    Angenommen die Eigenschaft 'care_allowance' von 'Person' ist auf 'UndefinedAllowance' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Pflegegeld' darf nicht leer sein.'

Szenario: Es dürfen keine doppelten Diagnosegruppen vorhanden sein.
    Angenommen die Diagnose(n) ist auf 'GeneticDisease, GeneticDisease' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Es dürfen keine doppelten Diagnosegruppen vorhanden sein.'

Szenario: Es muss mindestens eine Diagnosegruppe vorhanden sein
    Angenommen die Diagnose(n) ist auf '' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Es muss mindestens eine Diagnosegruppe vorhanden sein.'

Szenario: Es muss mindestens eine Diagnosegruppe vorhanden sein 2
    Angenommen die Diagnose(n) ist auf 'UndefinedDiagnosisGroup' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Es muss mindestens eine Diagnosegruppe vorhanden sein.'

Szenariogrundriss: Es darf nur eine Pallativ Diagnosegruppe vorhanden sein
    Angenommen die Diagnose(n) ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Es darf nur eine Palliativ Diagnose Gruppe vorhanden sein.'
Beispiele: 
    | Wert                              |
    | PalliativeCare1, PalliativeCare2  |
    | PalliativeCare1, PalliativeCare3  |
    | PalliativeCare1, PalliativeCare4  |
    | PalliativeCare2, PalliativeCare3  |
    | PalliativeCare2, PalliativeCare4  |
    | PalliativeCare3, PalliativeCare4  |  
    
Szenariogrundriss: Es dürfen mehrere Diagnosegruppen kombiniert sein
    Angenommen die Diagnose(n) ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
 Beispiele: 
    | Wert                              |
    | OncologicalDisease, Premature, MetabolicDisease, NeurologicalDisease, SurgicalCare, HeartDisease, GeneticDisease, PalliativeCare1  |
    | OncologicalDisease, Premature, MetabolicDisease, NeurologicalDisease, SurgicalCare, HeartDisease, GeneticDisease, PalliativeCare2  |
    | OncologicalDisease, Premature, MetabolicDisease, NeurologicalDisease, SurgicalCare, HeartDisease, GeneticDisease, PalliativeCare3  |
    | OncologicalDisease, Premature, MetabolicDisease, NeurologicalDisease, SurgicalCare, HeartDisease, GeneticDisease, PalliativeCare4  |