#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Aufenthalte einer Datenmeldung

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Admission' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name                          | Bezeichnung                                    |
    | housing_type_before_admission | Wohnsituation vor der Aufnahme                 |
    | main_attendance_relation      | Verwandtschaftsverhältnis Hauptbetreuungspers. |
    | main_attendance_closeness     | Räumliche Nähe Hauptbetreuungsperson           |
    | housing_reason                | Wohnraumsituations- und Ausstattungsgründe     | 

Szenariogrundriss: Die Textfelder enthalten ungültige Werte
    Angenommen die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Ungültiger Wert für '<Bezeichnung>' bei Aufnahme vom 01.02.2021 von Klient 1.'
Beispiele:
    | Name                  | Bezeichnung                                | Wert |
    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | =    |
    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | 0    |
    | personal_change_other | Veränderungen persönliche Situation        | =    |
    | personal_change_other | Veränderungen persönliche Situation        | 0    |
    | social_change_other   | Veränderungen nicht bewältigt, weil        | =    |
    | social_change_other   | Veränderungen nicht bewältigt, weil        | 0    |
    | housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | =    |
    | housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | 0    |

Szenariogrundriss: Die Textfelder enthalten zu lange Werte
    Angenommen die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Zu langer Text für '<Bezeichnung>' bei Aufnahme vom 01.02.2021 von Klient 1.'
Beispiele:
    | Name                  | Bezeichnung                                | Wert                               |
    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | abcdefghij abcdefghij abcdefghij x |
    | personal_change_other | Veränderungen persönliche Situation        | abcdefghij abcdefghij abcdefghij x |
    | social_change_other   | Veränderungen nicht bewältigt, weil        | abcdefghij abcdefghij abcdefghij x |
    | housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | abcdefghij abcdefghij abcdefghij x | 

Szenariogrundriss: Die Textfelder enthalten gültige Werte
    Angenommen die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
    | Name                  | Bezeichnung                                | Wert                           |
    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | abcdefghij abcdefghij abcdefgh |
    | personal_change_other | Veränderungen persönliche Situation        | abcdefghij abcdefghij abcdefgh |
    | social_change_other   | Veränderungen nicht bewältigt, weil        | abcdefghij abcdefghij abcdefgh |
    | housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | abcdefghij abcdefghij abcdefgh | 

Szenariogrundriss: Ungültiger Ort / Plz
    Angenommen die PLZ und der Ort von Admission sind auf auf '<Value1>' und '<Value2>' gesetzt
    Dann enthält das escapte Validierungsergebnis den Fehler 'Ungültige Kombination Ort/Plz bei Aufnahme vom 01.02.2021 von Klient 1.'
Beispiele:
    | Field1        | Field2    | Value1 | Value2     |
    | last_postcode | last_city | 0349   | Feldkirch  |
    | last_postcode | last_city | 0349   |            |
    | last_postcode | last_city |        | Feldkirch  |
    | last_postcode | last_city |        | Feldkirch  |
    | last_postcode | last_city |        |            |

Szenariogrundriss: Gültiger Ort / Plz
    Angenommen die PLZ und der Ort von Admission sind auf auf '<Value1>' und '<Value2>' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
    | Field1        | Field2    | Value1 | Value2    |
    | last_postcode | last_city | 6800   | Feldkirch |
    | last_postcode | last_city | 0000   | Anderer   |

Szenariogrundriss: Die Auflistung enthalten ungültige Werte
   Angenommen die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '<Name>' ist auf '<Wert1>' gesetzt
   Und die Eigenschaft '<Name2>' von 'Admission' ist auf '<Wert2>' gesetzt
   Dann enthält das Validierungsergebnis den Fehler 'Keine gültige Angabe bei '<Bezeichnung>' 
Beispiele:
    | Name            | Bezeichnung                                | Wert1                          | Name2                   | Wert2      |
    | PersonalChanges | Veränderungen persönliche Situation        |                                | personal_change_other   |            |
    | PersonalChanges | Veränderungen persönliche Situation        | UndefinedPc                    | personal_change_other   |            |
    | PersonalChanges | Veränderungen persönliche Situation        | UndefinedPc                    | personal_change_other   | abcdefghij |
    | PersonalChanges | Veränderungen persönliche Situation        | OwnDesirePc, UndefinedPc       | personal_change_other   | abcdefghij |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        |                                | social_change_other     |            |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        | UndefinedSc                    | social_change_other     |            |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        | UndefinedSc                    | social_change_other     | abcdefghij |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        | NoRelativeCarerSc, UndefinedSc | social_change_other     | abcdefghij |
  
