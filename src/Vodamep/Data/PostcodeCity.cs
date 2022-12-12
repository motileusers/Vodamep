using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.Data
{

    /// <summary>
    /// Postleitzahlen- / Ortspaar
    /// </summary>
    public class PostcodeCity
    {
        /// <summary>
        /// Postleitzahl
        /// </summary>
        public string PoCode { get; set; }

        /// <summary>
        /// Ort
        /// </summary>
        public string City { get; set; }


        public override string ToString()
        {
            return $"{PoCode} {City}";
        }

    }
}
