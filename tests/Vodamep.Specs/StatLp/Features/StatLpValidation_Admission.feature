#language: de-DE
Funktionalität: StatLp - Validierung Aufnahmen einer Datenmeldung

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft '<Name>' von 'Admission' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>''
	Und enthält das Validierungsergebnis den Fehler 'darf nicht leer sein.'

Beispiele:
	| Name                          | Bezeichnung                                    |
	| admission_date                | Aufnahmedatum                                  |
	| housing_type_before_admission | Wohnsituation vor der Aufnahme                 |
	| main_attendance_relation      | Verwandtschaftsverhältnis Hauptbetreuungspers. |
	| main_attendance_closeness     | Räumliche Nähe Hauptbetreuungsperson           |
	| housing_reason                | Wohnraumsituations- und Ausstattungsgründe     |
	| gender                        | Geschlecht                                     |
	| nationality                   | Staatsbürgerschaft                             |


Szenariogrundriss: Die Textfelder enthalten ungültige Werte
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Ungültiger Wert für '<Bezeichnung>' bei Aufnahme vom 01.01.2021 von'

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
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Zu langer Text für '<Bezeichnung>' bei Aufnahme vom 01.01.2021 von'

Beispiele:
	| Name                  | Bezeichnung                                | Wert                                                                                                   |
	| other_housing_type    | Sonstige Lebens-/Betreuungssituation       | Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m x |
	| personal_change_other | Veränderungen persönliche Situation        | Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m x |
	| social_change_other   | Veränderungen nicht bewältigt, weil        | Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m x |
	| housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m x |

Szenariogrundriss: Die Textfelder enthalten gültige Werte
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Beispiele:
	| Name                  | Bezeichnung                                | Wert                           |
	| other_housing_type    | Sonstige Lebens-/Betreuungssituation       | abcdefghij abcdefghij abcdefgh |
	| personal_change_other | Veränderungen persönliche Situation        | abcdefghij abcdefghij abcdefgh |
	| social_change_other   | Veränderungen nicht bewältigt, weil        | abcdefghij abcdefghij abcdefgh |
	| housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | abcdefghij abcdefghij abcdefgh |

Szenariogrundriss: Ungültiger Ort / Plz
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'last_postcode' von 'Admission' ist auf '<Value1>' gesetzt
	Und die Eigenschaft 'last_city' von 'Admission' ist auf '<Value2>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Ungültige Kombination Ort/Plz'

Beispiele:
	| Field1        | Field2    | Value1 | Value2    |
	| last_postcode | last_city | 0349   | Feldkirch |

Szenariogrundriss: Leerer Ort / Plz
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'last_postcode' von 'Admission' ist auf '<Value1>' gesetzt
	Und die Eigenschaft 'last_city' von 'Admission' ist auf '<Value2>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Keine Angabe von Ort/Plz'

Beispiele:
	| Field1        | Field2    | Value1 | Value2    |
	| last_postcode | last_city | 0349   |           |
	| last_postcode | last_city |        | Feldkirch |
	| last_postcode | last_city |        |           |

# vor 2019 wird PLZ / Ort nicht geprüft, da konnte alles gesendet werden
Szenariogrundriss: Gültiger Ort / Plz
	Angenommen die erste Aufnahme startet am '2018-05-30' und ist eine 'ContinuousAt'
	Und die Eigenschaft 'last_postcode' von 'Admission' ist auf '<PLZ>' gesetzt
	Und die Eigenschaft 'last_city' von 'Admission' ist auf '<Ort>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Beispiele:
	| PLZ  | Ort       |
	| 6800 | Feldkirch |
	| 0000 | Anderer   |
	| 9999 | ABCEDEF   |

Szenariogrundriss: Die Auflistung enthalten ungültige Werte
	Angenommen die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '<Name>' ist auf '<Wert1>' gesetzt
	Und die Eigenschaft '<Name2>' von 'Admission' ist auf '<Wert2>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Keine gültige Angabe bei '<Bezeichnung>'

Beispiele:
	| Name            | Bezeichnung                         | Wert1                          | Name2                 | Wert2      |
	| PersonalChanges | Veränderungen persönliche Situation |                                | personal_change_other |            |
	| PersonalChanges | Veränderungen persönliche Situation | UndefinedPc                    | personal_change_other |            |
	| PersonalChanges | Veränderungen persönliche Situation | UndefinedPc                    | personal_change_other | abcdefghij |
	| PersonalChanges | Veränderungen persönliche Situation | OwnDesirePc, UndefinedPc       | personal_change_other | abcdefghij |
	| SocialChanges   | Veränderungen nicht bewältigt, weil |                                | social_change_other   |            |
	| SocialChanges   | Veränderungen nicht bewältigt, weil | UndefinedSc                    | social_change_other   |            |
	| SocialChanges   | Veränderungen nicht bewältigt, weil | UndefinedSc                    | social_change_other   | abcdefghij |
	| SocialChanges   | Veränderungen nicht bewältigt, weil | NoRelativeCarerSc, UndefinedSc | social_change_other   | abcdefghij |

