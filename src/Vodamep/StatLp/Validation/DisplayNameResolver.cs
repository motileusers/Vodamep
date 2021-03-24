using System.Collections.Generic;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
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

            _dict.Add(nameof(StatLpReport.To), "Bis");
            _dict.Add(nameof(StatLpReport.ToD), "Bis");

            _dict.Add(nameof(StatLpReport.From), "Von");
            _dict.Add(nameof(StatLpReport.FromD), "Von");

            _dict.Add(nameof(StatLpReport.Institution), "Einrichtung");
           
            _dict.Add(nameof(Person.FamilyName), "Familienname");
            _dict.Add(nameof(Person.GivenName), "Vorname");
            _dict.Add(nameof(Person.Gender), "Geschlecht");
            _dict.Add(nameof(Person.Country), "Land");
            _dict.Add(nameof(Person.Birthday), "Geburtsdatum");
            _dict.Add(nameof(Person.BirthdayD), "Geburtsdatum");
           
            _dict.Add(nameof(Admission.HousingTypeBeforeAdmission), "Wohnsituation vor der Aufnahme");
            _dict.Add(nameof(Admission.MainAttendanceRelation), "Verwandtschaftsverhältnis Hauptbetreuungspers.");
            _dict.Add(nameof(Admission.MainAttendanceCloseness), "Räumliche Nähe Hauptbetreuungsperson");
            _dict.Add(nameof(Admission.HousingReason), "Wohnraumsituations- und Ausstattungsgründe");
            _dict.Add(nameof(Admission.OtherHousingType), "Sonstige Lebens-/Betreuungssituation");
            _dict.Add(nameof(Admission.PersonalChanges), "Veränderungen persönliche Situation");
            _dict.Add(nameof(Admission.PersonalChangeOther), "Veränderungen persönliche Situation");
            _dict.Add(nameof(Admission.SocialChanges), "Veränderungen nicht bewältigt, weil");
            _dict.Add(nameof(Admission.SocialChangeOther), "Veränderungen nicht bewältigt, weil");
            _dict.Add(nameof(Admission.HousingReasonOther), "Wohnraumsituations- und Ausstattungsgründe");
        }

        public string GetDisplayName(string name)
        {            
            if (!string.IsNullOrEmpty(name) && _dict.TryGetValue(name, out string value))
                return value;

            return name;
        }

    }
}
