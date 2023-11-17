using System.Collections.Generic;
using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Model
{
    public static class MkkpReportExtensions
    {
        /// <summary>
        /// Clearing IDs auf allen Personen setzen
        /// </summary>
        public static void SetClearingIds(this MkkpReport report, ClearingExceptions clearingExceptions)
        {
            foreach (Person person in report.Persons)
            {
                person.ClearingId = ClearingIdUtiliy.CreateClearingId(person.FamilyName, person.GivenName, person.BirthdayD);
                person.ClearingId = ClearingIdUtiliy.MapClearingId(clearingExceptions, person.ClearingId, report.SourceSystemId, person.Id);
            }
        }

        public static MkkpReport AddPerson(this MkkpReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static MkkpReport AddPersons(this MkkpReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        public static MkkpReport AsSorted(this MkkpReport report)
        {
            var result = new MkkpReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To,
                SourceSystemId = report.SourceSystemId,
            };

            result.Activities.AddRange(report.Activities.AsSorted());

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));
            result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));

            result.TravelTimes.AddRange(report.TravelTimes.OrderBy(x => x.StaffId).ThenBy(x => x.DateD).ThenBy(x => x.Minutes));

            return result;
        }
    }
}