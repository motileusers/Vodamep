#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Aufenthalte einer Datenmeldung



# Pflichtfelder
Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
    Angenommen die Eigenschaft '<Name>' von 'Admission' ist nicht gesetzt
    Dann enthält das escapte Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name                          | Bezeichnung                                    |
    | housing_type_before_admission | Wohnsituation vor der Aufnahme                 |
    | main_attendance_relation      | Verwandtschaftsverhältnis Hauptbetreuungspers. |
    | main_attendance_closeness     | Räumliche Nähe Hauptbetreuungsperson           |
    | housing_reason                | Wohnraumsituations- und Ausstattungsgründe     | 


# Textfelder: falsche Zeichen
# RegEx @"^[-,.a-zA-ZäöüÄÖÜß\(\) ][-,.a-zA-ZäöüÄÖÜß\(\) ]*[-,.a-zA-ZäöüÄÖÜß\(\) ]$"
Szenariogrundriss: Die Textfelder enthalten ungültige Werte
    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>'
    Dann enthält das Validierungsergebnis den Fehler 'Ungültiger Wert' für '<Bezeichnung>' bei Aufnamhe vom xx von Klient xxx
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


# Textfelder: zu viele Zeichen
# 0 bis 30 Zeichen
Szenariogrundriss: Die Textfelder enthalten zu lange Werte
    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>'
    Dann enthält das Validierungsergebnis den Fehler 'Zu langer Text' für '<Bezeichnung>' bei Aufnamhe vom xx von Klient xxx
Beispiele:
    | Name                  | Bezeichnung                                | Wert                               |
    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | abcdefghij abcdefghij abcdefghij x |
    | personal_change_other | Veränderungen persönliche Situation        | abcdefghij abcdefghij abcdefghij x |
    | social_change_other   | Veränderungen nicht bewältigt, weil        | abcdefghij abcdefghij abcdefghij x |
    | housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | abcdefghij abcdefghij abcdefghij x | 


# Textfelder: alles gut
Szenariogrundriss: Die Textfelder enthalten gültige Werte
    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>'
    Dann enthält das Validierungsergebnis keinen Fehler
Beispiele:
    | Name                  | Bezeichnung                                | Wert                             |
    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | abcdefghij abcdefghij abcdefghij |
    | personal_change_other | Veränderungen persönliche Situation        | abcdefghij abcdefghij abcdefghij |
    | social_change_other   | Veränderungen nicht bewältigt, weil        | abcdefghij abcdefghij abcdefghij |
    | housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | abcdefghij abcdefghij abcdefghij | 



# PLZ Ort
Szenariogrundriss: Ungültiger Ort / Plz
    Angenommen die Eigenschaft '<Field1>' und <Field2> von 'Admission' sind auf die Werte '<Value1>' und '<Value2>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Ungültige Kombination Ort/Plz bei Aufnamhe vom xx von Klient xxx
Beispiele:
    | Field1        | Field2    | Value1 | Value2     |
    | last_postcode | last_city | 0349   | Feldkirch  |
    | last_postcode | last_city | 0349   |            |
    | last_postcode | last_city |        | Feldkirch  |
    | last_postcode | last_city |        | Feldkirch  |
    | last_postcode | last_city | 6751   | Ausserbraz |
    | last_postcode | last_city |        |            |
	
Szenariogrundriss: Gültiger Ort / Plz
    Angenommen die Eigenschaft '<Field1>' und <Field2> von 'Admission' sind auf die Werte '<Value1>' und '<Value2>' gesetzt
    Dann enthält das Validierungsergebnis keinen Fehler
Beispiele:
    | Field1        | Field2    | Value1 | Value2    |
    | last_postcode | last_city | 6800   | Feldkirch |
    | last_postcode | last_city | 0000   | Anderer   |






# Textfelder und Auflistungen prüfen

Szenariogrundriss: Die Auflistung enthalten ungültige Werte
    Angenommen die Auflistung '<Name>' von 'Admission' enhält die Werte '<Werte>' und der Wert des Textfeldes '<Freitextfeld>' ist '<Freitextwert>'
    Dann enthält das Validierungsergebnis den Fehler 'Keine gültige Angabe bei '<Bezeichnung>' 
Beispiele:
    | Name             | Bezeichnung                                | Wert                                        | Wert       |
    | personal_changes | Veränderungen persönliche Situation        |                                             |            |
    | personal_changes | Veränderungen persönliche Situation        | UNDEFINED_PERSONALCHANGE                    |            |
    | personal_changes | Veränderungen persönliche Situation        | UNDEFINED_PERSONALCHANGE                    | abcdefghij |
    | personal_changes | Veränderungen persönliche Situation        | OWN_DESIRE_PC, UNDEFINED_PERSONALCHANGE     | abcdefghij |
    | social_changes   | Veränderungen nicht bewältigt, weil        |                                             |            |
    | social_changes   | Veränderungen nicht bewältigt, weil        | UNDEFINED_SOCIAL_CHANGE                     |            |
    | social_changes   | Veränderungen nicht bewältigt, weil        | UNDEFINED_SOCIAL_CHANGE                     | abcdefghij |
    | social_changes   | Veränderungen nicht bewältigt, weil        | NO_RELATIVE_CARER, UNDEFINED_SOCIAL_CHANGE  | abcdefghij |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe |                                             |            |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | UNDEFINED_HOUSING_REASON                    |            |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | UNDEFINED_HOUSING_REASON                    | abcdefghij |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE, UNDEFINED_HOUSING_REASON | abcdefghij | 


