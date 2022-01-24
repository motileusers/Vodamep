using System.Collections.Generic;
using System.Linq;
using Vodamep.ValidationBase;

namespace Vodamep.Cm.Model
{
    public static class CmReportExtensions
    {
        /// <summary>
        /// Clearing IDs auf allen Personen setzen
        /// </summary>
        public static void SetClearingIds(this CmReport report, ClearingExceptions clearingExceptions)
        {
            foreach (Person person in report.Persons)
            {
                person.ClearingId = ClearingIdUtiliy.CreateClearingId(person.FamilyName, person.GivenName, person.BirthdayD);
                person.ClearingId = ClearingIdUtiliy.MapClearingId(clearingExceptions, person.ClearingId, report.SourceSystemId, person.Id);
            }
        }

        public static CmReport AddPerson(this CmReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static CmReport AddPersons(this CmReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        public static CmReport AddActivity(this CmReport report, Activity activity) => report.InvokeAndReturn(m => m.Activities.Add(activity));
        public static CmReport AddActivities(this CmReport report, IEnumerable<Activity> activities) => report.InvokeAndReturn(m => m.Activities.AddRange(activities));

        public static CmReport AddClientActivity(this CmReport report, ClientActivity clientActivity) => report.InvokeAndReturn(m => m.ClientActivities.Add(clientActivity));
        public static CmReport AddClientActivities(this CmReport report, IEnumerable<ClientActivity> clientActivities) => report.InvokeAndReturn(m => m.ClientActivities.AddRange(clientActivities));

        
        public static CmReport AsSorted(this CmReport report)
        {
            var result = new CmReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To,
                SourceSystemId = report.SourceSystemId,
            };

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));

            result.Activities.AddRange(report.Activities.OrderBy(x => x.DateD)
                                                        .ThenBy(x => x.ActivityType)
                                                        .ThenBy(x => x.Minutes));

            result.ClientActivities.AddRange(report.ClientActivities.OrderBy(x => x.DateD)
                                                                    .ThenBy(x => x.PersonId)
                                                                    .ThenBy(x => x.ActivityType)
                                                                    .ThenBy(x => x.Minutes));


            return result;
        }

    }
}