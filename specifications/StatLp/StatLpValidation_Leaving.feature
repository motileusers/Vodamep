#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Entlassungen einer Datenmeldung

Szenariogrundriss: Pflichtfelder
    Angenommen die Eigenschaft '<Name>' von 'Leaving' ist nicht gesetzt
    Dann enthält das escapte Validierungsergebnis den Fehler ''<Bezeichnung>' darf nicht leer sein.'
Beispiele:
    | Name           | Bezeichnung |
    | leaving_reason | Abgangart   | 

Szenariogrundriss: Abhängigkeiten von Sterbefall / Entlassung
	Angenommen die Eigenschaft 'leaving_reason' von 'Leaving' ist auf '<leaving_reason>' gesetzt
	Angenommen die Eigenschaft 'death_location' von 'Leaving' ist auf '<death_location>' gesetzt
	Angenommen die Eigenschaft 'discharge_location' von 'Leaving' ist auf '<discharge_location>' gesetzt
	Angenommen die Eigenschaft 'discharge_location_other' von 'Leaving' ist auf '<discharge_location_other>' gesetzt
	Angenommen die Eigenschaft 'discharge_reason' von 'Leaving' ist auf '<discharge_reason>' gesetzt
	Angenommen die Eigenschaft 'discharge_reason_other' von 'Leaving' ist auf '<discharge_reason_other>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
		| leaving_reason | death_location       | discharge_location	| discharge_location_other | discharge_reason		| discharge_reason_other |
		| DeceasedLr	 | DeathNursingHomeDl	|						|                          |						|                        |
		| DischargeLr    |					    | HomeLivingAloneDc		|                          | EndShortTermCareDr		|                        |
		| DischargeLr    |						| OtherDc				| asd                      | EndShortTermCareDr		|                        |
		| DischargeLr    |						| OtherDc				| asd                      | OtherDr				| asd                    |
#Fehler weil Other gefüllt ist
#		| DischargeLr    |						| HomeLivingAloneDc		|                          |						| asdf                   |

Szenariogrundriss: Abhängigkeiten von Sterbefall / Entlassung - Fehler
	Angenommen die Eigenschaft 'leaving_reason' von 'Leaving' ist auf '<leaving_reason>' gesetzt
	Angenommen die Eigenschaft 'death_location' von 'Leaving' ist auf '<death_location>' gesetzt
	Angenommen die Eigenschaft 'discharge_location' von 'Leaving' ist auf '<discharge_location>' gesetzt
	Angenommen die Eigenschaft 'discharge_location_other' von 'Leaving' ist auf '<discharge_location_other>' gesetzt
	Angenommen die Eigenschaft 'discharge_reason' von 'Leaving' ist auf '<discharge_reason>' gesetzt
	Angenommen die Eigenschaft 'discharge_reason_other' von 'Leaving' ist auf '<discharge_reason_other>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler '<fehler>'
	Beispiele:
		| leaving_reason | death_location        | discharge_location | discharge_location_other | discharge_reason    | discharge_reason_other | fehler																										|
		| DeceasedLr	 |                       |                    |                          |                     |                        | Wenn der Klient '1' gestorben ist, muss eine Angabe zum Sterbeort gemacht werden.								|
		| DeceasedLr     | DeathNursingHomeDl  	 | HomeLivingAloneDc  |                          |                     |                        | Wenn der Klient '1' gestorben ist, darf keine Angabe zur Entlassung gemacht werden.							|
		| DeceasedLr     | DeathNursingHomeDl	 |                    | abc                      |                     |                        | Wenn der Klient '1' gestorben ist, darf keine Angabe zur Entlassung gemacht werden.							|
		| DeceasedLr     | DeathNursingHomeDl	 |                    |                          | EndShortTermCareDr  |                        | Wenn der Klient '1' gestorben ist, darf keine Angabe zur Entlassung gemacht werden.							|
		| DeceasedLr     | DeathNursingHomeDl	 |                    |                          |                     | asdf                   | Wenn der Klient '1' gestorben ist, darf keine Angabe zur Entlassung gemacht werden.							|
		| DischargeLr    |                       | NursingHomeDc      | asd                      | EndShortTermCareDr  |                        | Wenn bei der Entlassung von Klient '1' sonstige Angaben gemacht werden, muss 'Sonstige' ausgewählt werden.	|
		| DischargeLr    |                       | NursingHomeDc      |                          | EndShortTermCareDr  | asdfasd                | Wenn bei der Entlassung von Klient '1' sonstige Angaben gemacht werden, muss 'Sonstige' ausgewählt werden.	|
		| DischargeLr    |                       |                    |                          |                     |                        | Wenn der Klient '1' entlassen worden ist, muss angegeben werden, wohin der Klient entlassen wurde.			|
		| DischargeLr    | DeathNursingHomeDl	 |                    |                          |                     |                        | Wenn der Klient '1' entlassen worden ist, darf keine Angabe zum Sterbefall gemacht werden.					|
