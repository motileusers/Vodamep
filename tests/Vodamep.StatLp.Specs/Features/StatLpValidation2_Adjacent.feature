﻿#language: de-DE
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
