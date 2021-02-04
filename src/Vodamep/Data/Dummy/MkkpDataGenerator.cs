using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vodamep.Mkkp.Model;

namespace Vodamep.Data.Dummy
{
    internal class MkkpDataGenerator : GeneratorBase
    {
        private static MkkpDataGenerator _instance;

        public static MkkpDataGenerator Instance
        {
            get
            {

                if (_instance == null)
                    _instance = new MkkpDataGenerator();

                return _instance;
            }
        }

        private MkkpDataGenerator()
        {

        }

        public MkkpReport CreateMkkpReport(int? year = null, int? month = null, int persons = 100, int staffs = 5, bool addActivities = true)
        {
            var report = new MkkpReport()
            {
                Institution = new Institution() { Id = "mkkp_test", Name = "Testverein" }
            };

            var from = year.HasValue || month.HasValue ? new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1) : DateTime.Today.FirstDateInMonth().AddMonths(-1);


            report.FromD = from;
            report.ToD = report.FromD.LastDateInMonth();

            report.AddDummyPersons(persons);
            report.AddDummyStaffs(staffs);
            report.AddDummyTravelTime();

            if (addActivities)
                report.AddDummyActivities();

            return report;
        }

        public Person CreatePerson()
        {
            var id = (_id++).ToString();

            var person = new Person()
            {
                Id = id,
                FamilyName = _familynames[_rand.Next(_familynames.Length)],
                GivenName = _names[_rand.Next(_names.Length)],
                Insurance = "19",

                CareAllowance = ((CareAllowance[])(Enum.GetValues(typeof(CareAllowance))))
                            .Where(x => x != CareAllowance.Any && x != CareAllowance.UndefinedAllowance)
                            .ElementAt(_rand.Next(Enum.GetValues(typeof(CareAllowance)).Length - 2)),

                Gender = _rand.Next(2) == 1 ? Gender.Female : Gender.Male,

                Referrer = ((Referrer[])(Enum.GetValues(typeof(Referrer))))
                            .Where(x => x != Referrer.OtherReferrer &&
                                        x != Referrer.UndefinedReferrer)
                            .ElementAt(_rand.Next(Enum.GetValues(typeof(Referrer)).Length - 2)),

                HospitalDoctor = "Dr. " + _familynames[_rand.Next(_familynames.Length)],
                LocalDoctor = "Dr. " + _familynames[_rand.Next(_familynames.Length)],
            };

            person.Diagnoses.Add(DiagnosisGroup.Premature);

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
                FamilyName = _familynames[index],
                GivenName = _names[index],
                Insurance = "19",

                CareAllowance = ((CareAllowance[])(Enum.GetValues(typeof(CareAllowance))))
                            .Where(x => x != CareAllowance.Any)
                            .ElementAt(index),

                Gender = _rand.Next(2) == 1 ? Gender.Female : Gender.Male,

                //Referrer = ((Referrer[])(Enum.GetValues(typeof(Referrer))))
                //            .Where(x => x != Referrer.OtherReferrer &&
                //                        x != Referrer.UndefinedReferrer)
                //            .ElementAt(index),

                HospitalDoctor = $"Dr. {_familynames[_rand.Next(_familynames.Length)]}",
                LocalDoctor = $"Dr. {_familynames[_rand.Next(_familynames.Length)]}",
            };

            person.Diagnoses.Add(DiagnosisGroup.Premature);


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
                yield return CreatePerson();
        }

        public Staff CreateStaff(MkkpReport report)
        {
            var id = (_id++).ToString();

            var staff = new Staff
            {
                Id = id,
                FamilyName = _familynames[_rand.Next(_familynames.Length)],
                GivenName = _names[_rand.Next(_names.Length)],
            };

            return staff;
        }

        public Staff CreateStaff(MkkpReport report, int index, int nrOfEmployments, float employment)
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

        public IEnumerable<Staff> CreateStaffs(MkkpReport report, int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreateStaff(report);
        }
        public TravelTime CreateTravelTimes(MkkpReport report)
        {
            var travelTime = new TravelTime
            {
                Id = "0",
                DateD = DateTime.Now,
                Minutes = 125,
                StaffId = report.Staffs.First().Id,
            };

            return travelTime;
        }

        private ActivityType[] CreateRandomActivities()
        {
            var a = _activities[_rand.Next(_activities.Length)];

            if (string.IsNullOrEmpty(a)) return new ActivityType[0];

            return a.Split(',').Select(x => (ActivityType)int.Parse(x)).ToArray();
        }

        private Activity CreateRandomActivity(string personId, string staffId, DateTime date)
        {
            var placeOfActionValues = Enum.GetValues(typeof(PlaceOfAction));
            var placeOfAction = (PlaceOfAction) placeOfActionValues.GetValue(_rand.Next(placeOfActionValues.Length));

            var result = new Activity()
            {
                StaffId = staffId,
                PersonId = personId,
                DateD = date,
                Minutes = _rand.Next(10, 60),
                PlaceOfAction = placeOfAction

            };

            var activities = CreateRandomActivities();
            result.Entries.AddRange(activities.OrderBy(x => x));

            return result;
        }

        public Activity[] CreateActivities(MkkpReport report)
        {
            var result = new List<Activity>();

            foreach (var staff in report.Staffs)
            {
                // die zu betreuenden Personen zufällig zuordnen
                var persons = report.Persons.Count == 1 || report.Staffs.Count == 1 ? report.Persons.ToArray() : report.Persons.Where(x => _rand.Next(report.Staffs.Count) == 0).ToArray();

                // ein Mitarbeiter soll pro Monat max. 6000 Minuten arbeiten. 
                // wenn nur wenige Personen betreut werden: max 500 Minuten pro Person
                var minuten = _rand.Next(Math.Min(persons.Count() * 500, 6000));

                while (minuten > 0)
                {
                    var personId = persons[_rand.Next(persons.Length)].Id;

                    var date = report.FromD.AddDays(_rand.Next(report.ToD.Day - report.FromD.Day + 1));

                    // Pro Tag, Person und Mitarbeiter nur ein Eintrag erlaubt:
                    if (!result.Where(x => x.PersonId == personId && x.StaffId == staff.Id && x.DateD.Equals(date)).Any())
                    {
                        var a = CreateRandomActivity(personId, staff.Id, date);

                        minuten -= a.Minutes;

                        result.Add(a);
                    }
                }
            }

            //sicherstellen, dass jede Person zumindest einen Eintrag hat
            foreach (var p in report.Persons.Where(x => !result.Where(a => a.PersonId == x.Id).Any()).ToArray())
            {
                var date = report.FromD.AddDays(_rand.Next(report.ToD.Day - report.FromD.Day + 1));

                result.Add(CreateRandomActivity(p.Id, report.Staffs[_rand.Next(report.Staffs.Count)].Id, date));
            }


            return result.ToArray();
        }
    }
}
