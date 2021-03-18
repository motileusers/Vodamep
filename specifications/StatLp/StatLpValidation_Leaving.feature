#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Entlassungen einer Datenmeldung


# Pflichtfelder
#Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
#    Angenommen die Eigenschaft '<Name>' von 'Leaving' ist nicht gesetzt
#    Dann enthält das escapte Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'
#Beispiele:
#    | Name           | Bezeichnung |
#    | leaving_reason | Abgangart   | 



# Textfelder: falsche Zeichen
# RegEx @"^[-,.a-zA-ZäöüÄÖÜß\(\) ][-,.a-zA-ZäöüÄÖÜß\(\) ]*[-,.a-zA-ZäöüÄÖÜß\(\) ]$"
#Szenariogrundriss: Die Textfelder enthalten ungültige Werte
#    Angenommen die Eigenschaft '<Name>' von 'Leaving' enhält den Wert '<Wert>'
#    Dann enthält das Validierungsergebnis den Fehler 'Ungültiger Wert' für '<Bezeichnung>' bei Aufnamhe vom xx von Klient xxx
#Beispiele:
#    | Name                     | Bezeichnung                                | Wert |
#    | discharge_location_other | Sonstige Lebens-/Betreuungssituation       | =    |
#    | discharge_location_other | Sonstige Lebens-/Betreuungssituation       | 0    |
#    | discharge_reason_other   | Entlassung Grund                           | =    |
#    | discharge_reason_other   | Entlassung Grund                           | 0    |


# Textfelder: zu viele Zeichen
# 0 bis 30 Zeichen
#Szenariogrundriss: Die Textfelder enthalten zu lange Werte
#    Angenommen die Eigenschaft '<Name>' von 'Leaving' enhält den Wert '<Wert>'
#    Dann enthält das Validierungsergebnis den Fehler 'Zu langer Text' für '<Bezeichnung>' bei Aufnamhe vom xx von Klient xxx
#Beispiele:
#    | Name                     | Bezeichnung                          | Wert                               |
#    | discharge_location_other | Sonstige Lebens-/Betreuungssituation | abcdefghij abcdefghij abcdefghij x |
#    | discharge_location_other | Sonstige Lebens-/Betreuungssituation | abcdefghij abcdefghij abcdefghij x |
#    | discharge_reason_other   | Entlassung Grund                     | abcdefghij abcdefghij abcdefghij x |
#    | discharge_reason_other   | Entlassung Grund                     | abcdefghij abcdefghij abcdefghij x |


# Textfelder: alles gut
#Szenariogrundriss: Die Textfelder enthalten gültige Werte
#    Angenommen die Eigenschaft '<Name>' von 'Leaving' enhält den Wert '<Wert>'
#    Dann enthält das Validierungsergebnis keinen Fehler
#Beispiele:
#    | Name                  | Bezeichnung                                | Wert                             |
#    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | abcdefghij abcdefghij abcdefghij |
#    | personal_change_other | Veränderungen persönliche Situation        | abcdefghij abcdefghij abcdefghij |
#    | social_change_other   | Veränderungen nicht bewältigt, weil        | abcdefghij abcdefghij abcdefghij |
#    | housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | abcdefghij abcdefghij abcdefghij | 