#language: de-DE
Funktionalität: StatLp - Validierung der Entlassungen der einer Datenmeldung

Szenario: Das Entlassungsdatum muss gesetzt sein
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'leaving_date' von 'Leaving' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''LeavingDate' darf nicht leer sein.'

Szenario: Das Entlassungsdatum muss im Meldezeitraum liegen liegen
	Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft 'leaving_date' von 'Leaving' ist auf '2000-01-01' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Die Entlassung von Person '(.*)' muss im Meldezeitraum liegen.'

Szenariogrundriss: Pflichtfelder
	Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft '<Name>' von 'Leaving' ist nicht gesetzt
    Dann enthält das Validierungsergebnis den Fehler '<Fehler>'
Beispiele:
    | Name           | Fehler |
    | leaving_reason | Beim Abgang von Klient '(.*)' muss eine Abgang Art angegeben werden.   | 

Szenariogrundriss: Abhängigkeiten von Sterbefall / Entlassung
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'leaving_reason' von 'Leaving' ist auf '<leaving_reason>' gesetzt
	Und die Eigenschaft 'death_location' von 'Leaving' ist auf '<death_location>' gesetzt
	Und die Eigenschaft 'discharge_location' von 'Leaving' ist auf '<discharge_location>' gesetzt
	Und die Eigenschaft 'discharge_location_other' von 'Leaving' ist auf '<discharge_location_other>' gesetzt
	Und die Eigenschaft 'discharge_reason' von 'Leaving' ist auf '<discharge_reason>' gesetzt
	Und die Eigenschaft 'discharge_reason_other' von 'Leaving' ist auf '<discharge_reason_other>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
		| leaving_reason | death_location       | discharge_location	| discharge_location_other | discharge_reason		| discharge_reason_other |
		| DeceasedLr	 | DeathNursingHomeDl	|						|                          |						|                        |
		| DischargeLr    |					    | HomeLivingAloneDc		|                          | EndShortTermCareDr		|                        |
		| DischargeLr    |						| OtherDc				| asd                      | EndShortTermCareDr		|                        |
		| DischargeLr    |						| OtherDc				| asd                      | OtherDr				| asd                    |

Szenariogrundriss: Abhängigkeiten von Sterbefall / Entlassung - Fehler
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'leaving_reason' von 'Leaving' ist auf '<leaving_reason>' gesetzt
	Und die Eigenschaft 'death_location' von 'Leaving' ist auf '<death_location>' gesetzt
	Und die Eigenschaft 'discharge_location' von 'Leaving' ist auf '<discharge_location>' gesetzt
	Und die Eigenschaft 'discharge_location_other' von 'Leaving' ist auf '<discharge_location_other>' gesetzt
	Und die Eigenschaft 'discharge_reason' von 'Leaving' ist auf '<discharge_reason>' gesetzt
	Und die Eigenschaft 'discharge_reason_other' von 'Leaving' ist auf '<discharge_reason_other>' gesetzt
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
		| DischargeLr    |                       | HomeLivingAloneDc  |                          |                     |                        | Wenn der Klient '1' entlassen worden ist, muss angegeben werden, warum der Klient entlassen wurde.			|
 		|                |                       |                    |                          |                     |                        | Beim Abgang von Klient '(.*)' muss eine Abgang Art angegeben werden.												|	
		|                | DeathNursingHomeDl	 |                    |                          |                     |                        | Beim Abgang von Klient '(.*)' muss eine Abgang Art angegeben werden.												|
		|                |                       | HomeLivingAloneDc  |                          |                     |                        | Beim Abgang von Klient '(.*)' muss eine Abgang Art angegeben werden.												|
		|                |                       |                    |                          | EndShortTermCareDr  |                        | Beim Abgang von Klient '(.*)' muss eine Abgang Art angegeben werden.												|
		|                |                       |                    | abc                      |                     |                        | Beim Abgang von Klient '(.*)' muss eine Abgang Art angegeben werden.												|
		|                |                       |                    |                          |                     | abc                    | Beim Abgang von Klient '(.*)' muss eine Abgang Art angegeben werden.												|
		
Szenariogrundriss: Die Textfelder enthalten ungültige Werte
	Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft '<Name>' von 'Leaving' ist auf '<Wert>' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Ungültiger Wert für '<Bezeichnung>' bei Aufnahme vom '01.01.2021' von Klient '1'.'
Beispiele:
		| Name                     | Bezeichnung							| Wert |
		| discharge_location_other | Sonstige Lebens-/Betreuungssituation	| =    |
		| discharge_location_other | Sonstige Lebens-/Betreuungssituation	| 0    |
		| discharge_reason_other   | Entlassung Grund						| =    |
		| discharge_reason_other   | Entlassung Grund						| 0    |		

Szenariogrundriss: Die Textfelder enthalten zu lange Werte
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft '<Name>' von 'Leaving' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Zu langer Text für '<Bezeichnung>' bei Aufnahme vom '01.01.2021' von Klient '1'.'
Beispiele:
    | Name                     | Bezeichnung                          | Wert                               |
    | discharge_location_other | Sonstige Lebens-/Betreuungssituation | abcdefghij abcdefghij abcdefghij x |
    | discharge_reason_other   | Entlassung Grund                     | abcdefghij abcdefghij abcdefghij x |
    

Szenariogrundriss: Die Textfelder enthalten gültige Werte
	Angenommen es ist ein 'StatLpReport'
    Und die Eigenschaft '<Name1>' von 'Leaving' ist auf '<Wert1>' gesetzt
    Und die Eigenschaft '<Name2>' von 'Leaving' ist auf '<Wert2>' gesetzt
    Dann enthält das Validierungsergebnis keine Fehler
Beispiele:
    | Name1						| Wert1                  | Name2				| Wert2   |
    | discharge_location_other	| abcdefghij abcdefghij  | discharge_location	| OtherDc |
    | discharge_reason_other	| abcdefghij abcdefghij  | discharge_reason		| OtherDr |
  