# verstehe den test nicht
#		| DischargeLr    |                       | HomeLivingAloneDc  |                          |                     |                        | Wenn der Klient '1' entlassen worden ist, muss angegeben werden, warum der Klient entlassen wurde.         |
# leaving reason darf nicht leer sein
#		|                |                       |                    |                          |                     |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                | DeathNursingHomeDl	 |                    |                          |                     |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                |                       | HomeLivingAloneDc  |                          |                     |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                |                       |                    |                          | EndShortTermCareDr  |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                |                       |                    | abc                      |                     |                        | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
#		|                |                       |                    |                          |                     | abc                    | Beim Abgang von Klient xx muss eine Abgang Art angegeben werden.'                                         |
		
Szenariogrundriss: Die Textfelder enthalten ungültige Werte
    Angenommen die Eigenschaft '<Name>' von 'Leaving' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Ungültiger Wert für '<Bezeichnung>' bei Aufnahme vom '01.02.2021' von Klient '1'.'
Beispiele:
		| Name                     | Bezeichnung							| Wert |
		| discharge_location_other | Sonstige Lebens-/Betreuungssituation	| =    |
		| discharge_location_other | Sonstige Lebens-/Betreuungssituation	| 0    |
		| discharge_reason_other   | Entlassung Grund						| =    |
		| discharge_reason_other   | Entlassung Grund						| 0    |		

Szenariogrundriss: Die Textfelder enthalten zu lange Werte
	Angenommen die Eigenschaft '<Name>' von 'Leaving' ist auf '<Wert>' gesetzt
   Dann enthält das Validierungsergebnis den Fehler 'Zu langer Text für '<Bezeichnung>' bei Aufnahme vom '01.02.2021' von Klient '1'.'
Beispiele:
    | Name                     | Bezeichnung                          | Wert                               |
    | discharge_location_other | Sonstige Lebens-/Betreuungssituation | abcdefghij abcdefghij abcdefghij x |
    | discharge_location_other | Sonstige Lebens-/Betreuungssituation | abcdefghij abcdefghij abcdefghij x |
    | discharge_reason_other   | Entlassung Grund                     | abcdefghij abcdefghij abcdefghij x |
    | discharge_reason_other   | Entlassung Grund                     | abcdefghij abcdefghij abcdefghij x |

Szenariogrundriss: Die Textfelder enthalten gültige Werte
    Angenommen die Eigenschaft '<Name1>' von 'Leaving' ist auf '<Wert1>' gesetzt
    Und die Eigenschaft '<Name2>' von 'Leaving' ist auf '<Wert2>' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
    | Name1						| Wert1                  | Name2				| Wert2   |
    | discharge_location_other	| abcdefghij abcdefghij  | discharge_location	| OtherDc |
    | discharge_reason_other	| abcdefghij abcdefghij  | discharge_reason		| OtherDr |
   
# war nicht beabsichtigt?	
#Szenariogrundriss: Die Textfelder enthalten gültige Werte
#    Angenommen die Eigenschaft '<Name>' von 'Leaving' ist auf '<Wert>' gesetzt
#    Dann enthält das Validierungsergebnis keine Fehler
#Beispiele:
#    | Name                  | Bezeichnung                                | Wert                             |
#    | other_housing_type    | Sonstige Lebens-/Betreuungssituation       | abcdefghij abcdefghij abcdefghij |
#    | personal_change_other | Veränderungen persönliche Situation        | abcdefghij abcdefghij abcdefghij |
#    | social_change_other   | Veränderungen nicht bewältigt, weil        | abcdefghij abcdefghij abcdefghij |
#    | housing_reas