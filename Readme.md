# Allgemeines

Vorarlberger Datenmeldung in der Pflege.


# Anwendung


## Nuget Paket

Vodamep:
https://www.nuget.org/packages/Vodamep/


## URLs

Die Daten können an connexia über die folgende Endpunkte gesendet werden.

| Bereich | System | URL |
| ------- | ------ | --- |
| Case Management | Produktiv | https://daten.connexia.at/Vodamep/CaseManagement/Daten/ | 
| Case Management | Test | https://daten.connexia.at/Vodamep/CaseManagement/Test/ | 
| Gerontopsychiarische Pflege | Produktiv | https://daten.connexia.at/Vodamep/Agp/Daten/ | 
| Gerontopsychiarische Pflege | Test | https://daten.connexia.at/Vodamep/Agp/Test/ | 
| Hauskrankenpflege | Produktiv | https://daten.connexia.at/Vodamep/Hkp/Daten/ | 
| Hauskrankenpflege | Test | https://daten.connexia.at/Vodamep/Hkp/Test/ | 
| Mobile Hilfsdienste | Produktiv | https://daten.connexia.at/Vodamep/Mohi/Daten/ | 
| Mobile Hilfsdienste | Test | https://daten.connexia.at/Vodamep/Mohi/Test/ | 
| Mobile Kinderkrankenpflege | Produktiv | https://daten.connexia.at/Vodamep/Mkkp/Daten/ | 
| Mobile Kinderkrankenpflege | Test | https://daten.connexia.at/Vodamep/Mkkp/Test/ | 
| Stationär | Produktiv | https://daten.connexia.at/Vodamep/Stationaer/Daten/ | 
| Stationär | Test | https://daten.connexia.at/Vodamep/Stationaer/Test/ | 
| Tagesbetreuung | Produktiv | https://daten.connexia.at/Vodamep/Tagesbetreuung/Daten/ | 
| Tagesbetreuung | Test | https://daten.connexia.at/Vodamep/Tagesbetreuung/Test/ | 




# Source

## Bereiche
### Hauskrankenpflege - HKPV 
- [Datenmodel](./src/Vodamep/Hkpv/Model/Hkpv.proto)
- Validierungen: ./Testsystems/Vodamep.Spec/Hkpv/Features

### Stationäre Langzeitpflege - StatLp 
- [Datenmodel](./src/Vodamep/StatLp/Model/StatLp.proto)
- Validierungen: ./Testsystems/Vodamep.Spec/StatLp/Features

### Mobile Kinderkrankenpflege - Mkkp
- [Datenmodel](./src/Vodamep/Mkkp/Model/Mkkp.proto)
- Validierungen: ./Testsystems/Vodamep.Spec/Mkkp/Features

### Ambulante Gerontopsychiatrische Pflege - Agp
- [Datenmodel](./src/Vodamep/Agp/Model/Agp.proto)
- Validierungen: ./Testsystems/Vodamep.Spec/Agp/Features

### Tagesbetreuung - Tb
- [Datenmodel](./src/Vodamep/Tb/Model/Tb.proto)
- Validierungen: ./Testsystems/Vodamep.Spec/Tb/Features

### Case Management - Cm
- [Datenmodel](./src/Vodamep/Cm/Model/Cm.proto)
- Validierungen: ./Testsystems/Vodamep.Spec/Cm/Features

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