#| housing_reasons  | Wohnraumsituations- und Ausstattungsgründe |                                             |            |
# | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | UNDEFINED_HOUSING_REASON                    |            |
# | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | UNDEFINED_HOUSING_REASON                    | abcdefghij |
#| housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE, UNDEFINED_HOUSING_REASON | abcdefghij | 

Szenariogrundriss: Die Auflistung enthalten doppelte Werte
    Angenommen die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '<Name>' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Doppelte Angaben bei '<Bezeichnung>' 
Beispiele:
    | Name             | Bezeichnung                               | Wert                                                      |
    | PersonalChanges | Veränderungen persönliche Situation        | OwnDesirePc, OwnDesirePc                                  |
    | PersonalChanges | Veränderungen persönliche Situation        | OwnDesirePc, OwnDesirePc, IncreasedAssitanceNeedPc        |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        | NoRelativeCarerSc, NoRelativeCarerSc                      |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        | NoRelativeCarerSc, NoRelativeCarerSc, MissingMealsSc      |
  
# ist keine liste
#| housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE, BARRIERS_ENTRANCE                      |
# | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE, BARRIERS_ENTRANCE, BARRIERS_HABITATION |

Szenariogrundriss: Die Auflistung enthalten gültige Werte
   Angenommen die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '<Name>' ist auf '<Wert1>' gesetzt
   Und die Eigenschaft '<Name2>' von 'Admission' ist auf '<Wert2>' gesetzt
   Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
    | Name            | Bezeichnung                                | Wert1                                  | Name2                 | Wert2      |
    | PersonalChanges | Veränderungen persönliche Situation        | OwnDesirePc, LossMainAttendancePc      | personal_change_other | abcdefghij |
    | PersonalChanges | Veränderungen persönliche Situation        | OwnDesirePc                            | personal_change_other |            |
    | PersonalChanges | Veränderungen persönliche Situation        |                                        | personal_change_other | abcdefghij |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        | NoRelativeCarerSc, MissingMealsSc      | social_change_other   | abcdefghij |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        | NoRelativeCarerSc                      | social_change_other   |            |
    | SocialChanges   | Veränderungen nicht bewältigt, weil        |                                        | social_change_other   | abcdefghij |

# ist keine liste
#    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE, BARRIERS_HABITATION | abcdefghij |
#    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE                      |            |
#    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe |                                        | abcdefghij | 
#

Szenario: Auswahlfelder enthalten Werte, die einen Texteintrag erfordern
    Angenommen die Eigenschaft 'housing_type_before_admission' von 'Admission' ist auf 'OtherAl' gesetzt
    Und die Eigenschaft 'other_housing_type' von 'Admission' ist auf '' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Bei 'Wohnsituation vor der Aufnahme' im Textfeld bitte einen Wert angegeben.'

Szenario: Auswahlfelder enthalten Werte, Texteintrag vorhanden
    Angenommen die Eigenschaft 'housing_type_before_admission' von 'Admission' ist auf 'OtherAl' gesetzt
    Und die Eigenschaft 'other_housing_type' von 'Admission' ist auf 'asdf' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler

#Szenariogrundriss: Auswahlfelder enthalten Werte, bei dem kein Texteintrag zugelassen wird
#    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>' und das Feld '<Textfeldname>' enthält den Wert '<Textfeldwert>'
#    Dann enthält das Validierungsergebnis den Fehler 'Bei '<Bezeichnung>' im Textfeld bitte keinen Wert angegeben.
#Beispiele:
#    | Name                          | Bezeichnung                    | Wert                 | Textfeldname       | Textfeldwert |
#    | housing_type_before_admission | Wohnsituation vor der Aufnahme | AD_HOME_LIVING_ALONE | other_housing_type | asdf         | 


# Abhängigkeit Auswahlfeld + Textfeld
#Szenariogrundriss: Auswahlfeld enthält einen Wert, der einen Texteintrag erfordern
#    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>' und das Feld '<Textfeldname>' enthält den Wert '<Textfeldwert>'
#    Dann enthält das Validierungsergebnis den Fehler 'Bei '<Bezeichnung>' im Textfeld bitte einen Wert angegeben.
#Beispiele:
#    | Name           | Bezeichnung                                | Wert     | Textfeldname         | Textfeldwert |
#    | housing_reason | Wohnraumsituations- und Ausstattungsgründe | OTHER_DR | housing_reason_other |              |




# Eine Admission enthält eine Person, die nicht in der Personenliste ist -> Fehler

# Ein Admission (valid) muss im akutellen Monat liegen
# Ein Admission (valid) muss zum Start eines Stays vorhanden sein
# Valid darf keine Zeit beinhalten
 