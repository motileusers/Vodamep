using System.Collections.Generic;
using Vodamep.Mohi.Model;

namespace Vodamep.Mohi.Validation
{
    internal class DisplayNameResolver
    {
        private readonly IDictionary<string, string> _dict = new SortedDictionary<string, string>();

        public DisplayNameResolver()
        {
            Init();
        }

        private void Init()
        {
            _dict.Add(nameof(MohiReport.To), "Bis");
            _dict.Add(nameof(MohiReport.ToD), "Bis");

            _dict.Add(nameof(MohiReport.From), "Von");
            _dict.Add(nameof(MohiReport.FromD), "Von");

            _dict.Add(nameof(MohiReport.Institution), "Einrichtung");

            _dict.Add(nameof(Person.GivenName), "Vorname");
            _dict.Add(nameof(Person.FamilyName), "Familienname");
            _dict.Add(nameof(Person.Birthday), "Geburtsdatum");
            _dict.Add(nameof(Person.BirthdayD), "Geburtsdatum");

            _dict.Add(nameof(Person.CareAllowance), "Pflegegeld");
            _dict.Add(nameof(Person.Postcode), "PLZ");
            _dict.Add(nameof(Person.City), "Ort");
            _dict.Add(nameof(Person.Gender), "Geschlecht");
            _dict.Add(nameof(Person.Country), "Land");
            _dict.Add(nameof(Person.MainAttendanceCloseness), "Räumliche Nähe Hauptbetreuungsperson");
        }

        public string GetDisplayName(string name)
        {            
            if (!string.IsNullOrEmpty(name) && _dict.TryGetValue(name, out string value))
                return value;

            return name;
        }

    }
}
