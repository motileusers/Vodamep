#language: de-DE
Funktionalität: StatLp - Validierung Aufenthalte einer Datenmeldung

Szenario: Ein Stay enthält eine Person, die nicht in der Personenliste ist
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'person_id' von 'Stay' ist auf '2' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Person '2' ist nicht in der Personenliste vorhanden.'

Szenariogrundriss: Datumswerte dürfen keine Zeit beinhalten
	Angenommen es ist ein 'StatLpReport'
	Und die Datums-Eigenschaft '<Name>' von 'Stay' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''<Bezeichnung>' darf keine Uhrzeit beinhalten.'
Beispiele:
	| Name | Bezeichnung |
	| from | Von         |
	| to   | Bis         |

Szenario: Bis ist nach Von
	Angenommen Bis ist vor Von bei einem Stay
	Dann enthält das Validierungsergebnis den Fehler ''Von' muss vor 'Bis' liegen.'

Szenario: Von darf nicht leer sein
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'from' von 'Stay' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Von' darf nicht leer sein.'

Szenario: Das Aufnahmedatum darf nicht nach dem Meldungszeitraumende liegen
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'to' von 'Stay' ist auf '2022-05-30' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Der Aufenthalt (.*) liegt nach dem Meldunszeitraum.'

Szenario: Die Aufnahme darf nicht zur Gänze vor dem Meldezeitraum liegen
	Angenommen die erste Aufnahme startet am '2020-05-30', dauert 10 Tage und ist eine 'HolidayAt'
	Dann enthält das Validierungsergebnis den Fehler 'Der Aufenthalt (.*) liegt vor dem Meldunszeitraum.'

Szenario: Die Aufnahme startet vor dem Meldezeitraum, dauert aber bis in den Meldezeitraum
	Angenommen die erste Aufnahme startet am '2020-05-30', dauert 10 Tage und ist eine 'HolidayAt'
	Und es gibt eine weitere Aufnahme 'ContinuousAt', die 9999 Tage dauert
	Dann enthält das Validierungsergebnis keine Fehler

Szenario: Die Aufnahme darf nicht zur Gänze vor dem Meldezeitraum liegen auch wenn einige Zeit danach eine weiter Aufnahme folgt
	Angenommen die erste Aufnahme startet am '2020-05-30', dauert 10 Tage und ist eine 'HolidayAt'
	Und es gibt eine weitere Aufnahme 'ContinuousAt', die 400 Tage dauert, dazwischen liegen 14 Tage
	Dann enthält das Validierungsergebnis den Fehler 'Der Aufenthalt (.*) liegt vor dem Meldunszeitraum.'

Szenario: Aufnahmen dürfen sich nicht überschneiden
	Angenommen die erste Aufnahme startet am '2021-05-30', dauert 10 Tage und ist eine 'HolidayAt'
	Und es gibt eine weitere Aufnahme 'ContinuousAt', die 60 Tage dauert, dazwischen liegen -5 Tage
	Dann enthält das Validierungsergebnis den Fehler 'Die Aufenthalte dürfen sich nicht überschneiden!'

Szenario: Die maximale Aufenthaltsdauer für HolidayAt ist 42 Tage
	Angenommen die erste Aufnahme startet am '2021-05-30', dauert 60 Tage und ist eine 'HolidayAt'
	Dann enthält das Validierungsergebnis die Warnung 'Der Aufenthalt (.*) darf nicht länger als 42 Tage dauern'

Szenario: Die maximale Aufenthaltsdauer für TransitionalAt ist 365 Tage
	Angenommen die erste Aufnahme startet am '2020-05-30', dauert 600 Tage und ist eine 'TransitionalAt'
	Dann enthält das Validierungsergebnis die Warnung 'Der Aufenthalt (.*) darf nicht länger als 365 Tage dauern'

Szenario: Aufeinanderfolgende Aufnahmen müssen eine unterschiedlichen Aufnahmeart haben
	Angenommen die erste Aufnahme startet am '2021-05-30', dauert 30 Tage und ist eine 'HolidayAt'
	Und es gibt eine weitere Aufnahme 'HolidayAt', die 30 Tage dauert
	Dann enthält das Validierungsergebnis den Fehler 'Aufeinanderfolgende Aufenthalte müssen unterschiedliche Aufnahmearten haben!'

Szenario: Es darf keinen Wechsel von Continuous auf HolidayAt geben
	Angenommen die erste Aufnahme startet am '2021-05-30', dauert 30 Tage und ist eine 'ContinuousAt'
	Und es gibt eine weitere Aufnahme 'HolidayAt', die 30 Tage dauert
	Dann enthält das Validierungsergebnis den Fehler 'Nach dem Aufenthalt 'Daueraufnahme' (.*) darf kein 'Urlaub von der Pflege' folgen.'

