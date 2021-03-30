using System;
using System.Collections.Generic;
using Vodamep.Cm.Model;

namespace Vodamep.Data.Dummy
{
    internal class CmDataGenerator : GeneratorBase
    {
        private static CmDataGenerator _instance;

        public static CmDataGenerator Instance
        {
            get
            {

                if (_instance == null)
                    _instance = new CmDataGenerator();

                return _instance;
            }
        }


        private CmDataGenerator()
        {

        }


        public CmReport CreateCmReport(string institutionId = "", int? year = null, int? month = null, int persons = 100, int staffs = 5, bool addActivities = true)
        {
            var report = new CmReport()
            {

                Institution = new Institution() { Id = string.IsNullOrEmpty(institutionId) ? "cm_test" : institutionId, Name = "Testverein" }
            };

            var from = year.HasValue || month.HasValue ? new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1) : DateTime.Today.FirstDateInMonth().AddMonths(-1);


            report.FromD = from;
            report.ToD = report.FromD.LastDateInMonth();

            report.AddDummyPersons(persons);
            //report.AddDummyStaffs(staffs);
            //report.AddDummyTravelTime();

            //if (addActivities)
            //    report.AddDummyActivities();

            return report;
        }

        public Person CreatePerson(int index)
        {
            var person = new Person()
            {
                Id = index.ToString(),
                FamilyName = _familynames[index],
                GivenName = _names[index],
                CareAllowance = CareAllowance.L4,
                Gender = index % 2 == 1 ? Gender.InterGe : Gender.MaleGe,
            };

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
                yield return CreatePerson(i + 1);
        }


        //private ActivityType[] CreateRandomActivities()
        //{
        //    var a = _activities[_rand.Next(_activities.Length)];

        //    if (string.IsNullOrEmpty(a)) return new ActivityType[0];

        //    return a.Split(',').Select(x => (ActivityType)int.Parse(x)).Distinct().ToArray();
        //}

        //private Activity CreateRandomActivity(string personId, string staffId, DateTime date, int minuten)
        //{
        //    var result = new Activity()
        //    {
        //        StaffId = staffId,
        //        PersonId = personId,
        //        DateD = date,
        //        PlaceOfAction = ((PlaceOfAction[])(Enum.GetValues(typeof(PlaceOfAction))))
        //        .Where(x => x != PlaceOfAction.UndefinedPlace )
        //        .ElementAt(_rand.Next(Enum.GetValues(typeof(PlaceOfAction)).Length - 1))
        //    };

        //    result.Minutes = minuten;

        //    var activities = CreateRandomActivities();
        //    result.Entries.AddRange(activities.OrderBy(x => x));

        //    return result;
        //}

        public Activity[] CreateActivities(CmReport report)
        {
            var result = new List<Activity>();

            return result.ToArray();
        }
    }
}
