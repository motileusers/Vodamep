#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Entlassungen einer Datenmeldung

# Pflichtfelder
#Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
#    Angenommen die Eigenschaft '<Name>' von 'Leaving' ist nicht gesetzt
#    Dann enthält das escapte Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'
#Beispiele:
#    | Name           | Bezeichnung |
#    | leaving_reason | Abgangart   | 

# Feldabhängigkeiten
# leaving_reason muss immer befüllt sein
# - bei Tod muss die death-Spalte befüllt sein
# - bei Entlassung müssen die discharge-Spalten befüllt sein
# Texte ..._other
# - entweder Wert aus der Liste (discharge_location) oder alternativer Text zu diesem Feld (discharge_location_other)
# - wenn alterativer Text angegeben wird, muss Other (z.B. OTHER_DR) in der Liste ausgewählt werden
#Szenariogrundriss: Abhängigkeiten von Sterbefall / Entlassung
#	Angenommen die Eigenschaft '<leaving_reason>' von 'Leaving' ist auf '<leaving_reason_wert>' gesetzt
#	Angenommen die Eigenschaft '<death_location>' von 'Leaving' ist auf '<death_location_wert>' gesetzt
#	Angenommen die Eigenschaft '<discharge_location>' von 'Leaving' ist auf '<discharge_location_wert>' gesetzt
#	Angenommen die Eigenschaft '<discharge_location_other>' von 'Leaving' ist auf '<discharge_location_other_wert>' gesetzt
#	Angenommen die Eigenschaft '<discharge_reason>' von 'Leaving' ist auf '<discharge_reason_wert>' gesetzt
#	Angenommen die Eigenschaft '<discharge_reason_other>' von 'Leaving' ist auf '<discharge_reason_other_wert>' gesetzt
#	Dann enthält das das Validierungsergebnis keinen Fehler
#
#	Beispiele:
#		| leaving_reason | death_location        | discharge_location | discharge_location_other | discharge_reason    | discharge_reason_other | fehler |
#		| DECEASED_LR    | DEATH_NURSING_HOME_DL |                    |                          |                     |                        |        |
#		| DISCHARGE_LR   |                       | HOME_LIVING_ALONE  |                          | END_SHORT_TERM_CARE |                        |        |
#		| DISCHARGE_LR   |                       | OTHER_DL           | asd                      | END_SHORT_TERM_CARE |                        |        |
#		| DISCHARGE_LR   |                       | HOME_LIVING_ALONE  |                          |                     | asdf                   |        |
#		| DISCHARGE_LR   |                       | OTHER_DL           | asd                      | OTHER_DR            | asd                    |        |
#
#Szenariogrundriss: Abhängigkeiten von Sterbefall / Entlassung - Fehler
#	Angenommen die Eigenschaft '<leaving_reason>' von 'Leaving' ist auf '<leaving_reason_wert>' gesetzt
#	Angenommen die Eigenschaft '<death_location>' von 'Leaving' ist auf '<death_location_wert>' gesetzt
#	Angenommen die Eigenschaft '<discharge_location>' von 'Leaving' ist auf '<discharge_location_wert>' gesetzt
#	Angenommen die Eigenschaft '<discharge_location_other>' von 'Leaving' ist auf '<discharge_location_other_wert>' gesetzt
#	Angenommen die Eigenschaft '<discharge_reason>' von 'Leaving' ist auf '<discharge_reason_wert>' gesetzt
#	Angenommen die Eigenschaft '<discharge_reason_other>' von 'Leaving' ist auf '<discharge_reason_other_wert>' gesetzt
#	Dann enthält das das Validierungsergebnis den Fehler '<fehler>'
#
#	Beispiele:
#		| leaving_reason | death_location        | discharge_location | discharge_location_other | discharge_reason    | discharge_reason_other | fehler                                                                                                    |
#		|                |                       |                    |                          |                     |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                | DEATH_NURSING_HOME_DL |                    |                          |                     |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                |                       | HOME_LIVING_ALONE  |                          |                     |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                |                       |                    |                          | END_SHORT_TERM_CARE |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                |                       |                    | abc                      |                     |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                |                       |                    |                          |                     | abc                    | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		| DECEASED_LR    |                       |                    |                          |                     |                        | Wenn der Klient xx gestorben ist, muss eine Angabe zum Sterbeort gemacht werden.'                         |
#		| DECEASED_LR    | DEATH_NURSING_HOME_DL | HOME_LIVING_ALONE  |                          |                     |                        | Wenn der Klient xx gestorben ist, darf keine Angabe zur Entlassung gemacht werden.'                       |
#		| DECEASED_LR    | DEATH_NURSING_HOME_DL |                    | abc                      |                     |                        | Wenn der Klient xx gestorben ist, darf keine Angabe zur Entlassung gemacht werden.'                       |
#		| DECEASED_LR    | DEATH_NURSING_HOME_DL |                    |                          | END_SHORT_TERM_CARE |                        | Wenn der Klient xx gestorben ist, darf keine Angabe zur Entlassung gemacht werden.'                       |
#		| DECEASED_LR    | DEATH_NURSING_HOME_DL |                    |                          |                     | asdf                   | Wenn der Klient xx gestorben ist, darf keine Angabe zur Entlassung gemacht werden.'                       |
#		| DISCHARGE_LR   |                       | NURSING_HOME       | asd                      | END_SHORT_TERM_CARE |                        | Wenn bei der Entlassung von Klient xx sonstige Angaben gemacht werden, muss 'Sonstige' ausgewählt werden. |
#		| DISCHARGE_LR   |                       | NURSING_HOME       |                          | END_SHORT_TERM_CARE | asdfasd                | Wenn bei der Entlassung von Klient xx sonstige Angaben gemacht werden, muss 'Sonstige' ausgewählt werden. |
#		| DISCHARGE_LR   |                       |                    |                          |                     |                        | Wenn der Klient xx entlassen worden ist, muss angegeben werden, wohin der Klient entlassen wurde.         |
#		| DISCHARGE_LR   |                       | HOME_LIVING_ALONE  |                          |                     |                        | Wenn der Klient xx entlassen worden ist, muss angegeben werden, warum der Klient entlassen wurde.         |
#		| DISCHARGE_LR   | DEATH_NURSING_HOME_DL |                    |                          |                     |                        | Wenn der Klient xx entlassen worden ist, darf keine Angabe zum Sterbefall gemacht werden.'                |

