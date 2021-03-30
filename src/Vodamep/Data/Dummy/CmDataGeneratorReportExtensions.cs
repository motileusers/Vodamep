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

    
        public static Activity[] AddDummyActivities(this CmReport report)
        {
            if (report.FromD == null)
                report.FromD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            if (report.ToD == null)
                report.ToD = report.FromD.LastDateInMonth();

      

            if (report.Persons.Count == 0)
                report.AddDummyPerson();

            var a = CmDataGenerator.Instance.CreateActivities(report);
            report.Activities.AddRange(a);
            return a;
        }

    }
}
