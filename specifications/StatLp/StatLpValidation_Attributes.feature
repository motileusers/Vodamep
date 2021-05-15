#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Attribute einer Datenmeldung

Szenario: Ein Attribut enthält eine Person, die nicht in der Personenliste ist
	Angenommen die Eigenschaft 'person_id' von 'Attribute' ist auf '2' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Person '2' ist nicht in der Personenliste vorhanden.'

Szenario: ein Attribut muss im aktuellen Monat liegen
	Angenommen die Eigenschaft 'valid' von 'Admission' ist auf '2000-01-01' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Die Aufnahme von Person '1' muss im aktuellen Monat liegen.'

Szenario: Von darf keine Zeit beinhalten
	Angenommen die Datums-Eigenschaft 'from' von 'Attribute' hat eine Uhrzeit gesetzt
	Dann enthält das Validierungsergebnis den Fehler ''Von' darf keine Uhrzeit beinhalten.'

# Todo => der Validator bekommt ein Enum (von Gerhard geliefert) im Konstruktor, hier abfragen
# Attribute CAREALLOWANCEARGE darf nur L0_AR sein, wenn es sich um Altersheim handelt
# Klären: Wie kommen wir zur Gruppe ABAH?
# - innerhalb von Vodamep lokal
# - innerhalb vom Vodamep Server
# - innerhalb der Connexia Logik

Szenario: Pflegegeld ist undefiniert
	Angenommen die Eigenschaft 'attribute_type' von 'Attribute' ist auf 'Careallowance' gesetzt
	Angenommen die Eigenschaft 'value' von 'Attribute' ist auf 'UndefinedAllowance' gesetzt
	Dann enthält das Validierungsergebnis den Fehler 'Pflegestufe' von Klient '1' darf nicht leer sein.'

Szenariogrundriss: Attributewert stimmt mit AttributTyp zusammen
	Angenommen das Attribut mit dem  Typ '<AttributeType>' ist auf den Wert '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis keine Fehler

	Beispiele:
		| AttributeType     | Wert                      |
		| AdmissionType     | ContinuousAt              |
		| AdmissionType     | HolidayAt                 |
		| AdmissionType     | TransitionalAt            |
		| AdmissionType     | Covid19RespiteAt          |
		| AdmissionType     | GeriatricRemobilizationAt |
		| AdmissionType     | CareTransitionAt          |
		| Careallowance     | L1                        |
		| Careallowance     | L2                        |
		| Careallowance     | L3                        |
		| Careallowance     | L4                        |
		| Careallowance     | L5                        |
		| Careallowance     | L6                        |
		| Careallowance     | L7                        |
		| Careallowancearge | L0Ar                      |
		| Careallowancearge | L1Ar                      |
		| Careallowancearge | L2Ar                      |
		| Careallowancearge | L3Ar                      |
		| Careallowancearge | L4Ar                      |
		| Careallowancearge | L5Ar                      |
		| Careallowancearge | L6Ar                      |
		| Careallowancearge | L7Ar                      |
		| Finance           | SelfFi                    |
		| Finance           | SocialAssistanceFi        |
		| Finance           | SocialAssistanceClaimFi   |

Szenariogrundriss: Attributewert stimmt nicht mit AttributTyp zusammen
	Angenommen die Eigenschaft 'attribute_type' von 'Attribute' ist auf '<AttributeType>' gesetzt
	Angenommen die Eigenschaft 'value' von 'Attribute' ist auf '<Wert>' gesetzt
	Dann enthält das Validierungsergebnis den Fehler '<Fehler>'

	Beispiele:
		| AttributeType     | Wert                      | Fehler                                                                                                                                   |
		| Careallowance     | ContinuousAt              | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Daueraufnahme' gesetzt werden.                               |
		| Careallowance     | HolidayAt                 | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Urlaub von der Pflege' gesetzt werden.                       |
		| Careallowance     | TransitionalAt            | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Übergangspflege' gesetzt werden.                             |
		| Careallowance     | Covid19RespiteAt          | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'COVID-19 Entlastungspflege' gesetzt werden.                  |
		| Careallowance     | GeriatricRemobilizationAt | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Geriatrische Remobilisation' gesetzt werden.                 |
		| Careallowance     | CareTransitionAt          | Der Wert des Attributs mit dem Typen 'Pflegestufe' kann nicht auf den Wert 'Überleitungspflege' gesetzt werden.                          |
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
		| Careallowancearge | SelfFi                    | Der Wert des Attributs mit dem Typen 'Pflegestufe Arge' kann nicht auf den Wert 'Selbst/Angehörige 100 %' gesetzt werden.                |
		| Careallowancearge | SocialAssistanceFi        | Der Wert des Attributs mit dem Typen 'Pflegestufe Arge' kann nicht auf den Wert 'Mindestsicherung' gesetzt werden.                       |
		| Careallowancearge | SocialAssistanceClaimFi   | Der Wert des Attributs mit dem Typen 'Pflegestufe Arge' kann nicht auf den Wert 'Mindestsicherungsantrag in Bearbeitung' gesetzt werden. |

Szenariogrundriss: Fehlende Pflichtfelder für die Aufnahme
    Angenommen das Attribut '<Name>' fehlt
    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient '1' am 01.02.2021 wurde keine '<Bezeichnung>' gesendet.'
Beispiele:
    | Name              | Bezeichnung        |
    | AdmissionType     | Aufnahmeart        |
    | Careallowance     | Pflegestufe        |
    | Careallowancearge | Pflegestufe (Arge) |
    | Finance           | Finanzierung       |

Szenario: Mehrfache Attribute
    Angenommen enthält das zusätzliche Attribut der Person '1' mit dem  Typ 'AdmissionType' und dem Wert 'ContinuousAt'
    Dann enthält das Validierungsergebnis den Fehler 'Vor der Aufnahme von Klient '1' am 01.02.2021 wurde 'Aufnahmeart' mehrfach gesendet.'
