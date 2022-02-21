using System;
using Vodamep.ReportBase;

namespace Vodamep.StatLp.Model
{
    public partial class Person : IPerson
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
            return PersonNameBuilder.FullNameOrId(this.GivenName, this.FamilyName, this.Id);
        }
    }
};