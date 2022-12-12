using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Vodamep.Data
{

    /// <summary>
    /// Gemeinde (im Sinne der Gemeindekennzahl - GKZ)
    /// </summary>
    /// <remarks>
    /// In Vorarlberg gibt es 96 Gemeinden. 
    /// Jede Gemeinde verfügt über eine eindeutige Gemeindekennzahl. Z.B. 80101
    /// 
    /// Einer Gemeinde sind mehrere Postleitzahl- / Ortspaare zugewiesen
    /// - 6800 Feldkirch;80404;
    /// - 6800 Feldkirch-Altenstadt;80404;
    /// - 6781 Bartholomäberg;80101;
    /// Siehe: Datasets/PostcodeCity
    /// 
    /// Konvention fürs DataSet:
    /// Die erste PLZ für eine Gemeindekennzahl ist die PLZ / Bezeichnung für die Gemeinde
    /// 
    /// Postleitzahlen sind nicht eindeutig
    /// Es gibt Orte, die über die gleiche Postleitzahl verfügen
    /// 
    /// </remarks>
    public class Commune
    {
        /// <summary>
        /// Gemeindekennzahl
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Liste mit Postleitzahlen
        /// </summary>
        public List<PostcodeCity> PostcodeCities { get; set; } = new List<PostcodeCity>();

        /// <summary>
        /// Standard Plz / Ortsname für eine Gemeinde
        /// </summary>
        public PostcodeCity DefaultPostcode 
        {
            get
            {
                return PostcodeCities.FirstOrDefault();
            }
        }

        public override string ToString()
        {
            return $"{Id} / {DefaultPostcode.PoCode} {Name} ({PostcodeCities.Count} PLZ/Orte)";
        }
    }
}
