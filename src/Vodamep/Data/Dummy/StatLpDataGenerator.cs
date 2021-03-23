using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
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
                Institution = new Institution() { Id = string.IsNullOrEmpty(institutionId) ? "1234" : institutionId, Name = "Testverein" }
            };

            var from = year.HasValue || month.HasValue ? new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1) : DateTime.Today.FirstDateInMonth().AddMonths(-1);


            report.FromD = from;
            report.ToD = report.FromD.LastDateInMonth();


            report.AddDummyPersons(persons);
            report.AddDummyAdmissions();
            //report.AddDummyTravelTime();

            //if (addActivities)
            //    report.AddDummyActivities();

            return report;
        }

        public Person CreatePerson()
        {
            return this.CreatePerson(_id++);
        }

        public Person CreatePerson(long index)
        {
            var person = new Person()
            {
                Id = index.ToString(),
                FamilyName = _familynames[_rand.Next(_familynames.Length)],
                GivenName = _names[_rand.Next(_names.Length)],
                Country = CountryCodeProvider.Instance.Values.Values.ToArray()[_rand.Next(CountryCodeProvider.Instance.Values.Keys.Count())],
                Gender = ((Gender[])(Enum.GetValues(typeof(Gender))))
                    .Where(x => x != Gender.UndefinedGe)
                    .ElementAt(_rand.Next(Enum.GetValues(typeof(Gender)).Length - 1)),

            };

            person.BirthdayD = new DateTime(1920, 01, 01).AddDays(_rand.Next(20000));

            return person;
        }

        public IEnumerable<Person> CreatePersons(int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreatePerson(i + 1);
        }

        public Admission CreateAdmission(string personId)
        {
            var admission = new Admission()
            {
                PersonId = personId,

                LastPostcode = "6800",
                LastCity = "Feldkirch",

                HousingTypeBeforeAdmission = ((AdmissionLocation[])Enum.GetValues(typeof(AdmissionLocation)))
                    .Where(x => x != AdmissionLocation.UndefinedAl)
                    .ElementAt(_rand.Next(Enum.GetValues(typeof(AdmissionLocation)).Length - 1)),

                MainAttendanceRelation = ((MainAttendanceRelation[])Enum.GetValues(typeof(MainAttendanceRelation)))
                    .Where(x => x != MainAttendanceRelation.UndefinedMr)
                    .ElementAt(_rand.Next(Enum.GetValues(typeof(MainAttendanceRelation)).Length - 1)),

                MainAttendanceCloseness = ((MainAttendanceCloseness[])Enum.GetValues(typeof(MainAttendanceCloseness)))
                    .Where(x => x != MainAttendanceCloseness.UndefinedMc)
                    .ElementAt(_rand.Next(Enum.GetValues(typeof(MainAttendanceCloseness)).Length - 1)),

                HousingReason = ((HousingReason[])Enum.GetValues(typeof(HousingReason)))
                    .Where(x => x != HousingReason.UndefinedHr)
                    .ElementAt(_rand.Next(Enum.GetValues(typeof(HousingReason)).Length - 1)),

                PersonalChanges = { PersonalChange.IncreasedAssitanceNeedPc },

                SocialChanges = { SocialChange.MissingMealsSc }
            };

            return admission;

        }

        public IEnumerable<Admission> CreateAdmissions(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                yield return CreateAdmission(person.Id);
            }

        }

    }
}