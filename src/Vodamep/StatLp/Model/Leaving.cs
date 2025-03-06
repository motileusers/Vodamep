using System;

namespace Vodamep.StatLp.Model
{
    public partial class Leaving
    {
        /// <summary>
        /// Id, die verwendet werden kann um externe IDs zu speichern (wird nicht übermittelt)
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Referenz Id, die verwendet werden kann um externe IDs zu speichern (wird nicht übermittelt)
        /// </summary>
        public string ExternalStayId { get; set; }

        /// <summary>
        /// Referenz Datum, das verwendet werden kann um externe Daten zu speichern (wird nicht übermittelt)
        /// </summary>
        public DateTime ReferenceDate { get; set; }

        public DateTime LeavingDateD { get => this.LeavingDate.AsDate(); set => this.LeavingDate = value.AsTimestamp(); }
    }
}