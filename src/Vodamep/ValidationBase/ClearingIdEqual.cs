using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.ValidationBase
{

    /// <summary>
    /// Mapping von zwei Clearing IDs, die gleichgesetzt werden
    /// </summary>
    public class ClearingIdEqual
    {
        /// <summary>
        /// Clearing ID, die gesucht / geändert wird
        /// </summary>
        public string FromId { get; set; }


        /// <summary>
        /// Clearing ID, die auf die gesuchte gesetzt wird
        /// </summary>
        public string ToId { get; set; }

    }
}
