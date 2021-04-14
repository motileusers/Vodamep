#language: de-DE
Funktionalität: Cm - Validierung der gemeldeten Personen einer Datenmeldung

Szenario: Das Geburtsdatum darf nicht in der Zukunft liegen.
	Angenommen die Eigenschaft 'birthday' von 'Person' ist auf '2058-04-30' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Geburtsdatum' von Klient '1' darf nicht in der Zukunft liegen.'

#todo: > 1890, 1, 1 <heute
Szenario: Das Geburtsdatum darf nicht vor 1890 liegen.
	Angenommen die Eigenschaft 'birthday' von 'Person' ist auf '1889-12-31' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Der Wert von 'Geburtsdatum' von Klient '1' muss grösser oder gleich .*'

Szenario: PersonId ist nicht eindeutig.
	Angenommen der Id einer Person ist nicht eindeutig
	Dann enthält das Validierungsergebnis den Fehler 'Der Id ist nicht eindeutig.'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
	Angenommen die Eigenschaft '<Name>' von 'Person' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '1' darf nicht leer sein.'

	Beispiele:
		| Name           | Bezeichnung  |
		| family_name    | Familienname |
		| given_name     | Vorname      |
		| birthday       | Geburtsdatum |
		| gender         | Geschlecht   |
		| country        | Land         |
		| postcode       | PLZ          |
		| city           | Ort          |
		| care_allowance | Pflegegeld   |

# Regex "^[a-zA-ZäöüÄÖÜß][-a-zA-ZäöüÄÖÜß ]*?[a-zA-ZäöüÄÖÜß]$"
Szenariogrundriss: Der Name einer Person enthält ein ungültiges Zeichen
	Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '1' weist ein ungültiges Format auf.'

	Beispiele:
		| Name        | Bezeichnung  | Wert |
		| family_name | Familienname | t@st |
		| given_name  | Vorname      | t@st |

# Länge 2 - 50 Zeichen
Szenariogrundriss: Der Familienname einer Person ist zu kurz / lang
	Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '1' besitzt eine ungültige Länge'

	Beispiele:
		| Name        | Bezeichnung  | Wert                                                     |
		| family_name | Familienname | abcdefghij abcdefghij abcdefghij abcdefghij abcdefghij x |
		| family_name | Familienname | x                                                        |

# Länge 2 - 30 Zeichen
Szenariogrundriss: Der Vorname einer Person ist zu kurz / lang
	Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '1' besitzt eine ungültige Länge.'

	Beispiele:
		| Name       | Bezeichnung | Wert                               |
		| given_name | Vorname     | abcdefghij abcdefghij abcdefghij x |
		| given_name | Vorname     | x                                  |
#
# Land
Szenariogrundriss: Das Land einer Person enthält einen ungültigen Wert
	Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '1' hat einen ungülitgen Wert'

	Beispiele:
		| Name    | Bezeichnung | Wert |
		| country | Land        | A    |
		| country | Land        | ZZ   |

Szenariogrundriss: Das Land einer Person enthält einen gültigen Wert
	Angenommen die Eigenschaft '<Name>' von 'Person' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

	Beispiele:
		| Name    | Bezeichnung | Wert |
		| country | Land        | AD   |
		| country | Land        | AT   |

Szenariogrundriss: Die Datumsfelder dürfen keine Zeit enthalten
	Angenommen die Datums-Eigenschaft '<Name>' von 'Person' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von Klient '1' darf keine Uhrzeit beinhalten.'

	Beispiele:
		| Name     | Bezeichnung  |
		| birthday | Geburtsdatum |