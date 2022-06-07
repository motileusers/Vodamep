using System;
using System.Linq;
using Vodamep.Agp.Model;


namespace Vodamep.Data.Dummy
{
    internal static class AgpDataGeneratorAgpReportExtensions
    {
        public static Person AddDummyPerson(this AgpReport report)
        {
            var p = AgpDataGenerator.Instance.CreatePerson();
            report.AddPerson(p);
            return p;
        }

        public static Person[] AddDummyPersons(this AgpReport report, int count)
        {
            var p = AgpDataGenerator.Instance.CreatePersons(count).ToArray();
            report.AddPersons(p);
            return p;
        }

        public static StaffActivity[] AddDummyStaffActivities(this AgpReport report)
        {
            if (report.FromD == null)
                report.FromD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            if (report.ToD == null)
                report.ToD = report.FromD.LastDateInMonth();

            if (report.Persons.Count == 0)
                report.AddDummyPerson();

            var a = AgpDataGenerator.Instance.CreateStaffActivities(report);
            report.StaffActivities.AddRange(a);
            return a;
        }

        public static Activity[] AddDummyActivities(this AgpReport report)
        {
            if (report.FromD == null)
                report.FromD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            if (report.ToD == null)
                report.ToD = report.FromD.LastDateInMonth();

            if (report.Persons.Count == 0)
                report.AddDummyPerson();

            var a = AgpDataGenerator.Instance.CreateActivities(report);
            report.Activities.AddRange(a);
            return a;
        }

    }
}
