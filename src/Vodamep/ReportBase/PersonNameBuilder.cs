using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.ReportBase
{
    public class PersonNameBuilder
    {
        /// <summary>
        /// Formatierten Namen erstellen
        /// </summary>
        public static string FullNameOrId(string given, string family, string id)
        {
            string result = given;
            if (!string.IsNullOrEmpty(given) && !string.IsNullOrEmpty(family)) result += " ";
            result += family;

            if (string.IsNullOrEmpty(result)) result = id;

            return result;
        }
    }
}
