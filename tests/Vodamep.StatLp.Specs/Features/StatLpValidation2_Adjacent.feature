#language: de-DE
Funktionalität: StatLp - Validierung zweier angrenzender Datenmeldungen

Szenario: Das Geburtsdatum der Person hat sich geändert
Dann enthält das Validierungsergebnis die Warnung 'xxxxx'

Szenario: Das Geschlecht der Person hat sich geändert
Dann enthält das Validierungsergebnis die Warnung 'xxxxx'

Szenario: Eine Person die nicht entlassen wurde, befindet sich nicht in der darauf folgenden Meldung
Dann enthält das Validierungsergebnis den Fehler 'xxxxx'

Szenario: Eine Person die nicht aufgenommen wurde, befindet sich nicht in der vorherigen Meldung
Dann enthält das Validierungsergebnis den Fehler 'xxxxx'	
