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


#Szenario: Pflegegeld ist undefiniert
#    Angenommen die Eigenschaft 'care_allowance' von 'Attribute' ist auf 'UndefinedAllowance' gesetzt
#    Dann enthält das Validierungsergebnis den Fehler 'Pflegegeld' darf nicht leer sein.'



#Szenario: Attribute müssen mit AttributeType übereinstimmen
#    Angenommen die Eigenschaft 'care_allowance' von 'Attribute' ist auf 'UndefinedAllowance' gesetzt
#    Dann enthält das Validierungsergebnis den Fehler 'Pflegegeld' darf nicht leer sein.'

