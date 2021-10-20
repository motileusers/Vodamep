using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.ValidationBase
{

    /// <summary>
    /// Clearing Id, die eine potenziell doppelte Clearing ID splitten kann
    /// </summary>
    public class ClearingIdSpilt
    {

        /// <summary>
        /// Id des Datenlieferanten 
        /// </summary>
        public string SourceSystemId { get; set; }

        /// <summary>
        /// Id der Person, die gesplittet wird
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// Clearing ID, die auf die gesuchte gesetzt wird
        /// </summary>
        public string ToId { get; set; }

    }
}
