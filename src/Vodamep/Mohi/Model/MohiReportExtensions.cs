using System.Collections.Generic;
using System.Linq;
using Vodamep.ValidationBase;

namespace Vodamep.Mohi.Model
{
    public static class MohiReportExtensions
    {
        /// <summary>
        /// Clearing IDs auf allen Personen setzen
        /// </summary>
        public static void SetClearingIds(this MohiReport report, ClearingExceptions clearingExceptions)
        {
            foreach (Person person in report.Persons)
            {
                person.ClearingId = ClearingIdUtiliy.CreateClearingId(person.FamilyName, person.GivenName, person.BirthdayD);
                person.ClearingId = ClearingIdUtiliy.MapClearingId(clearingExceptions, person.ClearingId, report.SourceSystemId, person.Id);
            }
        }


        public static MohiReport AddPerson(this MohiReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static MohiReport AddPersons(this MohiReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        public static MohiReport AddActivity(this MohiReport report, Activity person) => report.InvokeAndReturn(m => m.Activities.Add(person));
        public static MohiReport AddActivities(this MohiReport report, IEnumerable<Activity> persons) => report.InvokeAndReturn(m => m.Activities.AddRange(persons));

        public static MohiReport AsSorted(this MohiReport report)
        {
            var result = new MohiReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To,
                SourceSystemId = report.SourceSystemId,
            };

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));

            result.Activities.AddRange(report.Activities.OrderBy(x => x.PersonId).ThenBy(x => x.HoursPerMonth));

            return result;
        }

    }
}