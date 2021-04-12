using System;
using System.Linq;
using Vodamep.Cm.Model;


namespace Vodamep.Data.Dummy
{
    internal static class CmDataGeneratorReportExtensions
    {
        public static Person AddDummyPerson(this CmReport report)
        {
            var p = CmDataGenerator.Instance.CreatePerson(1);
            report.AddPerson(p);
            return p;
        }

        public static Person[] AddDummyPersons(this CmReport report, int count)
        {
            var p = CmDataGenerator.Instance.CreatePersons(count).ToArray();
            report.AddPersons(p);
            return p;
        }

        public static Activity AddDummyActivity(this CmReport report)
        {
            var p = CmDataGenerator.Instance.CreateActivity(report.FromD);
            report.AddActivity(p);
            return p;
        }

        public static Activity[] AddDummyActivities(this CmReport report, int count)
        {
            var p = CmDataGenerator.Instance.CreateActivities(report, count).ToArray();
            report.AddActivities(p);
            return p;
        }

        public static ClientActivity AddDummyClientActivity(this CmReport report)
        {
            var p = CmDataGenerator.Instance.CreateClientActivity(report.Persons.First().Id, report.FromD);
            report.AddClientActivity(p);
            return p;
        }

        public static ClientActivity[] AddDummyClientActivities(this CmReport report, int count)
        {
            var p = CmDataGenerator.Instance.CreateClientActivities(count, report.FromD).ToArray();
            report.AddClientActivities(p);
            return p;
        }

    }
}
