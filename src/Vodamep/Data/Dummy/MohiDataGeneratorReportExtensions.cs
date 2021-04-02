using System;
using System.Linq;
using Vodamep.Mohi.Model;


namespace Vodamep.Data.Dummy
{
    internal static class MohiDataGeneratorReportExtensions
    {
        public static Person AddDummyPerson(this MohiReport report)
        {
            var p = MohiDataGenerator.Instance.CreatePerson(1);
            report.AddPerson(p);
            return p;
        }

        public static Person[] AddDummyPersons(this MohiReport report, int count)
        {
            var p = MohiDataGenerator.Instance.CreatePersons(count).ToArray();
            report.AddPersons(p);
            return p;
        }

        public static Activity AddDummyActivity(this MohiReport report)
        {
            var p = MohiDataGenerator.Instance.CreateActivity(report.FromD);
            report.AddActivity(p);
            return p;
        }

        public static Activity[] AddDummyActivities(this MohiReport report, int count)
        {
            var p = MohiDataGenerator.Instance.CreateActivities(report, count).ToArray();
            report.AddActivities(p);
            return p;
        }

    }
}
