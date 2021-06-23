using System;
using System.Linq;
using Vodamep.Mkkp.Model;

namespace Vodamep.Data.Dummy
{
    internal static class MkkpDataGeneratorMkkpReportExtensions
    {
        public static Person AddDummyPerson(this MkkpReport report)
        {
            var p = MkkpDataGenerator.Instance.CreatePerson();
            report.AddPerson(p);
            return p;
        }

        public static Person[] AddDummyPersons(this MkkpReport report, int count)
        {
            var p = MkkpDataGenerator.Instance.CreatePersons(count).ToArray();
            report.AddPersons(p);
            return p;
        }

        public static Staff AddDummyStaff(this MkkpReport report, bool useRandomValues)
        {
            var s = MkkpDataGenerator.Instance.CreateStaff(report, null, useRandomValues);

            report.Staffs.Add(s);
            return s;
        }

        public static Staff[] AddDummyStaffs(this MkkpReport report, int count, bool useRandomValues)
        {
            var s = MkkpDataGenerator.Instance.CreateStaffs(report, count, useRandomValues).ToArray();
            report.Staffs.AddRange(s);
            return s;
        }

        public static TravelTime AddDummyTravelTime(this MkkpReport report)
        {
            var tt = MkkpDataGenerator.Instance.CreateTravelTimes(report);
            report.TravelTimes.Add(tt);
            return tt;
        }

        public static Activity[] AddDummyActivities(this MkkpReport report)
        {
            if (report.FromD == null)
                report.FromD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);

            if (report.ToD == null)
                report.ToD = report.FromD.LastDateInMonth();

            if (report.Staffs.Count == 0)
                report.AddDummyStaff(true);

            if (report.Persons.Count == 0)
                report.AddDummyPerson();

            var a = MkkpDataGenerator.Instance.CreateActivities(report);
            report.Activities.AddRange(a);
            return a;
        }

    }
}