Szenariogrundriss: Die Auflistung enthalten doppelte Werte
	Angenommen die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '<Name>' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Doppelte Angaben bei '<Bezeichnung>'

Beispiele:
	| Name            | Bezeichnung                         | Wert                                                 |
	| PersonalChanges | Veränderungen persönliche Situation | OwnDesirePc, OwnDesirePc                             |
	| PersonalChanges | Veränderungen persönliche Situation | OwnDesirePc, OwnDesirePc, IncreasedAssitanceNeedPc   |
	| SocialChanges   | Veränderungen nicht bewältigt, weil | NoRelativeCarerSc, NoRelativeCarerSc                 |
	| SocialChanges   | Veränderungen nicht bewältigt, weil | NoRelativeCarerSc, NoRelativeCarerSc, MissingMealsSc |

Szenariogrundriss: Die Auflistung enthalten gültige Werte
	Angenommen es ist ein 'StatLpReport'
	Und die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '<Name>' ist auf '<Wert1>' gesetzt
	Und die Eigenschaft '<Name2>' von 'Admission' ist auf '<Wert2>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Beispiele:
	| Name            | Bezeichnung                         | Wert1                             | Name2                 | Wert2      |
	| PersonalChanges | Veränderungen persönliche Situation | OwnDesirePc, LossMainAttendancePc | personal_change_other | abcdefghij |
	| PersonalChanges | Veränderungen persönliche Situation | OwnDesirePc                       | personal_change_other |            |
	| PersonalChanges | Veränderungen persönliche Situation |                                   | personal_change_other | abcdefghij |
	| SocialChanges   | Veränderungen nicht bewältigt, weil | NoRelativeCarerSc, MissingMealsSc | social_change_other   | abcdefghij |
	| SocialChanges   | Veränderungen nicht bewältigt, weil | NoRelativeCarerSc                 | social_change_other   |            |
	| SocialChanges   | Veränderungen nicht bewältigt, weil |                                   | social_change_other   | abcdefghij |

Szenario: Auswahlfelder enthalten Werte, die einen Texteintrag erfordern
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'housing_type_before_admission' von 'Admission' ist auf 'OtherAl' gesetzt
	Und die Eigenschaft 'other_housing_type' von 'Admission' ist auf '' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Bei 'Wohnsituation vor der Aufnahme' im Textfeld bitte einen Wert angegeben.'

Szenario: Auswahlfelder enthalten Werte, Texteintrag vorhanden
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'housing_type_before_admission' von 'Admission' ist auf 'OtherAl' gesetzt
	Und die Eigenschaft 'other_housing_type' von 'Admission' ist auf 'asdf' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Szenario: Housing Reasons: Auswahlfelder enthalten Werte, die einen Texteintrag erfordern
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'housing_reason' von 'Admission' ist auf 'OtherHr' gesetzt
	Und die Eigenschaft 'housing_reason_other' von 'Admission' ist auf '' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Bei 'Wohnraumsituations- und Ausstattungsgründe' im Textfeld bitte einen Wert angegeben.'

Szenario: Housing Reasons: Auswahlfelder enthalten Werte, Texteintrag vorhanden
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'housing_reason' von 'Admission' ist auf 'OtherHr' gesetzt
	Und die Eigenschaft 'housing_reason_other' von 'Admission' ist auf 'asdf' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Szenario: Eine Admission enthält eine Person, die nicht in der Personenliste ist
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'person_id' von 'Admission' ist auf '2' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Person '2' ist nicht in der Personenliste vorhanden.'

Szenario: Valid darf keine Zeit beinhalten
	Angenommen es ist ein 'StatLpReport'
	Und die Datums-Eigenschaft 'admission_date' von 'Admission' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Aufnahmedatum' darf keine Uhrzeit beinhalten.'

Szenariogrundriss: Das Staatsbürgerschaft einer Aufnahme enthält einen ungültigen Wert
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von '
	Und enthält das Validierungsergebnis den Fehler 'hat einen ungültigen Wert'

Beispiele:
	| Name        | Bezeichnung        | Wert |
	| nationality | Staatsbürgerschaft | B    |
	| nationality | Staatsbürgerschaft | A    |

Szenariogrundriss: Das Staatsbürgerschaft einer Aufnahme enthält einen gültigen Wert
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Beispiele:
	| Name        | Bezeichnung        | Wert |
	| nationality | Staatsbürgerschaft | AD   |
	| nationality | Staatsbürgerschaft | AT   |
	| nationality | Staatsbürgerschaft | ZZ   |