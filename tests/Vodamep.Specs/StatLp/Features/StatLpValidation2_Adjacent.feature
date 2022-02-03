#language: de-DE
Funktionalität: StatLp - Validierung zweier angrenzender Datenmeldungen

Szenario: Das Geburtsdatum der Person hat sich geändert
Angenommen es gibt zwei aneinander grenzende StatLp-Datenmeldungen
Und die Eigenschaft 'birthday' von 'Person' ist auf '2020-12-31' gesetzt
Dann enthält das Validierungsergebnis die Warnung 'Für die Person '(.*)' wurde für 'Geburtsdatum' bisher (.*) gemeldet.'

Szenario: Das Geschlecht der Person hat sich geändert
Angenommen es gibt zwei aneinander grenzende StatLp-Datenmeldungen
Und die Eigenschaft 'gender' von 'Admission' ist auf 'm' gesetzt
Dann enthält das Validierungsergebnis die Warnung 'Für die Person '(.*)' wurde für 'Geschlecht' bisher (.*) gemeldet.'

Szenario: Die Aufenthalte zweier Personen die in beiden Meldungen sind, passen nicht zusammen
Angenommen es gibt zwei aneinander grenzende StatLp-Datenmeldungen
Und die Eigenschaft 'from' von 'Stay' ist auf '2000-01-01' gesetzt
Dann enthält das Validierungsergebnis den Fehler 'Aufenthalte von '(.*)' stimmen nicht mit der vorhergehenden Meldung überein'

Szenario: Ein Aufenthalt, der bisher kein Ende-Datum hatte, wird in der aktuellen Meldung beendet
Angenommen es gibt zwei aneinander grenzende StatLp-Datenmeldungen
Und die Eigenschaft 'to' von 'Stay' ist auf '2021-01-31' gesetzt
Dann enthält das Validierungsergebnis keine Fehler

Szenario: Eine Person die nicht entlassen wurde, befindet sich nicht in der darauf folgenden Meldung
Angenommen es gibt zwei aneinander grenzende StatLp-Datenmeldungen
Und die Eigenschaft 'id' von 'Person' ist auf '123' gesetzt
Und die Eigenschaft 'person_id' von 'Stay' ist auf '123' gesetzt
Dann enthält das Validierungsergebnis den Fehler 'Aufenthalte von '(.*)' wurden dieses Jahr gemeldet. Sie fehlen im letzten Jahr'

Szenario: Eine Person die nicht aufgenommen wurde, befindet sich nicht in der vorherigen Meldung
Angenommen es gibt zwei aneinander grenzende StatLp-Datenmeldungen
Und die Eigenschaft 'id' von 'Person' ist auf '123' gesetzt
Und die Eigenschaft 'person_id' von 'Stay' ist auf '123' gesetzt
Dann enthält das Validierungsergebnis den Fehler 'Aufenthalte von '(.*)' wurden letztes Jahr gemeldet. Sie fehlen in diesem Jahr'	

Szenario: Es findet zum Jahresende eine Wechsel von Urlaubspflege zu Daueraufnahme statt
Angenommen es gibt zwei aneinander grenzende StatLp-Datenmeldungen
Und zum Jahresende wechselt die AufnahmeArt von 'HolidayAt' zu 'ContinuousAt'
Dann enthält das Validierungsergebnis keine Fehler

Szenario: Es findet zum Jahresende eine Wechsel von Urlaubspflege zu Daueraufnahme statt, die Daueraufnahme wird dann aber nicht gemeldet
Angenommen es gibt zwei aneinander grenzende StatLp-Datenmeldungen
Und zum Jahresende wechselt die AufnahmeArt von 'HolidayAt' zu 'ContinuousAt'
Und die Liste 'Stay' ist leer
Dann enthält das Validierungsergebnis den Fehler 'Aufenthalte von '(.*)' wurden letztes Jahr gemeldet. Sie fehlen in diesem Jahr'	