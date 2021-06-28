using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vodamep.Agp.Model;

namespace Vodamep.Data.Dummy
{
    internal class AgpDataGenerator : GeneratorBase
    {
        private static AgpDataGenerator _instance;

        public static AgpDataGenerator Instance
        {
            get
            {

                if (_instance == null)
                    _instance = new AgpDataGenerator();

                return _instance;
            }
        }


        private AgpDataGenerator()
        {

        }


        public AgpReport CreateAgpReport(string institutionId = "", int? year = null, int? month = null, int persons = 100, int staffs = 5, bool useRandomValues = true, bool addActivities = true)
        {
            var report = new AgpReport()
            {

                Institution = new Institution() { Id = string.IsNullOrEmpty(institutionId) ? "agp_test" : institutionId, Name = "Testverein" }
            };

            var from = year.HasValue || month.HasValue ? new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1) : DateTime.Today.FirstDateInMonth().AddMonths(-1);


            report.FromD = from;
            report.ToD = report.FromD.LastDateInMonth();

            report.AddDummyPersons(persons);
            report.AddDummyStaffs(staffs, useRandomValues);
            report.AddDummyTravelTime(from);

            if (addActivities)
                report.AddDummyActivities();

            return report;
        }

        public Person CreatePerson(string id = null)
        {
            id = id ?? (_id++).ToString();

            var person = new Person()
            {
                Id = id,
                Insurance = "19",

                CareAllowance = ((CareAllowance[])(Enum.GetValues(typeof(CareAllowance))))
                            .Where(x => x != CareAllowance.Any && x != CareAllowance.UndefinedAllowance)
                            .ElementAt(_rand.Next(Enum.GetValues(typeof(Referrer)).Length - 2)),

                Gender = _rand.Next(2) == 1 ? Gender.Female : Gender.Male,

                Referrer = ((Referrer[])(Enum.GetValues(typeof(Referrer))))
                            .Where(x => x != Referrer.OtherReferrer &&
                                        x != Referrer.UndefinedReferrer)
                            .ElementAt(_rand.Next(Enum.GetValues(typeof(Referrer)).Length - 2)),

                HospitalDoctor = "Dr. " + _familynames[_rand.Next(_familynames.Length)],
                LocalDoctor = "Dr. " + _familynames[_rand.Next(_familynames.Length)],
            };

            person.Diagnoses.Add(DiagnosisGroup.AffectiveDisorder);

            // die Anschrift
            {
                var address = _addresses[_rand.Next(_addresses.Length)].Split(';');

                person.Postcode = address[6];
                person.City = address[3];
            }

            person.BirthdayD = new DateTime(1920, 01, 01).AddDays(_rand.Next(20000));

            return person;

        }

        public Person CreatePerson(int index)
        {
            var person = new Person()
            {
                Id = index.ToString(),
                Insurance = "19",

                CareAllowance = ((CareAllowance[])(Enum.GetValues(typeof(CareAllowance))))
                            .Where(x => x != CareAllowance.Any)
                            .ElementAt(index),

                Gender = _rand.Next(2) == 1 ? Gender.Female : Gender.Male,

                //Referrer = ((Referrer[])(Enum.GetValues(typeof(Referrer))))
                //            .Where(x => x != Referrer.OtherReferrer &&
                //                        x != Referrer.UndefinedReferrer)
                            //.ElementAt(index),

                HospitalDoctor = "Dr. " + _familynames[index],
                LocalDoctor = "Dr. " + _familynames[index],
            };

            person.Diagnoses.Add(DiagnosisGroup.AffectiveDisorder);


            // die Anschrift
            {
                var address = _addresses[index].Split(';');

                person.Postcode = address[6];
                person.City = address[3];
            }

            person.BirthdayD = new DateTime(1920, 01, 01).AddDays(index);

            return person;
        }

        public IEnumerable<Person> CreatePersons(int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreatePerson((i+1).ToString());
        }

