using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vodamep.ValidationBase
{

    /// <summary>
    /// Clearing Ausnahmen, um die Personen IDs zu mappen
    /// </summary>
    public class ClearingExceptions
    {
        public ClearingExceptions()
        {
            this.EqualMappings = new List<ClearingIdEqual>();
            this.SplitMappings = new List<ClearingIdSpilt>();
        }


        /// <summary>
        /// Personen IDs, die gleich gesetzt werden
        /// </summary>
        public List<ClearingIdEqual> EqualMappings { get; set; }


        /// <summary>
        /// Personen IDs, bei denen die Clearning IDs getrennt werden müssen
        /// </summary>
        public List<ClearingIdSpilt> SplitMappings { get; set; }



        internal Dictionary<string, ClearingIdEqual> EqualDictionary { get; set; }

        internal Dictionary<string, ClearingIdSpilt> SplitDictionary { get; set; }


        internal void BuildClearingDictionaries()
        {
            if (this.EqualDictionary == null)
                this.EqualDictionary = this.EqualMappings?.ToDictionary(x => x.FromId);

            if (this.SplitDictionary == null)
                this.SplitDictionary = this.SplitMappings?.ToDictionary(x => x.SourceSystemId + "." + x.PersonId);
        }

    }
}
