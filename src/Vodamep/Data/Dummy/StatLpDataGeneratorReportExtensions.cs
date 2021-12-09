using System;
using System.Linq;
using Vodamep.StatLp.Model;

namespace Vodamep.Data.Dummy
{
    internal static class StatLpDataGeneratorReportExtensions
    {
        public static Person AddDummyPerson(this StatLpReport report, int id = -1, bool randomValues = true)
        {
            var p = StatLpDataGenerator.Instance.CreatePerson(id, randomValues);
            report.AddPerson(p);
            return p;
        }

        public static Person[] AddDummyPersons(this StatLpReport report, int count)
        {
            var p = StatLpDataGenerator.Instance.CreatePersons(count).ToArray();
            report.AddPersons(p);
            return p;
        }

        public static Admission AddDummyAdmission(this StatLpReport report)
        {
            var p = StatLpDataGenerator.Instance.CreateAdmission(report.Persons.First().Id, report.FromD);
            report.AddAdmission(p);
            return p;
        }

        public static void AddDummyAdmissions(this StatLpReport report)
        {
            foreach (var person in report.Persons)
            {
                var firstStay = report.Stays.Where(x => x.PersonId == person.Id).OrderBy(x => x.From).FirstOrDefault();
                var a = StatLpDataGenerator.Instance.CreateAdmission(person.Id, firstStay?.FromD);
                report.AddAdmission(a);
            }
        }

        public static void AddDummyAttributes(this StatLpReport report)
        {
            foreach (var person in report.Persons)
            {
                var firstStay = report.Stays.Where(x => x.PersonId == person.Id).OrderBy(x => x.From).FirstOrDefault();

                var date = firstStay?.FromD ?? report.FromD;

                var a = StatLpDataGenerator.Instance.CreateAttributes(person.Id, date >= report.FromD ? date : report.FromD);
                report.AddAttributes(a);
            }
        }
        public static Stay AddDummyStay(this StatLpReport report, DateTime from)
        {
            var p = StatLpDataGenerator.Instance.CreateStay(report.Persons.First().Id, from);
            report.AddStay(p);
            return p;
        }

        public static Stay[] AddDummyStays(this StatLpReport report, DateTime from)
        {
            var p = StatLpDataGenerator.Instance.CreateStays(report.Persons, from).OrderBy(x => x.PersonId).ToArray();
            report.AddStays(p);
            return p;
        }

        public static Leaving AddDummyLeaving(this StatLpReport report)
        {
            var p = StatLpDataGenerator.Instance.CreateLeaving(report.Persons.First().Id, report.ToD);
            report.AddLeaving(p);
            return p;
        }

        public static Leaving[] AddDummyLeavings(this StatLpReport report)
        {
            var p = StatLpDataGenerator.Instance.CreateLeavings(report.Persons, report.ToD).OrderBy(x => x.PersonId).ToArray();
            report.AddLeavings(p);
            return p;
        }
    }
}