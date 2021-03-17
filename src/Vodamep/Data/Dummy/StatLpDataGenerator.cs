using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.StatLp.Model;

namespace Vodamep.Data.Dummy
{
    internal class StatLpDataGenerator : GeneratorBase
    {
        private static StatLpDataGenerator _instance;

        public static StatLpDataGenerator Instance
        {
            get
            {

                if (_instance == null)
                    _instance = new StatLpDataGenerator();

                return _instance;
            }
        }


        private StatLpDataGenerator()
        {

        }


        public StatLpReport CreateStatLpReport(string institutionId = "", int? year = null, int? month = null, int persons = 100, int staffs = 5, bool addActivities = true)
        {
            var report = new StatLpReport()
            {
                Institution = new Institution() { Id = string.IsNullOrEmpty(institutionId) ? "statlp_test" : institutionId, Name = "Testverein" }
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

        public Person CreatePerson()
        {
            var id = (_id++).ToString();

            var person = new Person()
            {
                Id = id,
                FamilyName = _familynames[_rand.Next(_familynames.Length)],
                GivenName = _names[_rand.Next(_names.Length)],
                Country = CountryCodeProvider.Instance.Values.Values.ToArray()[_rand.Next(CountryCodeProvider.Instance.Values.Keys.Count())],
                Gender = _rand.Next(2) == 1 ? Gender.Female : Gender.Male,
            };

            person.BirthdayD  = new DateTime(1920, 01, 01).AddDays(_rand.Next(20000));

            return person;

        }

        public Person CreatePerson(int index)
        {
            var person = new Person()
            {
                Id = index.ToString(),
              
                Gender = _rand.Next(2) == 1 ? Gender.Female : Gender.Male,


            };


            return person;
        }

        public IEnumerable<Person> CreatePersons(int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreatePerson();
        }

    }
}