#language: de-DE
Funktionalität: StatLp - Validierung der gemeldeten Aufenthalte einer Datenmeldung


# Ein Attribute enthält eine Person, die nicht in der Personenliste ist -> Fehler

# Ein Attribute (From) muss im akutellen Monat liegen
# From darf keine Zeit beinhalten




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


