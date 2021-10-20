using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Vodamep.ReportBase
{
    public class SHAHasher
    {

        /// <summary>
        /// Hash aus den Report Daten erzeugen
        /// </summary>
        public static string GetReportHash(byte[] report)
        {
            using (var s = SHA256.Create())
            {
                var h = s.ComputeHash(report);

                var sha256 = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(h));

                return sha256;
            }
        }
    }
}
