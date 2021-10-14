using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodamep.ReportBase;
using Vodamep.Tb.Validation;

namespace Vodamep.Tb.Model
{
    public static class TbReportExtensions
    {
        public static TbReport AddPerson(this TbReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static TbReport AddPersons(this TbReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));
        public static TbReport AddActivity(this TbReport report, Activity activity) => report.InvokeAndReturn(m => m.Activities.Add(activity));
        public static TbReport AddActivities(this TbReport report, IEnumerable<Activity> activities) => report.InvokeAndReturn(m => m.Activities.AddRange(activities));

        private static TbReport InvokeAndReturn(this TbReport m, Action<TbReport> action)
        {
            action(m);
            return m;
        }

        public static Task<SendResult> Send(this TbReport report, Uri address, string username, string password) => new ReportSendClient(address).Send(report, username, password);

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