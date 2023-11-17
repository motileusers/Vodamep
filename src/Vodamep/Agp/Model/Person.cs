using System;
using Vodamep.ReportBase;

namespace Vodamep.Agp.Model
{
    public partial class Person : INamedPerson
    {
        /// <summary>
        /// Id, die für das Personen Mapping der Personen mit unterschiedlichen IDs über mehrere Meldungen hinweg verwendet wird.
        /// Keine Befüllung notwendig. Wird beim Prüfen der History zur Laufzeit von der Clearing Stelle vergeben.
        /// Unterschiedliche Personen IDs von 2 Systemen werden auf eine ID gemappt. Siehe StatLpReport.source_system_id
        /// </summary>
        public string ClearingId { get; set; }

        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }
        public string GetDisplayName()
        {
            string result = this.GivenName;
            if (!string.IsNullOrEmpty(this.GivenName) && !string.IsNullOrEmpty(this.FamilyName)) result += " ";
            result += this.FamilyName;

            if (string.IsNullOrEmpty(result)) result = this.Id;

            return result;
        }
    }
}