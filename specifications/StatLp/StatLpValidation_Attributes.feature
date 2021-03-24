#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Attribute einer Datenmeldung

Szenario: Ein Attribut enthält eine Person, die nicht in der Personenliste ist
    Angenommen die Eigenschaft 'person_id' von 'Attribute' ist auf '2' gesetzt
    Dann enthält das Validierungsergebnis den Fehler 'Person '2' ist nicht in der Personenliste vorhanden.'

# Ein Attribute (From) muss im akutellen Monat liegen

 Szenario: Von darf keine Zeit beinhalten
    Angenommen die Datums-Eigenschaft 'from' von 'Attribute' hat eine Uhrzeit gesetzt
    Dann enthält das Validierungsergebnis den Fehler ''Von' darf keine Uhrzeit beinhalten.'



# Todo
# Attribute CAREALLOWANCEARGE darf nur L0_AR sein, wenn es sich um Altersheim handelt
# Klären: Wie kommen wir zur Gruppe ABAH?
# - innerhalb von Vodamep lokal
# - innerhalb vom Vodamep Server
# - innerhalb der Connexia Logik


#Szenario: Geschlecht ist undefiniert
#    Angenommen die Eigenschaft 'gender' von 'Person' ist auf 'UndefinedGender' gesetzt
#    Dann enthält das Validierungsergebnis den Fehler ''Geschlecht' darf nicht leer sein.'
#
#Szenario: Pflegegeld ist undefiniert
#    Angenommen die Eigenschaft 'care_allowance' von 'Person' ist auf 'UndefinedAllowance' gesetzt
#    Dann enthält das Validierungsergebnis den Fehler 'Pflegegeld' darf nicht leer sein.'


