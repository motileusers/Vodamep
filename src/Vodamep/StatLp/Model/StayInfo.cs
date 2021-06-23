using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.StatLp.Model
{

    /// <summary>
    /// Information zu einem Aufenthalt, meldungsübergreifend, über mehrere Monate
    /// </summary>
    public class StayInfo
    {

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        /// <summary>
        /// Zugehörige Aufnahme, muss eigentlich immer vorhanden sein
        /// </summary>
        public Admission Admission { get; set; }


        /// <summary>
        /// Liste mit Aufnahmen, falls mehrere vorhanden sind (Fehler)
        /// </summary>
        public List<Admission> Admissions { get; set; } = new List<Admission>();

        /// <summary>
        /// Zugehörige Entlassung, bie bestehender Aufnahme leer
        /// </summary>
        public Leaving Leaving { get; set; }

        /// <summary>
        /// Liste mit Entlassungen, falls mehrere vorahnden sind (Fehler)
        /// </summary>
        public List<Leaving> Leavings { get; set; } = new List<Leaving>();


        /// <summary>
        /// Liste mit Attributen, die ein in den Stays gesendet wurden
        /// </summary>
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();


        /// <summary>
        /// Aufenthalt, der zeilich vor diesem lag
        /// </summary>
        public StayInfo PreviousStay { get; set; }
        
        
        /// <summary>
        /// Aufenthalt, der zeitlich nach diesem lag
        /// </summary>
        public StayInfo NextStay { get; set; }

        /// <summary>
        /// Handelt es sich um den aktuellen Aufenthalt?
        /// = Letzte Meldung mit Aufenthalt eines Klienten bis zum Monatsende, ohne Entlassung zum Monatsende
        /// </summary>
        public bool IsActualStay { get; set; }
    }
}
