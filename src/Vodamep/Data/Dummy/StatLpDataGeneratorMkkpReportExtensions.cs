using System;
using System.Linq;
using Vodamep.StatLp.Model;

namespace Vodamep.Data.Dummy
{
    internal static class StatLpDataGeneratorMkkpReportExtensions
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
    }
}