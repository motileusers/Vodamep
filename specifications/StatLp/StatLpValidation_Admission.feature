#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Aufnahmen einer Datenmeldung

Szenario: Das Aufnahmedatum muss im aktuellen Monat liegen
	Angenommen die Eigenschaft 'admission_date' von 'Admission' ist auf '2000-01-01' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Das Aufnahmedatum von'
	Und enthält das Validierungsergebnis den Fehler 'muss kleinergleich dem Gültigkeitsdatum'

Szenario: Das ursprüngliche Aufnahmedatum unterscheidet sich vom Aufnahmedatum
	Angenommen die Eigenschaft 'admission_date' von 'Admission' ist auf '2021-02-02' gesetzt
	Dann enthält das Validierungsergebnis die Warnung 'Das ursprüngliche Aufnahmedatum von'
	Und enthält das Validierungsergebnis die Warnung 'unterscheidet sich'

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
	Angenommen die Eigenschaft '<Name>' von 'Admission' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>''
	Und  enthält das Validierungsergebnis den Fehler 'darf nicht leer sein.'

	Beispiele:
		| Name                          | Bezeichnung                                    |
		| admission_date                | Aufnahmedatum                                  |
		| origin_admission_date         | Ursprüngliches Aufnahmedatum                   |
		| housing_type_before_admission | Wohnsituation vor der Aufnahme                 |
		| main_attendance_relation      | Verwandtschaftsverhältnis Hauptbetreuungspers. |
		| main_attendance_closeness     | Räumliche Nähe Hauptbetreuungsperson           |
		| housing_reason                | Wohnraumsituations- und Ausstattungsgründe     |
		| gender                        | Geschlecht                                     |
		| country                       | Land                                           |

Szenariogrundriss: Die Textfelder enthalten ungültige Werte
	Angenommen die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Ungültiger Wert für '<Bezeichnung>' bei Aufnahme vom 01.02.2021 von'

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
	Dann enthält das Validierungsergebnis den Fehler 'Zu langer Text für '<Bezeichnung>' bei Aufnahme vom 01.02.2021 von'

	Beispiele:
		| Name                  | Bezeichnung                                | Wert                                                                                                   |
		| other_housing_type    | Sonstige Lebens-/Betreuungssituation       | Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m x |
		| personal_change_other | Veränderungen persönliche Situation        | Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m x |
		| social_change_other   | Veränderungen nicht bewältigt, weil        | Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m x |
		| housing_reason_other  | Wohnraumsituations- und Ausstattungsgründe | Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m x |

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
	Angenommen die Eigenschaft 'last_postcode' von 'Admission' ist auf '<Value1>' gesetzt
	Und die Eigenschaft 'last_city' von 'Admission' ist auf '<Value2>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Ungültige Kombination Ort/Plz bei Aufnahme vom 01.02.2021 von'

	Beispiele:
		| Field1        | Field2    | Value1 | Value2    |
		| last_postcode | last_city | 0349   | Feldkirch |

Szenariogrundriss: Leerer Ort / Plz
	Angenommen die Eigenschaft 'last_postcode' von 'Admission' ist auf '<Value1>' gesetzt
	Und die Eigenschaft 'last_city' von 'Admission' ist auf '<Value2>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Keine Angabe von Ort/Plz bei Aufnahme vom 01.02.2021 von'

	Beispiele:
		| Field1        | Field2    | Value1 | Value2    |
		| last_postcode | last_city | 0349   |           |
		| last_postcode | last_city |        | Feldkirch |
		| last_postcode | last_city |        | Feldkirch |
		| last_postcode | last_city |        |           |

# vor 2019 wird PLZ / Ort nicht geprüft, da konnte alles gesendet werden
Szenariogrundriss: Gültiger Ort / Plz
	Angenommen Eine Meldung gilt vom <MeldungVon> bis <MeldungBis> und ist eine Standard Meldung und enthält eine Aufnahme von Person 1 vom <Aufnahme>
	Angenommen die Eigenschaft 'last_postcode' von 'Admission' ist auf '<PLZ>' gesetzt
	Und die Eigenschaft 'last_city' von 'Admission' ist auf '<Ort>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

	Beispiele:
		| PLZ  | Ort       | MeldungVon | MeldungBis | Aufnahme   |
		| 6800 | Feldkirch | 01.12.2019 | 31.12.2019 | 31.12.2019 |
		| 0000 | Anderer   | 01.12.2019 | 31.12.2019 | 31.12.2019 |
		| 9999 | ABCEDEF   | 01.12.2018 | 31.12.2018 | 31.12.2018 |

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
	Angenommen die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '<Name>' ist auf '<Wert1>' gesetzt
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
	Angenommen die Eigenschaft 'housing_type_before_admission' von 'Admission' ist auf 'OtherAl' gesetzt
	Und die Eigenschaft 'other_housing_type' von 'Admission' ist auf '' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Bei 'Wohnsituation vor der Aufnahme' im Textfeld bitte einen Wert angegeben.'

Szenario: Auswahlfelder enthalten Werte, Texteintrag vorhanden
	Angenommen die Eigenschaft 'housing_type_before_admission' von 'Admission' ist auf 'OtherAl' gesetzt
	Und die Eigenschaft 'other_housing_type' von 'Admission' ist auf 'asdf' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Szenario: Housing Reasons: Auswahlfelder enthalten Werte, die einen Texteintrag erfordern
	Angenommen die Eigenschaft 'housing_reason' von 'Admission' ist auf 'OtherHr' gesetzt
	Und die Eigenschaft 'housing_reason_other' von 'Admission' ist auf '' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Bei 'Wohnraumsituations- und Ausstattungsgründe' im Textfeld bitte einen Wert angegeben.'

Szenario: Housing Reasons: Auswahlfelder enthalten Werte, Texteintrag vorhanden
	Angenommen die Eigenschaft 'housing_reason' von 'Admission' ist auf 'OtherHr' gesetzt
	Und die Eigenschaft 'housing_reason_other' von 'Admission' ist auf 'asdf' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

Szenario: Eine Admission enthält eine Person, die nicht in der Personenliste ist
	Angenommen die Eigenschaft 'person_id' von 'Admission' ist auf '2' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Person '2' ist nicht in der Personenliste vorhanden.'

Szenario: Eine Aufnahme muss im aktuellen Monat liegen
	Angenommen die Eigenschaft 'admission_date' von 'Admission' ist auf '2000-01-01' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Das Aufnahmedatum von'
	Und enthält das Validierungsergebnis den Fehler 'muss kleinergleich dem Gültigkeitsdatum'

Szenario: Valid darf keine Zeit beinhalten
	Angenommen die Datums-Eigenschaft 'admission_date' von 'Admission' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Aufnahmedatum' darf keine Uhrzeit beinhalten.'

Szenariogrundriss: Das Land einer Aufnahme enthält einen ungültigen Wert
	Angenommen die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' von '
	Und enthält das Validierungsergebnis den Fehler 'hat einen ungültigen Wert'

	Beispiele:
		| Name    | Bezeichnung | Wert |
		| country | Land        | B    |
		| country | Land        | A    |

Szenariogrundriss: Das Land einer Aufnahme enthält einen gültigen Wert
	Angenommen die Eigenschaft '<Name>' von 'Admission' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

	Beispiele:
		| Name    | Bezeichnung | Wert |
		| country | Land        | AD   |
		| country | Land        | AT   |