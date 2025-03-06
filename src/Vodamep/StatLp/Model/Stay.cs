using System;

namespace Vodamep.StatLp.Model
{
    public partial class Stay
    {
        /// <summary>
        /// Id, die verwendet werden kann um externe IDs zu speichern (wird nicht übermittelt)
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Personen Id, die extern verwendet werden kann
        /// </summary>
        public string ExternalPersonId { get; set; }

        /// <summary>
        /// Aufnahme Id, die extern verwendet werden kann
        /// </summary>
        public string ExternalAdmissionId { get; set; }

        /// <summary>
        /// Abgang Id, die extern verwendet werden kann
        /// </summary>
        public string ExternalLeavingId { get; set; }

        /// <summary>
        /// Referenz Datum, das verwendet werden kann um externe Daten zu speichern (wird nicht übermittelt)
        /// </summary>
        public DateTime ReferenceDate  { get; set; }


        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime? ToD { 
            get => this.To != null ? this.To.AsDate() :(DateTime ?) null; 
            set => this.To = value.HasValue? value.Value.AsTimestamp() : null; 
        }

    }
}