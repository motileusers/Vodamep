using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.Data
{

    /// <summary>
    /// Code mit Gültigkeitszeitraum
    /// </summary>
    public class ValidCode
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }
    }
}