Szenariogrundriss: Die Auflistung enthalten doppelte Werte
    Angenommen die Auflistung '<Name>' von 'Admission' enhält die Werte '<Werte>' und der Wert des Textfeldes '<Freitextfeld>' ist '<Freitextwert>'
    Dann enthält das Validierungsergebnis den Fehler 'Doppelte Angaben bei '<Bezeichnung>' 
Beispiele:
    | Name             | Bezeichnung                                | Wert                                                      | Wert |
    | personal_changes | Veränderungen persönliche Situation        | OWN_DESIRE_PC, OWN_DESIRE_PC                              |      |
    | personal_changes | Veränderungen persönliche Situation        | OWN_DESIRE_PC, OWN_DESIRE_PC, INCREASED_ASSITANCE_NEED    |      |
    | social_changes   | Veränderungen nicht bewältigt, weil        | NO_RELATIVE_CARER, NO_RELATIVE_CARER                      |      |
    | social_changes   | Veränderungen nicht bewältigt, weil        | NO_RELATIVE_CARER, NO_RELATIVE_CARER, MISSING_MEALS       |      |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE, BARRIERS_ENTRANCE                      |      |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE, BARRIERS_ENTRANCE, BARRIERS_HABITATION |      |


Szenariogrundriss: Die Auflistung enthalten gültige Werte
    Angenommen die Auflistung '<Name>' von 'Admission' enhält die Werte '<Werte>' und der Wert des Textfeldes '<Freitextfeld>' ist '<Freitextwert>'
    Dann enthält das Validierungsergebnis keinen Fehler
Beispiele:
    | Name             | Bezeichnung                                | Wert                                   | Wert       |
    | personal_changes | Veränderungen persönliche Situation        | OWN_DESIRE_PC, LOSS_MAIN_ATTENDANCE    | abcdefghij |
    | personal_changes | Veränderungen persönliche Situation        | OWN_DESIRE_PC                          |            |
    | personal_changes | Veränderungen persönliche Situation        |                                        | abcdefghij |
    | social_changes   | Veränderungen nicht bewältigt, weil        | NO_RELATIVE_CARER, MISSING_MEALS       | abcdefghij |
    | social_changes   | Veränderungen nicht bewältigt, weil        | NO_RELATIVE_CARER                      |            |
    | social_changes   | Veränderungen nicht bewältigt, weil        |                                        | abcdefghij |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE, BARRIERS_HABITATION | abcdefghij |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe | BARRIERS_ENTRANCE                      |            |
    | housing_reasons  | Wohnraumsituations- und Ausstattungsgründe |                                        | abcdefghij | 





# Abhängigkeit Listen + Textfeld
Szenariogrundriss: Auswahlfelder enthalten Werte, die einen Texteintrag erfordern
    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>' und das Feld '<Textfeldname>' enthält den Wert '<Textfeldwert>'
    Dann enthält das Validierungsergebnis den Fehler 'Bei '<Bezeichnung>' im Textfeld bitte einen Wert angegeben.
Beispiele:
    | Name                          | Bezeichnung                    | Wert     | Textfeldname       | Textfeldwert |
    | housing_type_before_admission | Wohnsituation vor der Aufnahme | AD_OTHER | other_housing_type |              | 


Szenariogrundriss: Auswahlfelder enthalten Werte, Texteintrag vorhanden
    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>' und das Feld '<Textfeldname>' enthält den Wert '<Textfeldwert>'
    Dann enthält das Validierungsergebnis keinen Fehler.
Beispiele:
    | Name                          | Bezeichnung                    | Wert     | Textfeldname       | Textfeldwert |
    | housing_type_before_admission | Wohnsituation vor der Aufnahme | AD_OTHER | other_housing_type | asdf         | 



Szenariogrundriss: Auswahlfelder enthalten Werte, bei dem kein Texteintrag zugelassen wird
    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>' und das Feld '<Textfeldname>' enthält den Wert '<Textfeldwert>'
    Dann enthält das Validierungsergebnis den Fehler 'Bei '<Bezeichnung>' im Textfeld bitte keinen Wert angegeben.
Beispiele:
    | Name                          | Bezeichnung                    | Wert                 | Textfeldname       | Textfeldwert |
    | housing_type_before_admission | Wohnsituation vor der Aufnahme | AD_HOME_LIVING_ALONE | other_housing_type | asdf         | 


# Abhängigkeit Auswahlfeld + Textfeld
Szenariogrundriss: Auswahlfeld enthält einen Wert, der einen Texteintrag erfordern
    Angenommen die Eigenschaft '<Name>' von 'Admission' enhält den Wert '<Wert>' und das Feld '<Textfeldname>' enthält den Wert '<Textfeldwert>'
    Dann enthält das Validierungsergebnis den Fehler 'Bei '<Bezeichnung>' im Textfeld bitte einen Wert angegeben.
Beispiele:
    | Name           | Bezeichnung                                | Wert     | Textfeldname         | Textfeldwert |
    | housing_reason | Wohnraumsituations- und Ausstattungsgründe | OTHER_DR | housing_reason_other |              |




# Eine Admission enthält eine Person, die nicht in der Personenliste ist -> Fehler

# Ein Admission (valid) muss im akutellen Monat liegen
# Ein Admission (valid) muss zum Start eines Stays vorhanden sein
# Valid darf keine Zeit beinhalten
 