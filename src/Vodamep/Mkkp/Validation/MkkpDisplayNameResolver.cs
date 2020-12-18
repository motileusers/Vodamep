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
            _dict.Add(nameof(Person.HospitalDoctor), "Betreuender Arzt (Krankenhaus)");
            _dict.Add(nameof(Person.LocalDoctor), "Betreuender Arzt (Niedergelassener Bereich");
            _dict.Add(nameof(Person.Diagnoses), "Erkrankungen / Diagnose");

            if (!_dict.ContainsKey(nameof(Activity.Id)))
                _dict.Add(nameof(Activity.Id), "Leistungs-ID");
            if (!_dict.ContainsKey(nameof(Activity.Date)))
                _dict.Add(nameof(Activity.Date), "Datum");
            if (!_dict.ContainsKey(nameof(Activity.DateD)))
                _dict.Add(nameof(Activity.DateD), "Datum");
            if (!_dict.ContainsKey(nameof(Activity.PersonId)))
                _dict.Add(nameof(Activity.PersonId), "Personen-ID");
            if (!_dict.ContainsKey(nameof(Activity.StaffId)))
                _dict.Add(nameof(Activity.StaffId), "Mitarbeiter-ID");
            if (!_dict.ContainsKey(nameof(Activity.PlaceOfAction)))
                _dict.Add(nameof(Activity.PlaceOfAction), "Einsatzort");
            if (!_dict.ContainsKey(nameof(Activity.Entries)))
                _dict.Add(nameof(Activity.Entries), "Leistungsbereiche");
            if (!_dict.ContainsKey(nameof(Activity.Minutes)))
                _dict.Add(nameof(Activity.Minutes), "Leistungszeit");

            if (!_dict.ContainsKey(nameof(TravelTime.Id)))
                _dict.Add(nameof(TravelTime.Id), "Wegzeiten-ID");
            if (!_dict.ContainsKey(nameof(TravelTime.Date)))
                _dict.Add(nameof(TravelTime.Date), "Datum");
            if (!_dict.ContainsKey(nameof(TravelTime.DateD)))
                _dict.Add(nameof(TravelTime.DateD), "Datum");
            if (!_dict.ContainsKey(nameof(TravelTime.StaffId)))
                _dict.Add(nameof(TravelTime.StaffId), "Mitarbeiter-ID");
            if (!_dict.ContainsKey(nameof(TravelTime.Minutes)))
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
