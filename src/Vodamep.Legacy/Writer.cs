using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vodamep.Data;
using Vodamep.Data.Hkpv;
using Vodamep.Hkpv.Model;
using Vodamep.Legacy.Reader;

namespace Vodamep.Legacy
{
    public class Writer
    {
        Dictionary<string, string> unknonwStaffIds = new Dictionary<string, string>();

        public string Write(string path, ReadResult data, bool asJson = false)
        {
            var date = data.L.Select(x => x.Datum).First().FirstDateInMonth();
            var report = new HkpvReport()
            {
                FromD = date,
                ToD = date.LastDateInMonth(),
                Institution = new Institution() { Id = data.V.Vereinsnummer, Name = data.V.Bezeichnung }
            };
            foreach (var a in data.A)
            {
                var name = (Familyname: a.Name_1, Givenname: a.Name_2);
                if (string.IsNullOrEmpty(name.Givenname)) name = GetName(a.Name_1);

                var (Postcode, City) = GetPostCodeCity(a);

                report.AddPerson(new Person()
                {
                    Id = GetId(a.Adressnummer),
                    BirthdayD = a.Geburtsdatum,
                    FamilyName = (name.Familyname ?? string.Empty).Trim(),
                    GivenName = (name.Givenname ?? string.Empty).Trim(),
                    Ssn = (a.Versicherungsnummer ?? string.Empty).Trim(),
                    Insurance = (a.Versicherung ?? string.Empty).Trim(),
                    Nationality = (a.Staatsbuergerschaft ?? string.Empty).Trim(),
                    CareAllowance = (CareAllowance)a.Pflegestufe,
                    Postcode = Postcode,
                    City = City,
                    Gender = GetGender(a.Geschlecht)
                });
            }

            foreach (var p in data.P)
            {
                var (Familyname, Givenname) = GetName(p.Pflegername);

                Staff staff = new Staff()
                {
                    Id = p.Pflegernummer.ToString(), 
                    FamilyName = Familyname,
                    GivenName = Givenname,
                    Qualification = p.Berufstitel
                };


                var anstellungen = data.S.Where(x => x.Pflegernummer == p.Pflegernummer).OrderBy(x => x.Von.GetValueOrDefault(DateTime.MinValue));
                foreach (var anstellung in anstellungen)
                {
                    if (anstellung.Von == null)
                        anstellung.Von = DateTime.MinValue;

                    if (anstellung.Bis == null)
                        anstellung.Bis = DateTime.MaxValue;


                    if (anstellung.Von < date)
                        anstellung.Von = date;

                    if (anstellung.Bis > date.LastDateInMonth())
                        anstellung.Bis = date.LastDateInMonth();


                    Debug.WriteLine(anstellung.Von + " " + anstellung.Bis);


                    Employment employment = new Employment()
                    {
                        FromD = anstellung.Von,
                        ToD = anstellung.Bis,
                        HoursPerWeek = 40 * anstellung.VZAE,
                    };

                    staff.Employments.Add(employment);
                }



                report.Staffs.Add(staff);
            }

            foreach (var l in data.L.GroupBy(x => new { x.Datum, x.Adressnummer, x.Pfleger }))
            {
                var a = new Activity()
                {
                    StaffId = l.Key.Pfleger.ToString(),
                    DateD = l.Key.Datum
                };
                foreach (var entry in l.OrderBy(x => x.Leistung))
                {
                    var t = (ActivityType)entry.Leistung;
                    for (var i = 0; i < entry.Anzahl; i++)
                        a.Entries.Add(t);
                }

                if (a.RequiresPersonId())
                {
                    a.PersonId = GetId(l.Key.Adressnummer);
                }
                report.Activities.Add(a);
            }

            var filename = report.AsSorted().WriteToPath(path, asJson: asJson, compressed: false);

            if (unknonwStaffIds.Count > 0)
                Console.WriteLine($"Unbekannte PflegerIds: {String.Join(", ", unknonwStaffIds.Keys.ToArray())}");
            return filename;
        }


        private Gender GetGender(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Gender.UndefinedGender;

            switch (value.ToLower().Substring(0, 1))
            {
                case "m":
                    return Gender.Male;
                case "w":
                case "f":
                    return Gender.Female;

                default:
                    return Gender.UndefinedGender;
            }
        }

        private (string Familyname, string Givenname) GetName(string name)
        {
            var names = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            while (names.Count < 2)
                names.Add("- - -");

            return (names[0], string.Join(" ", names.Skip(1)));

        }

        private string GetId(int id) => $"{id}";

        private (string Postcode, string City) GetPostCodeCity(Model.AdresseDTO a)
        {

            var plz = (a.Postleitzahl ?? string.Empty).Trim();
            var ort = (a.Ort ?? string.Empty).Trim();

            if (plz.Length > 4)
            {
                // im Datenbestand gibt es "gebastelte" Postleitzahlen mit 6 Zeichen
                // z.b. 690060 Möggers
                // entweder nur die ersten vier Zeichen verwenden, oder die PLZ anhand des Ortsnames ermitteln

                if (Data.PostcodeCityProvider.Instance.IsValid($"{plz.Substring(0, 4)} {ort}"))
                {
                    plz = plz.Substring(0, 4);
                }
                else
                {
                    var plz2 = Data.PostcodeCityProvider.Instance.GetCSV().Where(x => x.EndsWith($" {ort};")).FirstOrDefault();
                    if (!string.IsNullOrEmpty(plz2))
                        plz = plz2;
                }
            }

            return (plz, ort);
        }
    }
}
