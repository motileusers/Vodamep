
# Vodamep - Client

(dmc) Daten-Meldungs-Client:

```
dmc.exe

Usage -  <action>

Actions

  Send <File> -options - Absenden der Meldung.

    Option          Description
    File* (-F)
    Address* (-A)
    User (-U)
    Password (-P)

  Validate <File>  - Prüfung der Meldung.

    Option       Description
    File* (-F)

  PackFile <File> -options - Meldung neu verpacken.

    Option               Description
    File* (-F)
    Json (-J)            Save as JSON.
    NoCompression (-N)   [Default='False']

  PackRandom -options - Meldung mit Testdaten erzeugen.

    Option               Description
    Month (-M)
    Year (-Y)
    Persons (-P)         [Default='100']
    Staffs (-S)          [Default='5']
    AddActivities (-A)   [Default='True']
    Json (-J)            Save as JSON.
    NoCompression (-N)   [Default='False']

  List <Source>  - Listet erlaubte Werte.

    Option         Description
    Source* (-S)
                   Insurances
                   CountryCodes
                   Postcode_City


  Diff <File> <File> <File>  - Vergleiche 2 Dateien.

    Option         Description
    Source* (-S)
                   Insurances
                   CountryCodes
                   Postcode_City

```


Beispiele:

```
Stammdaten auflisten

PLZ/Orte - Standard Liste für alle Module, außer HKPV
dmc List -s Postcode_City

PLZ/Orte - Liste für HKPV
dmc List -s Postcode_City -t hkpv



Validierungen Historie - nur StatLP

Für eine Datei (-f) die gesamte Historie (-h)
validatehistory -f D:\Data\C-0791-201509.json -h D:\Data\C*.* -t StatLp

Für alle vorhandenen Dateien (-f) die gesamte Historie (-h) mit Wildcard 
validatehistory -f D:\Data\C*.json -h D:\Data\C*.* -t StatLp





```
