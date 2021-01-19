using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vodamep.Agp.Validation;

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

        public static Task<SendResult> Send(this AgpReport report, Uri address, string username, string password) => new AgpReportSendClient(address).Send(report, username, password);

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

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));
            result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));
            result.TravelTimes.AddRange(report.TravelTimes.OrderBy(x => x.Id));

            return result;
        }

        public static string GetSHA256Hash(this AgpReport report)
        {
            using (var s = SHA256.Create())
            {
                var h = s.ComputeHash(report.ToByteArray());

                var sha256 = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(h));

                return sha256;
            }
        }


    }
}