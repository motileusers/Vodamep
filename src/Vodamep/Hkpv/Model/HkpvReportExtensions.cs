using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodamep.Hkpv.Validation;
using Vodamep.ReportBase;

namespace Vodamep.Hkpv.Model
{
    public static class HkpvReportExtensions
    {
        public static HkpvReport AddPerson(this HkpvReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static HkpvReport AddPersons(this HkpvReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        private static HkpvReport InvokeAndReturn(this HkpvReport m, Action<HkpvReport> action)
        {
            action(m);
            return m;
        }

        public static Task<SendResult> Send(this HkpvReport report, Uri address, string username, string password) => new ReportSendClient(address).Send(report, username, password);

        public static HkpvReportValidationResult Validate(this HkpvReport report) => (HkpvReportValidationResult)new HkpvReportValidator().Validate(report);

        public static string ValidateToText(this HkpvReport report, bool ignoreWarnings) => new HkpvReportValidationResultFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static IEnumerable<string> ValidateToEnumerable(this HkpvReport report, bool ignoreWarnings) => new HkpvReportValidationResultListFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static HkpvReport AsSorted(this HkpvReport report)
        {
            var result = new HkpvReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            result.Activities.AddRange(report.Activities.AsSorted());
            
            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));            
            result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));

            return result;
        }

    }
}