Szenario: Es darf keinen Wechsel von Continuous auf TransitionalAt geben
	Angenommen die erste Aufnahme startet am '2021-05-30', dauert 30 Tage und ist eine 'ContinuousAt'
	Und es gibt eine weitere Aufnahme 'TransitionalAt', die 30 Tage dauert
	Dann enthält das Validierungsergebnis den Fehler 'Nach dem Aufenthalt 'Daueraufnahme' (.*) darf kein 'Übergangspflege' folgen.'


Szenario: Ungülige Aufnahmeart Krisenintervention
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'type' von 'Stay' ist auf 'CrisisInterventionAt' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'bei der Aufnahme vom'
	Und enthält das Validierungsergebnis den Fehler 'ist nicht mehr erlaubt.'
	Und enthält das Validierungsergebnis genau einen Fehler

Szenario: Ungülige Aufnahmeart Probe
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'type' von 'Stay' ist auf 'TrialAt' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'bei der Aufnahme vom'
	Und enthält das Validierungsergebnis den Fehler 'ist nicht mehr erlaubt.'
	Und enthält das Validierungsergebnis genau einen Fehler

Szenario: Wenn Bis null ist, kommt danach kein Aufenthalt mehr
	Angenommen es gibt eine weitere Aufnahme 'GeriatricRemobilizationAt', die 30 Tage dauert
	Und die Eigenschaft 'to' von 'Stay' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Die Aufenthalte dürfen sich nicht überschneiden!'

Szenario: Jeder Aufenthaltsbeginn muss eine Aufnahmemeldung haben
	Angenommen die erste Aufnahme startet am '2021-05-30', dauert 30 Tage und ist eine 'ContinuousAt'
	Und die Liste 'Admission' ist leer
	Dann enthält das Validierungsergebnis den Fehler 'Für die Neuaufnahme vom '30.05.2021' von '(.*)' wurden keine Aufnahmedaten gemeldet.'

Szenario: Jeder Aufenthalt darf nur eine Aufnahmemeldung haben
	Angenommen die erste Aufnahme startet am '2021-05-01', dauert 10 Tage und ist eine 'TransitionalAt'
	Und eine weitere Aufnahme startet am '2021-05-12', dauert 30 Tage und ist eine 'ContinuousAt'
	Dann enthält das Validierungsergebnis den Fehler 'Für die Neuaufnahme (.*) wurden mehrere Aufnahmedaten gemeldet.'

Szenario: Jedes Aufenthaltsende muss eine Entlassungsmeldung haben
	Angenommen die erste Aufnahme startet am '2021-05-01', dauert 30 Tage und ist eine 'ContinuousAt'
	Und die Liste 'Leaving' ist leer
	Dann enthält das Validierungsergebnis den Fehler 'Für die Entlassung am '31.05.2021' von '(.*)' wurden keine Entlassungsdaten gemeldet.'

Szenario: Ein Aufenthaltsende am 31.12 muss nicht zwingend eine Entlassungsmeldung haben. Es könnte ein Wechsel der Aufnahmeart sein.
	Angenommen die erste Aufnahme startet am '2021-12-01', dauert 31 Tage und ist eine 'HolidayAt'
	Und die Eigenschaft 'to' von 'Stay' ist auf '2021-12-31' gesetzt
	Und die Liste 'Leaving' ist leer
	Dann enthält das Validierungsergebnis keine Fehler
	
Szenario: Jeder Aufenthalt darf nur eine Entlassungsmeldung haben
	Angenommen es gibt eine Entlassungsmeldung mehrfach
	Dann enthält das Validierungsergebnis den Fehler 'Für die Aufnahme (.*) wurden mehrere Entlassungsdaten gemeldet.'

Szenario: Zu jedem Aufenthalt müssen die Attribute definiert sein
	Angenommen die erste Aufnahme startet am '2021-05-30', dauert 30 Tage und ist eine 'ContinuousAt'
	Und die Liste 'Attribute' ist leer
	Dann enthält das Validierungsergebnis den Fehler 'Für die Person '(.*)' wurde am 30.05.2021 keine 'Pflegestufe' gemeldet'
	Und enthält das Validierungsergebnis den Fehler 'Für die Person '(.*)' wurde am 30.05.2021 keine 'Pflegestufe Arge' gemeldet'
	Und enthält das Validierungsergebnis den Fehler 'Für die Person '(.*)' wurde am 30.05.2021 keine 'Finanzierung' gemeldet'

Szenario: Bei mehreren Aufnahmen müssen gleich beleibende Attribute nicht mehrmals gemeldet werden
	Angenommen die erste Aufnahme startet am '2021-05-30', dauert 30 Tage und ist eine 'ContinuousAt'	
	Und eine weitere Aufnahme startet am '2021-08-12', dauert 30 Tage und ist eine 'ContinuousAt'
	Dann enthält das Validierungsergebnis keine Fehler
	