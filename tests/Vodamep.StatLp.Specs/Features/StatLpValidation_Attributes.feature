#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Attribute einer Datenmeldung

Szenario: Ein Attribut enthält eine Person, die nicht in der Personenliste ist
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'person_id' von 'Attribute' ist auf '2' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Person '2' ist nicht in der Personenliste vorhanden.'

Szenario: Ein Attribut muss im aktuellen Monat liegen
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'from' von 'Attribute' ist auf '2000-01-01' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'muss im aktuellen Monat liegen.'

Szenario: Von darf keine Zeit beinhalten
	Angenommen es ist ein 'StatLpReport'
	Und die Datums-Eigenschaft 'from' von 'Attribute' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Von' darf keine Uhrzeit beinhalten.'

# Todo => nach dem refactoring
#Szenario: Ungülige Aufnahmeart Krisenintervention
#	Angenommen es ist ein 'StatLpReport'
#	Und die Eigenschaft 'attribute_type' von 'Attribute' ist auf 'AdmissionType' gesetzt
#	Und die Eigenschaft 'value' von 'Attribute' ist auf 'CrisisInterventionAt' gesetzt
#	Dann enthält das Validierungsergebnis den Fehler 'bei der Aufnahme vom'
#	Und enthält das Validierungsergebnis den Fehler 'ist nicht mehr erlaubt.'
#    Und enthält das Validierungsergebnis genau einen Fehler
#
#Szenario: Ungülige Aufnahmeart Probe
#	Angenommen es ist ein 'StatLpReport'
#	Und die Eigenschaft 'attribute_type' von 'Attribute' ist auf 'AdmissionType' gesetzt
#	Und die Eigenschaft 'value' von 'Attribute' ist auf 'TrialAt' gesetzt
#	Dann enthält das Validierungsergebnis den Fehler 'bei der Aufnahme vom'
#	Und enthält das Validierungsergebnis den Fehler 'ist nicht mehr erlaubt.'
#    Und enthält das Validierungsergebnis genau einen Fehler

# Todo => der Validator bekommt ein Enum (von Gerhard geliefert) im Konstruktor, hier abfragen
# Attribute CAREALLOWANCEARGE darf nur L0_AR sein, wenn es sich um Altersheim handelt
# Klären: Wie kommen wir zur Gruppe ABAH?
# - innerhalb von Vodamep lokal
# - innerhalb vom Vodamep Server
# - innerhalb der Connexia Logik

Szenario: Pflegegeld ist undefiniert
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'attribute_type' von 'Attribute' ist auf 'CareAllowance' gesetzt
	Und die Eigenschaft 'value' von 'Attribute' ist auf 'UndefinedAllowance' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Pflegestufe'
	Und enthält das Validierungsergebnis den Fehler 'darf nicht leer sein.'

Szenariogrundriss: Attributewert stimmt mit AttributTyp zusammen
	Angenommen es ist ein 'StatLpReport'
	Und das Attribut mit dem  Typ '<AttributeType>' ist auf den Wert '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

	Beispiele:
		| AttributeType     | Wert                      |
		| AdmissionType     | ContinuousAt              |
		| AdmissionType     | HolidayAt                 |
		| AdmissionType     | TransitionalAt            |
		| AdmissionType     | Covid19RespiteAt          |
		| AdmissionType     | GeriatricRemobilizationAt |
		| AdmissionType     | CareTransitionAt          |
		| CareAllowance     | L1                        |
		| CareAllowance     | L2                        |
		| CareAllowance     | L3                        |
		| CareAllowance     | L4                        |
		| CareAllowance     | L5                        |
		| CareAllowance     | L6                        |
		| CareAllowance     | L7                        |
		| CareAllowanceArge | L0Ar                      |
		| CareAllowanceArge | L1Ar                      |
		| CareAllowanceArge | L2Ar                      |
		| CareAllowanceArge | L3Ar                      |
		| CareAllowanceArge | L4Ar                      |
		| CareAllowanceArge | L5Ar                      |
		| CareAllowanceArge | L6Ar                      |
		| CareAllowanceArge | L7Ar                      |
		| Finance           | SelfFi                    |
		| Finance           | SocialAssistanceFi        |
		| Finance           | SocialAssistanceClaimFi   |

