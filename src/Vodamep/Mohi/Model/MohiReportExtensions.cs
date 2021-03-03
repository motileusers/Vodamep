using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vodamep.Mohi.Validation;
using Vodamep.ReportBase;

namespace Vodamep.Mohi.Model
{
    public static class MohiReportExtensions
    {
        public static MohiReport AddPerson(this MohiReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static MohiReport AddPersons(this MohiReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        private static MohiReport InvokeAndReturn(this MohiReport m, Action<MohiReport> action)
        {
            action(m);
            return m;
        }

        public static Task<SendResult> Send(this MohiReport report, Uri address, string username, string password) => new ReportSendClient(address).Send(report, username, password);

        public static MohiReportValidationResult Validate(this MohiReport report) => (MohiReportValidationResult)new MohiReportValidator().Validate(report);

        public static string ValidateToText(this MohiReport report, bool ignoreWarnings) => new MohiReportValidationResultFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static IEnumerable<string> ValidateToEnumerable(this MohiReport report, bool ignoreWarnings) => new MohiReportValidationResultListFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static MohiReport AsSorted(this MohiReport report)
        {
            var result = new MohiReport()
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