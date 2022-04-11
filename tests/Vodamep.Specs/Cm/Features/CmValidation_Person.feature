#language: de-DE
Funktionalität: Cm - Validierung der gemeldeten Personen einer Datenmeldung

Szenario: Das Geburtsdatum darf nicht in der Zukunft liegen.
	Angenommen es ist ein 'CmReport'
	Und die Eigenschaft 'birthday' von 'Person' ist auf '2058-04-30' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Geburtsdatum' von Klient 'Wolfgang Huber' darf nicht in der Zukunft liegen.'

#todo: > 1890, 1, 1 <heute
Szenario: Das Geburtsdatum darf nicht vor 1890 liegen.
	Angenommen es ist ein 'CmReport'
	Und die Eigenschaft 'birthday' von 'Person' ist auf '1889-12-31' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Geburtsdatum' von Klient 'Wolfgang Huber' muss grösser oder gleich .*'

Szenario: PersonId ist nicht eindeutig.
	Angenommen der Id einer Cm-Person ist nicht eindeutig
	Dann enthält das Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
	Angenommen es ist ein 'CmReport'
	Und die Eigenschaft '<Name>' von 'Person' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '<NewName>' darf nicht leer sein.'

Beispiele:
	| Name           | Bezeichnung        | NewName        |
	| family_name    | Familienname       | Wolfgang       |
	| given_name     | Vorname            | Huber          |
	| birthday       | Geburtsdatum       | Wolfgang Huber |
	| gender         | Geschlecht         | Wolfgang Huber |
	| nationality    | Staatsbürgerschaft | Wolfgang Huber |
	| postcode       | PLZ                | Wolfgang Huber |
	| city           | Ort                | Wolfgang Huber |
	| care_allowance | Pflegegeld         | Wolfgang Huber |

# Regex "^[a-zA-ZäöüÄÖÜß][-a-zA-ZäöüÄÖÜß ]*?[a-zA-ZäöüÄÖÜß]$"
Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
	Angenommen es ist ein 'CmReport'
	Und die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '(.*)' weist ein ungültiges Format auf.'

Beispiele:
	| Name        | Bezeichnung  | Wert |
	| family_name | Familienname | t@st |
	| given_name  | Vorname      | t@st |

# Länge 2 - 50 Zeichen
Szenariogrundriss: Der Familienname einer Person ist zu kurz / lang
	Angenommen es ist ein 'CmReport'
	Und die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '(.*)' besitzt eine ungültige Länge'

Beispiele:
	| Name        | Bezeichnung  | Wert                                                     |
	| family_name | Familienname | abcdefghij abcdefghij abcdefghij abcdefghij abcdefghij x |
	| family_name | Familienname | x                                                        |

# Länge 2 - 30 Zeichen
Szenariogrundriss: Der Vorname einer Person ist zu kurz / lang
	Angenommen es ist ein 'CmReport'
	Und die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '(.*)' besitzt eine ungültige Länge.'

Beispiele:
	| Name       | Bezeichnung | Wert                               |
	| given_name | Vorname     | abcdefghij abcdefghij abcdefghij x |
	| given_name | Vorname     | x                                  |
#
# Land
Szenariogrundriss: Das Land einer Person enthält einen ungültigen Wert
	Angenommen es ist ein 'CmReport'
	Und die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient 'Wolfgang Huber' hat einen ungültigen Wert'

Beispiele:
	| Name        | Bezeichnung        | Wert |
	| nationality | Staatsbürgerschaft | A    |

Szenariogrundriss: Das Land einer Person enthält einen gültigen Wert
	Angenommen es ist ein 'CmReport'
	Und die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Beispiele:
	| Name        | Bezeichnung        | Wert |
	| nationality | Staatsbürgerschaft | AD   |
	| nationality | Staatsbürgerschaft | AT   |
	| nationality | Staatsbürgerschaft | ZZ   |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
	Angenommen es ist ein 'CmReport'
	Und die Datums-Eigenschaft '<Name>' von 'Person' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient 'Wolfgang Huber' darf keine Uhrzeit beinhalten.'

Beispiele:
	| Name     | Bezeichnung  |
	| birthday | Geburtsdatum |