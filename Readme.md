# Vodamep

Vorarlberger Datenmeldung in der Pflege.

## Bereiche
### Hauskrankenpflege - HKPV 
- [Datenmodel](./src/Vodamep/Hkpv/Model/Hkpv.proto)
- Validierungen: ./tests/Vodamep.Spec/Hkpv/Features

### Stationäre Langzeitpflege - StatLp 
- [Datenmodel](./src/Vodamep/StatLp/Model/StatLp.proto)
- Validierungen: ./tests/Vodamep.Spec/StatLp/Features

### Mobile Kinderkrankenpflege - Mkkp
- [Datenmodel](./src/Vodamep/Mkkp/Model/Mkkp.proto)
- Validierungen: ./tests/Vodamep.Spec/Mkkp/Features

### Ambulante Gerontopsychiatrische Pflege - Agp
- [Datenmodel](./src/Vodamep/Agp/Model/Agp.proto)
- Validierungen: ./tests/Vodamep.Spec/Agp/Features

### Tb
- [Datenmodel](./src/Vodamep/Tb/Model/Tb.proto)
- Validierungen: ./tests/Vodamep.Spec/Tb/Features

### Cm
- [Datenmodel](./src/Vodamep/Cm/Model/Cm.proto)
- Validierungen: ./tests/Vodamep.Spec/Cm/Features

## Werte
- ./src/Vodamep/Datasets

## Projekte
- [Meldungsclient](./src/Vodamep.Client/Readme.md)
- [Meldungsserver](./src/Vodamep.Api/Readme.md)
- [Meldungsbibliothek](./src/Vodamep/Readme.md) zur Einbindung in .net-Projekte.


## Build

Für ein Build ist die Installation von [.Net SDK](https://www.microsoft.com/net/download/windows) erforderlich.
```
.\dotnet tool restore
.\dotnet cake
```
