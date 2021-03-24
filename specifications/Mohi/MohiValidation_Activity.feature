﻿##language: de-DE
#Funktionalität: Mohi - Validierung der Leistungen
#
## Werte < 0 und >= 10.000
## Nur in 0.25 Schritten
## Connexia Source NumberValidationUtility.ValidateDecimalPlace
#Szenariogrundriss: Gültige Werte bei der Leistung
#	Angenommen die Eigenschaft '<Name>' von 'activities' '<Nummer>' ist auf '<Wert>' gesetzt
#	Dann enthält das Validierungsergebnis den Fehler '<Fehler>'
#
#	Beispiele:
#		| Name            | Nummer | Wert | Fehler                                                                      |
#		| hours_per_month | 1      | 0    | Bei der Leistung von Person x wurde ein Wert < 0.25 eingegeben.             |
#		| hours_per_month | 1      | -1   | Bei der Leistung von Person x wurde kein Wert von > 10.000 eingegeben.      |
#		| hours_per_month | 1      | 0.1  | Bei der Leistung von Person x wurde kein Wert in 0.25 Schritten eingegeben. |
#
#Szenariogrundriss: Ungültige Werte bei der Leistung
#	Angenommen die Eigenschaft '<Name>' von 'activities' '<Nummer>' ist auf '<Wert>' gesetzt
#	Dann enthält das Validierungsergebnis keinen Fehler
#
#	Beispiele:
#		| Name            | Nummer | Wert | Fehler |
#		| hours_per_month | 1      | 1000 |        |
#		| hours_per_month | 1      | 0.5  |        |
#		| hours_per_month | 1      | 0.25 |        |
#
#Szenariogrundriss: Gültiger Klient bei der Leistung
#	Angenommen die Eigenschaft '<Name>' von 'activities' '<Nummer>' ist auf '<Wert>' gesetzt
#	Dann enthält das Validierungsergebnis keinen Fehler
#
#	Beispiele:
#		| Name      | Nummer | Wert | Fehler |
#		| person_id | 1      | 1    |        |
#
#Szenariogrundriss: Ungültiger Klient bei der Leistung
#	Angenommen die Eigenschaft '<Name>' von 'activities' '<Nummer>' ist auf '<Wert>' gesetzt
#	Dann enthält das Validierungsergebnis keinen Fehler
#
#	Beispiele:
#		| Name      | Nummer | Wert | Fehler                                          |
#		| person_id | -1     | 1    | FKlient xx wurden keine Personendaten gesendet. |