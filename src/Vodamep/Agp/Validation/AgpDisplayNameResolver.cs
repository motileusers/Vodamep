using System.Collections.Generic;
using Vodamep.Agp.Model;

namespace Vodamep.Agp.Validation
{
    internal class AgpDisplayNameResolver
    {
        private readonly IDictionary<string, string> _dict = new SortedDictionary<string, string>();

        public AgpDisplayNameResolver()
        {
            Init();
        }

        private void Init()
        {

            _dict.Add(nameof(AgpReport.To), "Bis");
            _dict.Add(nameof(AgpReport.ToD), "Bis");
            _dict.Add(nameof(AgpReport.From), "Von");
            _dict.Add(nameof(AgpReport.FromD), "Von");
            _dict.Add(nameof(AgpReport.Institution), "Einrichtung");

            //alle Ids werden auf "ID" geprüft.
            _dict.Add(nameof(Person.Id), "ID");
            _dict.Add(nameof(Person.Birthday), "Geburtsdatum");
            _dict.Add(nameof(Person.BirthdayD), "Geburtsdatum");
            _dict.Add(nameof(Person.Gender), "Geschlecht");
            _dict.Add(nameof(Person.Postcode), "Plz");
            _dict.Add(nameof(Person.City), "Ort");
            _dict.Add(nameof(Person.Insurance), "Versicherung");
            _dict.Add(nameof(Person.CareAllowance), "Pflegegeld");
            _dict.Add(nameof(Person.Referrer), "Zuweiser");
            _dict.Add(nameof(Person.OtherReferrer), "Sonstiger Zuweiser");
            _dict.Add(nameof(Person.HospitalDoctor), "Betreuender Arzt (Krankenhaus)");
            _dict.Add(nameof(Person.LocalDoctor), "Betreuender Arzt (Niedergelassener Bereich)");
            _dict.Add(nameof(Person.Diagnoses), "Erkrankungen / Diagnose");

            _dict.Add(nameof(Staff.GivenName), "Vorname");
            _dict.Add(nameof(Staff.FamilyName), "Familienname");
            
            _dict.Add(nameof(Activity), "Aktivität");
            _dict.Add(nameof(Activity.Date), "Datum");
            _dict.Add(nameof(Activity.DateD), "Datum");
            _dict.Add(nameof(Activity.PersonId), "Personen-ID");
            _dict.Add(nameof(Activity.StaffId), "Mitarbeiter-ID");
            _dict.Add(nameof(Activity.PlaceOfAction), "Einsatzort");
            _dict.Add(nameof(Activity.Entries), "Leistungsbereiche");
            _dict.Add(nameof(Activity.Minutes), "Leistungszeit");

            //_dict.Add(nameof(TravelTime.Id), "Wegzeiten-ID");
            //_dict.Add(nameof(TravelTime.Date), "Datum");
            //_dict.Add(nameof(TravelTime.DateD), "Datum");
            //_dict.Add(nameof(TravelTime.StaffId), "Mitarbeiter-ID");
            //_dict.Add(nameof(TravelTime.Minutes), "Wegzeit");

        }

        public string GetDisplayName(string name)
        {            
            if (!string.IsNullOrEmpty(name) && _dict.TryGetValue(name, out string value))
                return value;

            return name;
        }

    }
}
