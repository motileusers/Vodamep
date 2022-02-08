#language: de-DE
Funktionalität: Agp - Validierung der gemeldeten Personen einer Datenmeldung

Szenario: Es wurde ein ungültiger Ort angegeben.
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'postcode' von 'Person' ist auf '6900' gesetzt
	Und die Eigenschaft 'city' von 'Person' ist auf 'Dornbirn' gesetzt
	Dann enthält das Validierungsergebnis genau einen Fehler
	Und die Fehlermeldung lautet: ''6900 Dornbirn' ist kein gültiger Ort.'

Szenario: Das Geburtsdatum darf nicht in der Zukunft liegen.
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'birthday' von 'Person' ist auf '2058-04-30' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Geburtsdatum' darf nicht in der Zukunft liegen.'

Szenario: Das Geburtsdatum darf nicht vor 1900 liegen.
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'birthday' von 'Person' ist auf '1899-12-31' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Geburtsdatum' muss grösser oder gleich .*'

Szenario: PersonId ist nicht eindeutig.
	Angenommen der Id einer AGP-Person ist nicht eindeutig
	Dann enthält das Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft '<Name>' von 'Person' ist nicht gesetzt
	Dann enthält das Validierungsergebnis genau einen Fehler
	Und die Fehlermeldung lautet: ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
	| Name     | Bezeichnung  |
	| birthday | Geburtsdatum |
	| postcode | Plz          |
	| city     | Ort          |
	| gender   | Geschlecht   |

Szenariogrundriss: Eine Eigenschaft ist mit einem ungültigen Wert gesetzt.
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis genau einen Fehler
	Und die Fehlermeldung lautet: 'Für '<Bezeichnung>' ist '<Wert>' kein gültiger Code.'
Beispiele:
	| Name      | Bezeichnung  | Wert |
	| insurance | Versicherung | test |

Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis genau einen Fehler
	Und die Fehlermeldung lautet: 'weist ein ungültiges Format auf'
Beispiele:
	| Name            | Bezeichnung                                 | Wert |
	| hospital_doctor | Betreuender Arzt (Krankenhaus)              | t@st |
	| local_doctor    | Betreuender Arzt (Niedergelassener Bereich) | t@st |
	
Szenariogrundriss: Der Name einer Person enthält ein spezielles, aber gültiges Zeichen
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
	| Name            | Bezeichnung                                 | Wert          |
	| hospital_doctor | Betreuender Arzt (Krankenhaus)              | Dr. Frank     |
	| local_doctor    | Betreuender Arzt (Niedergelassener Bereich) | Dr. Dr. Frank |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
	Angenommen es ist ein 'AgpReport'
	Und die Datums-Eigenschaft '<Name>' von 'Person' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
	| Name     | Bezeichnung  |
	| birthday | Geburtsdatum |

Szenario: Der Zuweiser ist 'Anderer', dann muss 'Anderer Zuweiser' befüllt sein
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'referrer' von 'Person' ist auf 'OtherReferrer' gesetzt
	Und die Eigenschaft 'other_referrer' von 'Person' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Wenn der Zuweiser von Klient '1' ein Anderer Zuweiser ist, dann muss Anderer Zuweiser gesetzt sein.'
    
Szenario: Geschlecht ist undefiniert
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'gender' von 'Person' ist auf 'UndefinedGe' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Geschlecht' darf nicht leer sein.'

Szenario: Pflegegeld ist undefiniert
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'care_allowance' von 'Person' ist auf 'UndefinedAllowance' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Pflegegeld' darf nicht leer sein.'

Szenario: Es dürfen keine doppelten Diagnosegruppen vorhanden sein.
	Angenommen die Agp-Diagnose(n) ist auf 'AffectiveDisorder, AffectiveDisorder' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Es dürfen keine doppelten Diagnosegruppen für Klient '1' vorhanden sein.'

Szenario: Es muss mindestens eine Diagnosegruppe vorhanden sein
	Angenommen die Agp-Diagnose(n) ist auf '' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Es muss mindestens eine Diagnosegruppe für Klient '1' vorhanden sein.'