using System.Collections.Generic;
using System.Linq;
using Vodamep.Tb.Validation;
using Vodamep.ValidationBase;

namespace Vodamep.Tb.Model
{
    public static class TbReportExtensions
    {
        /// <summary>
        /// Clearing IDs auf allen Personen setzen
        /// </summary>
        public static void SetClearingIds(this TbReport report, ClearingExceptions clearingExceptions)
        {
            foreach (Person person in report.Persons)
            {
                person.ClearingId = ClearingIdUtiliy.CreateClearingId(person.FamilyName, person.GivenName, person.BirthdayD);
                person.ClearingId = ClearingIdUtiliy.MapClearingId(clearingExceptions, person.ClearingId, report.SourceSystemId, person.Id);
            }
        }
        public static TbReport AddPerson(this TbReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static TbReport AddPersons(this TbReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));
        public static TbReport AddActivity(this TbReport report, Activity activity) => report.InvokeAndReturn(m => m.Activities.Add(activity));
        public static TbReport AddActivities(this TbReport report, IEnumerable<Activity> activities) => report.InvokeAndReturn(m => m.Activities.AddRange(activities));

        public static TbReportValidationResult Validate(this TbReport report) => (TbReportValidationResult)new TbReportValidator().Validate(report);

        public static string ValidateToText(this TbReport report, bool ignoreWarnings) => new TbReportValidationResultFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static IEnumerable<string> ValidateToEnumerable(this TbReport report, bool ignoreWarnings) => new TbReportValidationResultListFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static TbReport AsSorted(this TbReport report)
        {
            var result = new TbReport()
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