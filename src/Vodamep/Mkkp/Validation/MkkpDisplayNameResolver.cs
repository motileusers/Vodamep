using System.Collections.Generic;
using Vodamep.Mkkp.Model;

namespace Vodamep.Mkkp.Validation
{
    internal class MkkpDisplayNameResolver
    {
        private readonly IDictionary<string, string> _dict = new SortedDictionary<string, string>();

        public MkkpDisplayNameResolver()
        {
            Init();
        }

        private void Init()
        {

            _dict.Add(nameof(MkkpReport.To), "Bis");
            _dict.Add(nameof(MkkpReport.ToD), "Bis");
            _dict.Add(nameof(MkkpReport.From), "Von");
            _dict.Add(nameof(MkkpReport.FromD), "Von");
            _dict.Add(nameof(MkkpReport.Institution), "Einrichtung");

            _dict.Add(nameof(Person.Id), "Personen-ID");
            _dict.Add(nameof(Person.Birthday), "Geburtsdatum");
            _dict.Add(nameof(Person.BirthdayD), "Geburtsdatum");
            _dict.Add(nameof(Person.Gender), "Geschlecht");
            _dict.Add(nameof(Person.Postcode), "Plz");
            _dict.Add(nameof(Person.City), "Ort");
            _dict.Add(nameof(Person.Insurance), "Versicherung");
            _dict.Add(nameof(Person.CareAllowance), "Pflegegeld");
            _dict.Add(nameof(Person.Referrer), "Zuweiser");
            _dict.Add(nameof(Person.OtherReferrer), "Sonstiger Zuweiser");

            _dict.Add(nameof(Activity.Id), "Leistungs-ID");
            _dict.Add(nameof(Activity.Date), "Datum");
            _dict.Add(nameof(Activity.DateD), "Datum");
            _dict.Add(nameof(Activity.PersonId), "Personen-ID");
            _dict.Add(nameof(Activity.StaffId), "Mitarbeiter-ID");
            _dict.Add(nameof(Activity.PlaceOfAction), "Einsatzort");
            _dict.Add(nameof(Activity.Entries), "Leistungsbereiche");
            _dict.Add(nameof(Activity.Minutes), "Leistungszeit");
            _dict.Add(nameof(Activity.HospitalDoctor), "Betreuender Arzt (Krankenhaus)");
            _dict.Add(nameof(Activity.LocalDoctor), "Betreuender Arzt (Niedergelassener Bereich");
            _dict.Add(nameof(Activity.Diagnoses), "Erkrankungen / Diagnose");

            _dict.Add(nameof(TravelTime.Id), "Wegzeiten-ID");
            _dict.Add(nameof(TravelTime.Date), "Datum");
            _dict.Add(nameof(TravelTime.DateD), "Datum");
            _dict.Add(nameof(TravelTime.StaffId), "Mitarbeiter-ID");
            _dict.Add(nameof(TravelTime.Minutes), "Wegzeit");

        }

        public string GetDisplayName(string name)
        {            
            if (!string.IsNullOrEmpty(name) && _dict.TryGetValue(name, out string value))
                return value;

            return name;
        }

    }
}
