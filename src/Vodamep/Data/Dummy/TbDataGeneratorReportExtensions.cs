using System;
using System.Linq;
using Vodamep.Tb.Model;


namespace Vodamep.Data.Dummy
{
    internal static class TbDataGeneratorReportExtensions
    {
        public static Person AddDummyPerson(this TbReport report)
        {
            var p = TbDataGenerator.Instance.CreatePerson(1);
            report.AddPerson(p);
            return p;
        }

        public static Person[] AddDummyPersons(this TbReport report, int count)
        {
            var p = TbDataGenerator.Instance.CreatePersons(count).ToArray();
            report.AddPersons(p);
            return p;
        }

        public static Activity AddDummyActivity(this TbReport report)
        {
            var p = TbDataGenerator.Instance.CreateActivity(report.Persons.First().Id);
            report.AddActivity(p);
            return p;
        }

        public static Activity[] AddDummyActivities(this TbReport report, int count)
        {
            var p = TbDataGenerator.Instance.CreateActivities(report, count).ToArray();
            report.AddActivities(p);
            return p;
        }

    }
}