        public Staff CreateStaff(AgpReport report, bool useRandomValues = true)
        {
            var id = (_id++).ToString();

            var staff = new Staff
            {
                Id = id,
                FamilyName = useRandomValues ? _familynames[_rand.Next(_familynames.Length)]: _familynames[0],
                GivenName = useRandomValues ? _names[_rand.Next(_names.Length)] : _names[0],
            };

            return staff;
        }

        public Staff CreateStaff(AgpReport report, int index, int nrOfEmployments, float employment)
        {
            var id = index.ToString();

            var staff = new Staff
            {
                Id = id,
                FamilyName = _familynames[index],
                GivenName = _names[index],
            };

            return staff;
        }

        public IEnumerable<Staff> CreateStaffs(AgpReport report, int count, bool useRandomValues = true)
        {
            for (var i = 0; i < count; i++)
                yield return CreateStaff(report, useRandomValues);
        }

        public TravelTime CreateTravelTimes (AgpReport report, DateTime from)
        {
            var travelTime = new TravelTime
            {
                Id = "0",
                DateD = from,
                Minutes = 125,
                StaffId = report.Staffs.First().Id,
            };

            return travelTime;
        }

        private ActivityType[] CreateRandomActivities()
        {
            var a = _activities[_rand.Next(_activities.Length)];

            if (string.IsNullOrEmpty(a)) return new ActivityType[0];

            return a.Split(',').Select(x => (ActivityType)int.Parse(x)).Distinct().ToArray();
        }

        private Activity CreateRandomActivity(string personId, string staffId, DateTime date, int minuten)
        {
            var result = new Activity()
            {
                StaffId = staffId,
                PersonId = personId,
                DateD = date,
                PlaceOfAction = ((PlaceOfAction[])(Enum.GetValues(typeof(PlaceOfAction))))
                .Where(x => x != PlaceOfAction.UndefinedPlace )
                .ElementAt(_rand.Next(Enum.GetValues(typeof(PlaceOfAction)).Length - 1))
            };

            result.Minutes = minuten;

            var activities = CreateRandomActivities();
            result.Entries.AddRange(activities.OrderBy(x => x));

            return result;
        }

        public Activity[] CreateActivities(AgpReport report)
        {
            var result = new List<Activity>();

            foreach (var staff in report.Staffs)
            {
                // die zu betreuenden Personen zufällig zuordnen
                var persons = report.Persons.Count == 1 || report.Staffs.Count == 1 ? report.Persons.ToArray() : report.Persons.Where(x => _rand.Next(report.Staffs.Count) == 0).ToArray();

                // ein Mitarbeiter soll pro Tag max 10h arbeiten und das nur in 5 Minuten Schritten
                // hier wurde die hälfte gewählt, damit für den nächsten Schritt noch genug puffer ist
                var minuten = _rand.Next(Math.Min(persons.Count() * 1, 100 / 5)) * 5;

                while (minuten > 0)
                {
                    var personId = persons[_rand.Next(persons.Length)].Id;

                    var date = report.FromD.AddDays(_rand.Next(report.ToD.Day - report.FromD.Day + 1));

                    // Pro Tag, Person und Mitarbeiter nur ein Eintrag erlaubt:
                    if (!result.Where(x => x.PersonId == personId && x.StaffId == staff.Id && x.DateD.Equals(date)).Any())
                    {
                        var a = CreateRandomActivity(personId, staff.Id, date, minuten);

                        minuten -= a.Minutes;

                        result.Add(a);
                    }
                }
            }

            //sicherstellen, dass jede Person zumindest einen Eintrag hat
            foreach (var p in report.Persons.Where(x => !result.Where(a => a.PersonId == x.Id).Any()).ToArray())
            {
                var date = report.FromD.AddDays(_rand.Next(report.ToD.Day - report.FromD.Day + 1));

                result.Add(CreateRandomActivity(p.Id, report.Staffs[_rand.Next(report.Staffs.Count)].Id, date, 5));
            }


            return result.ToArray();
        }
    }
}
