#language: de-DE
Funktionalität: Agp - Validierung der gemeldeten Mitarbeiter Aktivitäten der Datenmeldung

Szenariogrundriss: Eine Eigenschaft ist nicht gesetzt
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft '<Name>' von 'StaffActivity' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler '<Bezeichnung> bei Mitarbeiter Aktivität (.*) darf nicht leer sein.'
Beispiele:
	| Name          | Bezeichnung   |
	| id            | ID            |
	| activity_type | Aktivitätstyp |

Szenariogrundriss: Datum ist nicht gesetzt
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft '<Name>' von 'StaffActivity' ist nicht gesetzt
	Dann enthält das Validierungsergebnis den Fehler '<Bezeichnung> bei Mitarbeiter Aktivität (.*) darf nicht leer sein.'
Beispiele:
	| Name          | Bezeichnung   |
	| date          | Datum         |

Szenariogrundriss: Minuten Werte müssen > 0 sein - Mitarbeiter Leistungen
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'minutes' von 'StaffActivity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis genau einen Fehler
	Und die Fehlermeldung lautet: 'Leistungszeit mit Id '(.*)' muss größer 0 sein.'
Beispiele:
	| Wert |
	| 0    |
	| -1   |

Szenariogrundriss: Minuten nur in 5 Minuten Schritten - Mitarbeiter Leistungen
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'minutes' von 'StaffActivity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis genau einen Fehler
	Und die Fehlermeldung lautet: 'Leistungszeit von '(.*)' darf nur in 5 Minuten Schritten eingegeben werden.'
Beispiele:
	| Wert |
	| 1    |
	| 3    |
	| 17   |

Szenariogrundriss: Minuten nur in 5 Minuten Schritten - Mitarbeiter Leistungen - korrekt
	Angenommen es ist ein 'AgpReport'
	Und die Eigenschaft 'minutes' von 'StaffActivity' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler
	Und es enthält keine Warnungen
Beispiele:
	| Wert |
	| 5    |
	| 10   |
	| 15   |


#todo -- doppelten Einträge in der StaffActivity pro Tag und Leistungstyp vermeiden