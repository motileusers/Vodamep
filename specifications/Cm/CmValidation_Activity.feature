#language: de-DE
Funktionalität: Cm - Validierung der Leistungen
# case management id

# Werte > 0 und <= 10.000
# Connexia Source NumberValidationUtility.ValidateDecimalPlace
#Szenariogrundriss: Ungültige Werte bei der Leistung
#	Angenommen die Eigenschaft '<Name>' von 'Activity' ist auf '<Wert>' gesetzt
#	Dann enthält das Validierungsergebnis den Fehler '<Fehler>'
#
#	Beispiele:
#		| Name          | Nummer | Wert         | Fehler                                                                                         |
#		| minutes       | 1      | 0            | Bei der Leistung von Person '1' am xx wurde ein Wert < 1 angegeben.                              |
#		| minutes       | 1      | 10001        | Bei der Leistung von Person '1' am xx wurde kein Wert von > 10.000 angegeben.                    |
#		| activity_type | 1      | UNDEFINED_CA | Bei der Leistung von Person '1' am xx wurde keine Kategorie angegeben.                           |
#		| date          | 1      | 2058-04-30   | Bei der Leistung von Person '1' am xx wurde ein Datum außerhalb des Meldungszeitraums angegeben. |
# es ist keine person Id verfügbar.
	
# Werte > 0 und <= 10.000
# Connexia Source NumberValidationUtility.ValidateDecimalPlace
Szenariogrundriss: Ungültige Werte bei der Leistung
	Angenommen die Eigenschaft '<Name>' von 'ClientActivity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler '<Fehler>'

	Beispiele:
		| Name          | Nummer | Wert         | Fehler																									|
		| minutes       | 1      | 0            | Bei der Leistung von Person '1' am 02.02.2021 wurde ein Wert < 0.25 angegeben.							|
		| minutes       | 1      | 10001        | Bei der Leistung von Person '1' am 02.02.2021 wurde ein Wert > 10000 angegeben.							|
		| activity_type | 1      | UndefinedCa	| Bei der Leistung von Person '1' am 02.02.2021 wurde keine Kategorie angegeben.							|
		| date          | 1      | 2058-04-30   | Bei der Leistung von Person '1' am 30.04.2058 wurde ein Datum außerhalb des Meldungszeitraums angegeben.	|
		| person_id     | 1      | 3            | Unbekannter Klient bei Leistung am 02.02.2021.																|

Szenariogrundriss: Gültige Werte bei der Leistung
	Angenommen die Eigenschaft '<Name>' von 'Activity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

	Beispiele:
		| Name    | Nummer | Wert | Fehler |
		| minutes | 1      | 1000 |        |

## Beim Generator am besten einfach 2 Leistungen anlegen und die 2. Leistung auf den 1. Klienten setzen
#Szenariogrundriss: Mehrfache Leistungen pro Klient pro Monat
#	Angenommen die Eigenschaft '<Name>' von 'activities' '<Nummer>' ist auf '<Wert>' gesetzt
#	Dann enthält das Validierungsergebnis keinen Fehler
#
#	Beispiele:
#		| Name    | Nummer | Wert | Fehler |
#		| minutes | 2      | 1    |        |
#		| minutes | 1      | 1    |        |
# es ist keine person Id verfügbar.


Szenariogrundriss: Gültiger Klient bei der Leistung
	Angenommen die Eigenschaft '<Name>' von 'ClientActivity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

	Beispiele:
		| Name      | Nummer | Wert | Fehler |
		| person_id | 1      | 1    |        |

#Szenariogrundriss: Ungültiger Klient bei der Leistung
#	Angenommen die Eigenschaft '<Name>' von 'client_activities' '<Nummer>' ist auf '<Wert>' gesetzt
#	Dann enthält das Validierungsergebnis keinen Fehler
#
#	Beispiele:
#		| Name      | Nummer | Wert | Fehler                                             |
#		| person_id | -1     | 1    | Für Klient xx wurden keine Personendaten gesendet. |
#verstehe ich nicht


## Todo: Case Manager wird derzeit noch nicht geprüft, weil er noch nicht im Model vorhanden ist