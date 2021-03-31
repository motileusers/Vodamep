using System;
using System.Linq;
using Vodamep.StatLp.Model;
using Attribute = System.Attribute;

namespace Vodamep.Data.Dummy
{
    internal static class StatLpDataGeneratorReportExtensions
    {
        public static Person AddDummyPerson(this StatLpReport report)
        {
            var p = StatLpDataGenerator.Instance.CreatePerson();
            report.AddPerson(p);
            return p;
        }

        public static Person[] AddDummyPersons(this StatLpReport report, int count)
        {
            var p = StatLpDataGenerator.Instance.CreatePersons(count).ToArray();
            report.AddPersons(p);
            return p;
        }

        public static Admission[] AddDummyAdmissions(this StatLpReport report)
        {
            var p = StatLpDataGenerator.Instance.CreateAdmissions(report.Persons).OrderBy(x => x.PersonId).ToArray();
            report.AddAdmissions(p);
            return p;
        }

        public static Vodamep.StatLp.Model.Attribute[] AddDummyAttributes(this StatLpReport report)
        {
            var p = StatLpDataGenerator.Instance.CreateAttributes(report.Persons).OrderBy(x => x.PersonId).ToArray();
            report.AddAttributes(p);
            return p;
        }

        public static Stay[] AddDummyStays(this StatLpReport report, DateTime from)
        {
            var p = StatLpDataGenerator.Instance.CreateStays(report.Persons, from).OrderBy(x => x.PersonId).ToArray();
            report.AddStays(p);
            return p;
        }

        public static Leaving[] AddDummyLeavings(this StatLpReport report)
        {
            var p = StatLpDataGenerator.Instance.CreateLeavings(report.Persons).OrderBy(x => x.PersonId).ToArray();
            report.AddLeavings(p);
            return p;
        }
    }
}