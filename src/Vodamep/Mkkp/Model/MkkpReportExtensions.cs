using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vodamep.Data.Dummy;
using Vodamep.Mkkp.Validation;

namespace Vodamep.Mkkp.Model
{
    public static class MkkpReportExtensions
    {
        public static MkkpReport AddPerson(this MkkpReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static MkkpReport AddPersons(this MkkpReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        private static MkkpReport InvokeAndReturn(this MkkpReport m, Action<MkkpReport> action)
        {
            action(m);
            return m;
        }

        // todo


        public static MkkpReportValidationResult Validate(this MkkpReport report) => (MkkpReportValidationResult)new MkkpReportValidator().Validate(report);

        // todo

        public static MkkpReport AsSorted(this MkkpReport report)
        {
            var result = new MkkpReport();

            // todo

            return result;
        }

        public static string GetSHA256Hash(this MkkpReport report)
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