using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Vodamep.Agp.Validation;

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

        // todo


        public static AgpReportValidationResult Validate(this AgpReport report) => (AgpReportValidationResult)new AgpReportValidator().Validate(report);

        // todo

        public static AgpReport AsSorted(this AgpReport report)
        {
            var result = new AgpReport();

            // todo

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