Szenariogrundriss: Attributewert stimmt nicht mit AttributTyp zusammen
	Angenommen es ist ein 'StatLpReport'
	Und die Eigenschaft 'attribute_type' von 'Attribute' ist auf '<AttributeType>' gesetzt
	Und die Eigenschaft 'value' von 'Attribute' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler '<Fehler>'

	Beispiele:
		| AttributeType     | Wert                      | Fehler                                                                                                                                   |
		| CareAllowance     | ContinuousAt              | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Daueraufnahme' gesetzt werden.                               |
		| CareAllowance     | HolidayAt                 | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Urlaub von der Pflege' gesetzt werden.                       |
		| CareAllowance     | TransitionalAt            | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Übergangspflege' gesetzt werden.                             |
		| CareAllowance     | Covid19RespiteAt          | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'COVID-19 Entlastungspflege' gesetzt werden.                  |
		| CareAllowance     | GeriatricRemobilizationAt | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Geriatrische Remobilisation' gesetzt werden.                 |
		| CareAllowance     | CareTransitionAt          | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Überleitungspflege' gesetzt werden.                          |
		| AdmissionType     | L1                        | Der Wert des Attributs mit dem Typen 'Aufnahmeart' kann nicht auf den Wert 'Stufe 1' gesetzt werden.                               |
		| AdmissionType     | L2                        | Der Wert des Attributs mit dem Typen 'Aufnahmeart' kann nicht auf den Wert 'Stufe 2' gesetzt werden.                               |
		| AdmissionType     | L3                        | Der Wert des Attributs mit dem Typen 'Aufnahmeart' kann nicht auf den Wert 'Stufe 3' gesetzt werden.                               |
		| AdmissionType     | L4                        | Der Wert des Attributs mit dem Typen 'Aufnahmeart' kann nicht auf den Wert 'Stufe 4' gesetzt werden.                               |
		| AdmissionType     | L5                        | Der Wert des Attributs mit dem Typen 'Aufnahmeart' kann nicht auf den Wert 'Stufe 5' gesetzt werden.                               |
		| AdmissionType     | L6                        | Der Wert des Attributs mit dem Typen 'Aufnahmeart' kann nicht auf den Wert 'Stufe 6' gesetzt werden.                               |
		| AdmissionType     | L7                        | Der Wert des Attributs mit dem Typen 'Aufnahmeart' kann nicht auf den Wert 'Stufe 7' gesetzt werden.                               |
		| Finance           | L0Ar                      | Der Wert des Attributs mit dem Typen 'Finanzierung' kann nicht auf den Wert 'Stufe 1' gesetzt werden.                              |
		| Finance           | L1Ar                      | Der Wert des Attributs mit dem Typen 'Finanzierung' kann nicht auf den Wert 'Stufe 2' gesetzt werden.                              |
		| Finance           | L2Ar                      | Der Wert des Attributs mit dem Typen 'Finanzierung' kann nicht auf den Wert 'Stufe 3' gesetzt werden.                              |
		| Finance           | L3Ar                      | Der Wert des Attributs mit dem Typen 'Finanzierung' kann nicht auf den Wert 'Stufe 4' gesetzt werden.                              |
		| Finance           | L4Ar                      | Der Wert des Attributs mit dem Typen 'Finanzierung' kann nicht auf den Wert 'Stufe 5' gesetzt werden.                              |
		| Finance           | L5Ar                      | Der Wert des Attributs mit dem Typen 'Finanzierung' kann nicht auf den Wert 'Stufe 6' gesetzt werden.                              |
		| Finance           | L6Ar                      | Der Wert des Attributs mit dem Typen 'Finanzierung' kann nicht auf den Wert 'Stufe 7' gesetzt werden.                              |
		| Finance           | L7Ar                      | Der Wert des Attributs mit dem Typen 'Finanzierung' kann nicht auf den Wert 'Stufe 8' gesetzt werden.                              |
		| CareAllowanceArge | SelfFi                    | Der Wert des Attributs mit dem Typen 'Pflegestufe Arge' kann nicht auf den Wert 'Selbst/Angehörige 100 %' gesetzt werden.                |
		| CareAllowanceArge | SocialAssistanceFi        | Der Wert des Attributs mit dem Typen 'Pflegestufe Arge' kann nicht auf den Wert 'Mindestsicherung' gesetzt werden.                       |
		| CareAllowanceArge | SocialAssistanceClaimFi   | Der Wert des Attributs mit dem Typen 'Pflegestufe Arge' kann nicht auf den Wert 'Mindestsicherungsantrag in Bearbeitung' gesetzt werden. |

Szenariogrundriss: Fehlende Pflichtfelder für die Aufnahme
	Angenommen es ist ein 'StatLpReport'
    Und das Attribut '<Name>' fehlt
    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient '(.*)' am 01.02.2021 wurde keine '<Bezeichnung>' gesendet.'
Beispiele:
    | Name              | Bezeichnung        |
    | AdmissionType     | Aufnahmeart        |
    | CareAllowance     | Pflegestufe        |
    | CareAllowanceArge | Pflegestufe (Arge) |
    | Finance           | Finanzierung       |

Szenario: Mehrfache Attribute	
    Angenommen enthält das zusätzliche Attribut der Person '1' mit dem  Typ 'AdmissionType' und dem Wert 'ContinuousAt'
    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient '(.*)' am 01.02.2021 wurde 'Aufnahmeart' mehrfach gesendet.'