# Textfelder: falsche Zeichen
# RegEx @"^[-,.a-zA-ZäöüÄÖÜß\(\) ][-,.a-zA-ZäöüÄÖÜß\(\) ]*[-,.a-zA-ZäöüÄÖÜß\(\) ]$"
#Szenariogrundriss: Die Textfelder enthalten ungültige Werte
#    Angenommen die Eigenschaft '<Name>' von 'Leaving' enhält den Wert '<Wert>'
#    Dann enthält das Validierungsergebnis den Fehler 'Ungültiger Wert' für '<Bezeichnung>' bei Aufnamhe vom xx von Klient xxx
#Beispiele:
#    | Name                     | Bezeichnung                                | Wert |
#		| Name                     | Bezeichnung                          | Wert |
#		| discharge_location_other | Sonstige Lebens-/Betreuungssituation | =    |
#		| discharge_location_other | Sonstige Lebens-/Betreuungssituation | 0    |
#		| discharge_reason_other   | Entlassung Grund                     | =    |
#		| discharge_reason_other   | Entlassung Grund                     | 0    |

# Textfelder: zu viele Zeichen
# 0 bis 30 Zeichen
#Szenariogrundriss: Die Textfelder enthalten zu lange Werte
#	Angenommen die Eigenschaft '<Name>' von 'Leaving' enhält den Wert '<Wert>'
#    Dann enthält das Validierungsergebnis den Fehler 'Zu langer Text' für '<Bezeichnung>' bei Aufnamhe vom xx von Klient xxx
#Beispiele:
#    | Name                     | Bezeichnung                          | Wert                               |
#    | discharge_location_other | Sonstige Lebens-/Betreuungssituation | abcdefghij abcdefghij abcdefghij x |
#    | discharge_location_other | Sonstige Lebens-/Betreuungssituation | abcdefghij abcdefghij abcdefghij x |
#    | discharge_reason_other   | Entlassung Grund                     | abcdefghij abcdefghij abcdefghij x |
#    | discharge_reason_other   | Entlassung Grund                     | abcdefghij abcdefghij abcdefghij x |
#
## Textfelder: alles gut
#Szenariogrundriss: Die Textfelder enthalten gültige Werte
#    Angenommen die Eigenschaft '<Name>' von 'Leaving' enhält den Wert '<Wert>'
#    Dann enthält das Validierungsergebnis keinen Fehler
#Beispiele:
#    | Name                  | Bezeichnung                                | Wert                             |
#    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | abcdefghij abcdefghij abcdefghij |
#    | personal_change_other | Veränderungen persönliche Situation        | abcdefghij abcdefghij abcdefghij |
#    | social_change_other   | Veränderungen nicht bewältigt, weil        | abcdefghij abcdefghij abcdefghij |
#    | housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | abcdefghij abcdefghij abcdefghij | 