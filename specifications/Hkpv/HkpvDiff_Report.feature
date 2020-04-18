#language: de-DE
Funktionalität: HkpvDiffReport

Szenario: Fehlender Klient.
	Angenommen die Meldung1 enthält die Klienten '1,2,3' mit Leistungen
	Und die Meldung2 enthält die Klienten '1,3' mit Leistungen
	Dann enthält das Ergebnis '2' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'PersonActivity', dem Wert1 '3' und dem Wert2 '2'
	Und das '2'. Element besitzt den Status 'Missing' mit der Id 'Person', dem Wert1 '2' und dem Wert2 ''

Szenario: Unterschied bei Klient
	Angenommen die Meldung1 enthält die Klienten '1,2,3' mit Leistungen
	Und die Meldung2 enthält die Klienten '1,2,3' mit Leistungen
	Und die Eigenschaft 'ssn' von 'Person' von Report 2 ist auf '4711030120' gesetzt
	Und die Eigenschaft 'family_name' von 'Person' von Report 2 ist auf 'Müller' gesetzt
	Dann enthält das Ergebnis '1' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'Person', dem Wert1 '1' und dem Wert2 ''

Szenario: Fehlender Mitarbeiter
	Angenommen die Meldung1 enthält den Mitarbeiter '1' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '2' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '3' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '1' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '3' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '0' Stunden pro Woche
	Dann enthält das Ergebnis '2' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'StaffActivity', dem Wert1 '3' und dem Wert2 '2'
	Und das '2'. Element besitzt den Status 'Missing' mit der Id 'Staff', dem Wert1 '2' und dem Wert2 ''

Szenario: Fehlende Klientenleistung
	Angenommen die Meldung1 enthält den Klienten '1' mit mit '2' Klientenleistungen
	Und die Meldung1 enthält den Klienten '2' mit mit '2' Klientenleistungen
	Und die Meldung1 enthält den Klienten '3' mit mit '2' Klientenleistungen
	Und die Meldung2 enthält den Klienten '1' mit mit '2' Klientenleistungen
	Und die Meldung2 enthält den Klienten '2' mit mit '1' Klientenleistungen
	Und die Meldung2 enthält den Klienten '3' mit mit '2' Klientenleistungen
	Dann enthält das Ergebnis '1' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'PersonActivity', dem Wert1 '6' und dem Wert2 '5'

Szenario: Unterschiedliche Klientenleistung
	Angenommen die Meldung1 enthält den Klienten '1' mit mit '1' Klientenleistungen
	Und die Meldung1 enthält den Klienten '2' mit mit '1' Klientenleistungen
	Und die Meldung1 enthält den Klienten '3' mit mit '1' Klientenleistungen
	Und die Meldung2 enthält den Klienten '1' mit mit '1' Klientenleistungen
	Und die Meldung2 enthält den Klienten '2' mit mit '1' Klientenleistungen
	Und die Meldung2 enthält den Klienten '3' mit mit '1' Klientenleistungen
	Und die Eigenschaft 'entries' von 'Activity' von Report 1 ist auf '2' gesetzt
	Dann enthält das Ergebnis '1' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'PersonActivity', dem Wert1 '6' und dem Wert2 '5'

Szenario: Fehlende nicht klientenbezogene Klientenleistung
	Angenommen die Meldung1 enthält den Mitarbeiter '1' mit '2' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '2' mit '2' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '3' mit '2' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '1' mit '2' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '2' mit '1' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '3' mit '2' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Dann enthält das Ergebnis '1' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'StaffActivity', dem Wert1 '6' und dem Wert2 '5'

Szenario: Unterschiedliche nicht klientenbezogene Klientenleistung
	Angenommen die Meldung1 enthält den Mitarbeiter '1' mit '1' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '2' mit '1' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '3' mit '1' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '1' mit '1' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '2' mit '1' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '3' mit '1' Mitarbeiterleistungen und mit '0' Anstellungen mit '0' Stunden pro Woche
	Und der '1'. Eintrag der '1'. Aktivität von Report 2 ist auf '2' gesetzt
	Dann enthält das Ergebnis '1' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'StaffActivity', dem Wert1 '6' und dem Wert2 '5'

Szenario: Fehlendes Anstellungsverhältnis Mitarbeiter
	Angenommen die Meldung1 enthält den Mitarbeiter '1' mit '1' Mitarbeiterleistungen und mit '2' Anstellungen mit '38.5' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '2' mit '1' Mitarbeiterleistungen und mit '3' Anstellungen mit '38.5' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '3' mit '1' Mitarbeiterleistungen und mit '4' Anstellungen mit '38.5' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '1' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '10.0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '2' mit '1' Mitarbeiterleistungen und mit '3' Anstellungen mit '38.5' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '3' mit '1' Mitarbeiterleistungen und mit '4' Anstellungen mit '38.5' Stunden pro Woche
	Dann enthält das Ergebnis '2' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'Employment', dem Wert1 '86.625' und dem Wert2 '69.875'
	Und das '2'. Element besitzt den Status 'Difference' mit der Id 'Staff', dem Wert1 '1' und dem Wert2 ''

Szenario: Unterschiedliches Anstellungsverhältnis Mitarbeiter
	Angenommen die Meldung1 enthält den Mitarbeiter '1' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '38.5' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '2' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '38.5' Stunden pro Woche
	Und die Meldung1 enthält den Mitarbeiter '3' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '38.5' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '1' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '10.0' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '2' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '38.5' Stunden pro Woche
	Und die Meldung2 enthält den Mitarbeiter '3' mit '1' Mitarbeiterleistungen und mit '1' Anstellungen mit '38.5' Stunden pro Woche
	Dann enthält das Ergebnis '2' Objekte(e)
	Und das '1'. Element besitzt den Status 'Difference' mit der Id 'Employment', dem Wert1 '28.875' und dem Wert2 '21.75'
	Und das '2'. Element besitzt den Status 'Difference' mit der Id 'Staff', dem Wert1 '1' und dem Wert2 ''
	