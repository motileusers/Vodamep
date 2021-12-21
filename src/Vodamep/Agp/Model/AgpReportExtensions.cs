using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vodamep.Agp.Validation;
using Vodamep.ReportBase;
using ResultFormatterTemplate = Vodamep.Agp.Validation.ResultFormatterTemplate;

namespace Vodamep.Agp.Model
{
    public static class AgpReportExtensions
    {
        public static AgpReport AddPerson(this AgpReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static AgpReport AddPersons(this AgpReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        private static AgpReport InvokeAndReturn(this AgpReport m, Action<AgpReport> action)
        {
            action(m);
            return m;
        }

        public static Task<SendResult> Send(this AgpReport report, Uri address, string username, string password) => new ReportSendClient(address).Send(report, username, password);

        public static AgpReportValidationResult Validate(this AgpReport report) => (AgpReportValidationResult)new AgpReportValidator().Validate(report);

        public static string ValidateToText(this AgpReport report, bool ignoreWarnings) => new AgpReportValidationResultFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static IEnumerable<string> ValidateToEnumerable(this AgpReport report, bool ignoreWarnings) => new AgpReportValidationResultListFormatter(ResultFormatterTemplate.Text, ignoreWarnings).Format(report, Validate(report));

        public static AgpReport AsSorted(this AgpReport report)
        {
            var result = new AgpReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            result.Activities.AddRange(report.Activities.AsSorted());
            result.StaffActivities.AddRange(report.StaffActivities.AsSorted());

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));
            result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));

            return result;
        }


    }
}