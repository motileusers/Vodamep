using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.StatLp.Validation
{

    /// <summary>
    /// Datensatz für das Clearing/Mapping der IDs
    /// </summary>
    public class IdMapping
    {
        /// <summary>
        /// Key für das Quellsystem der Daten
        /// </summary>
        public string SourceSystemId { get; set; }

        /// <summary>
        /// Id im Datenpaket
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Clearing Id mit der die History Prüfung durchgeführt wird
        /// </summary>
        public string ClearingId { get; set; }


        /// <summary>
        /// Anzahl der gemappten Personen
        /// </summary>
        public int Count { get; set; }

        public override string ToString()
        {
            return this.SourceSystemId + " / " + this.Id + " / " + ClearingId;
        }

    }
}
