using System.Collections.Generic;
using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Hkpv.Model
{
    public static class HkpvReportExtensions
    {
        /// <summary>
        /// Clearing IDs auf allen Personen setzen
        /// </summary>
        public static void SetClearingIds(this HkpvReport report, ClearingExceptions clearingExceptions)
        {
            foreach (Person person in report.Persons)
            {
                person.ClearingId = person.Ssn;
                person.ClearingId = ClearingIdUtiliy.MapClearingId(clearingExceptions, person.ClearingId, report.SourceSystemId, person.Id);
            }
        }

        public static HkpvReport AddPerson(this HkpvReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static HkpvReport AddPersons(this HkpvReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));
        
        public static HkpvReport AsSorted(this HkpvReport report)
        {
            var result = new HkpvReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To,
                SourceSystemId = report.SourceSystemId,
            };

            result.Activities.AddRange(report.Activities.AsSorted());

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));
            result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));

            return result;
        }

    }
}