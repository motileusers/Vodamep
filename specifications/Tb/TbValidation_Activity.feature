#language: de-DE
Funktionalität: Tb - Validierung der Leistungen

# Werte > 0 und <= 10.000
# Nur in 0.25 Schritten
# Connexia Source NumberValidationUtility.ValidateDecimalPlace
Szenariogrundriss: Ungültige Werte bei der Leistung
	Angenommen die Eigenschaft '<Name>' von 'Activity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler '<Fehler>'
	Beispiele:
		| Name            | Nummer | Wert  | Fehler                                                                     |
		| hours_per_month | 1      | 0     | Bei der Leistung von Person '1' wurde ein Wert < 0.25 angegeben.           |
		| hours_per_month | 1      | 10001 | Bei der Leistung von Person '1' wurde ein Wert > 10000 angegeben.			|
		| hours_per_month | 1      | 0.375 | muss der Wert in 0.25 Schritten angegeben werden.							|

Szenariogrundriss: Gültige Werte bei der Leistung
	Angenommen die Eigenschaft '<Name>' von 'Activity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler
	Beispiele:
		| Name            | Nummer | Wert | Fehler |
		| hours_per_month | 1      | 1000 |        |
		| hours_per_month | 1      | 0.5  |        |
		| hours_per_month | 1      | 0.25 |        |

# Beim Generator am besten einfach 2 Leistungen anlegen und die 2. Leistung auf den 1. Klienten setzen
Szenariogrundriss: Mehrfache Leistungen pro Klient pro Monat
	Angenommen für einen Klient gibt es mehrfache Leistungen
	Dann enthält das Validierungsergebnis den Fehler 'Mehrfache Leistungen für Klient '1' vorhanden.'

	Beispiele:
		| Name      | Nummer | Wert |
		| person_id | 2      | 1    |

Szenariogrundriss: Gültiger Klient bei der Leistung
	Angenommen die Eigenschaft '<Name>' von 'Activity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler
	Beispiele:
		| Name      | Nummer | Wert | Fehler |
		| person_id | 1      | 1    |        |

Szenariogrundriss: Ungültiger Klient bei der Leistung
	Angenommen die Eigenschaft '<Name>' von 'Activity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler
	Beispiele:
		| Name      | Nummer | Wert | Fehler												|
		| person_id | -1     | 1    | Für Klient '-1' wurden keine Personendaten gesendet.	|