using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Vodamep.Cm.Model;
using Enum = System.Enum;

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
            report.AddDummyActivity();
            report.AddDummyClientActivity();

            return report;
        }

        public Person CreatePerson(int index)
        {
            var person = new Person()
            {
                Id = index.ToString(),
                FamilyName = _familynames[index],
                GivenName = _names[index],
                CareAllowance = ((CareAllowance[])(Enum.GetValues(typeof(CareAllowance))))
                    .Where(x => x != CareAllowance.UndefinedAllowance)
                    .ElementAt(_rand.Next(Enum.GetValues(typeof(CareAllowance)).Length - 1)),
                Country = "AT",
                Gender = Gender.MaleGe,
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

        public ClientActivity CreateClientActivity(string personId, DateTime reportDate)
        {
            var clientActivity = new ClientActivity
            {
               PersonId = personId,
               Minutes = 500,
               Date = reportDate.AddDays(1).AsTimestamp(),
               ActivityType = ((ClientActivityType[])(Enum.GetValues(typeof(ClientActivityType))))
                   .Where(x => x != ClientActivityType.UndefinedCa)
                   .ElementAt(_rand.Next(Enum.GetValues(typeof(ClientActivityType)).Length - 1)),

            };

            return clientActivity;
        }

        public IEnumerable<ClientActivity> CreateClientActivities(int count, DateTime reportDate)
        {
            for (var i = 0; i < count; i++)
                yield return CreateClientActivity((i + 1).ToString(), reportDate);
        }

        public Activity CreateActivity(DateTime reportDate)
        {
            var clientActivity = new Activity
            {
                Minutes = 500,
                Date = reportDate.AddDays(1).AsTimestamp(),
                ActivityType = ((ActivityType[])(Enum.GetValues(typeof(ActivityType))))
                    .Where(x => x != ActivityType.UndefinedCt)
                    .ElementAt(_rand.Next(Enum.GetValues(typeof(ActivityType)).Length - 1)),
            };

            return clientActivity;
        }



        public IEnumerable<Activity> CreateActivities(CmReport report, int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreateActivity(report.FromD);
        }
    }
}
