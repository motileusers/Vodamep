using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodamep.Cm.Validation;
using Vodamep.ReportBase;

namespace Vodamep.Cm.Model
{
    public static class CmReportExtensions
    {
        public static CmReport AddPerson(this CmReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static CmReport AddPersons(this CmReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        public static CmReport AddClientActivity(this CmReport report, ClientActivity person) => report.InvokeAndReturn(m => m.ClientActivities.Add(person));
        public static CmReport AddClientActivities(this CmReport report, IEnumerable<ClientActivity> persons) => report.InvokeAndReturn(m => m.ClientActivities.AddRange(persons));

        private static CmReport InvokeAndReturn(this CmReport m, Action<CmReport> action)
        {
            action(m);
            return m;
        }

        public static Task<SendResult> Send(this CmReport report, Uri address, string username, string password) => new ReportSendClient(address).Send(report, username, password);

        public static CmReportValidationResult Validate(this CmReport report) => (CmReportValidationResult)new CmReportValidator().Validate(report);

        public static string ValidateToText(this CmReport report, bool ignoreWarnings) => new CmReportValidationResultFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static IEnumerable<string> ValidateToEnumerable(this CmReport report, bool ignoreWarnings) => new CmReportValidationResultListFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static CmReport AsSorted(this CmReport report)
        {
            var result = new CmReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            //result.Activities.AddRange(report.Activities.AsSorted());
            
            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));            
            //result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));

            return result;
        }

    }
}