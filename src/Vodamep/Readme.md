# Vodamep - Library


nuget-Paket installieren:
```
Install-Package Vodamep
```

Beispiel:
```
using System;
using Vodamep.Hkpv.Model;

class Program
{
    static void Main(string[] args)
    {    
        var report = HkpvReport.CreateDummyData();

        report.Persons[0].City = "Falsche Angabe";

        Console.WriteLine(report.ValidateToText());

        Console.ReadKey();
    }
}
```



