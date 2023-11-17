using System.Collections.Generic;
using System.Linq;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Model
{
    public static class AgpReportExtensions
    {

        /// <summary>
        /// Clearing IDs auf allen Personen setzen
        /// </summary>
        public static void SetClearingIds(this AgpReport report, ClearingExceptions clearingExceptions)
        {
            foreach (Person person in report.Persons)
            {
                person.ClearingId = ClearingIdUtiliy.CreateClearingId(person.FamilyName, person.GivenName, person.BirthdayD);
                person.ClearingId = ClearingIdUtiliy.MapClearingId(clearingExceptions, person.ClearingId, report.SourceSystemId, person.Id);
            }
        }


        public static AgpReport AddPerson(this AgpReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static AgpReport AddPersons(this AgpReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        public static AgpReport AsSorted(this AgpReport report)
        {
            var result = new AgpReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To,
                SourceSystemId = report.SourceSystemId,
            };

            result.Activities.AddRange(report.Activities.AsSorted());
            result.StaffActivities.AddRange(report.StaffActivities.AsSorted());

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));

            return result;
        }


    }
}