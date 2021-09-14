using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{

    /// <summary>
    /// Clearing der Personen IDs durchführen
    /// </summary>
    /// <remarks>
    /// Daten werden von Einrichtung 1  von System A gesendet.
    /// Nach einiger Zeit wird der Anbieter gewechselt und Sytem B sendet die Daten von Einrichtung 1.
    ///   Die Primary Keys im Quellsystem ändern sich, weil eine Datenmigration durchgeführt wurde.
    ///   Mit einem Primary Key Mapping und ID von System A auf ID von System B können Daten zusammengeführt werden.
    /// Eine automatisches Mapping unterstützt die Funktion. 
    ///   Anhand von Name und Geburtsdatum des Klienten können die Daten automatisch gemappt werden.
    /// </remarks>
    public class IdMapper
    {

        public void Map(StatLpReportHistory history)
        {
            // Eine Liste mit allen Reports erstellen
            List<StatLpReport> reports = history.StatLpReports.ToList();
            reports.Add(history.StatLpReport);

            // Zurücksetzen
            ResetExistingClearingIds(reports);


            // Mehrere Stufen vom Mapping werden angewendet
            // Die letzte Stufe ist jene mit der höchsten Priorität und kann alle 
            // Stufen davor überschreiben: Fixe Mappings aus der DB

            // Ungemappte Daten verarbeiten
            MapSourceAndId(reports);

            // Mapping anhand Geburtstag und Name
            MapBirthdayNameIds(reports);

            // Gespeicherte Mappings anwenden
            MapSavedIds(reports, history);

        }




        /// <summary>
        /// Fixe Mappings für Personen / Quellen festlegen.
        /// Mit dieser Methode kann das gesamte automatische Mapping überschrieben werden.
        /// </summary>
        private void MapSavedIds(List<StatLpReport> reports, StatLpReportHistory history)
        {
            Dictionary<string, IdMapping> savedIdMapping = BuildSavedMappingDictionary(history.ExistingIdMappings);


            foreach (StatLpReport report in reports)
            {
                foreach (Person person in report.Persons)
                {
                    string mappingKey = GetMappingKey(report, person); ;

                    if (savedIdMapping.ContainsKey(mappingKey))
                    {
                        person.ClearingId = savedIdMapping[mappingKey].ClearingId;
                    }
                }
            }
        }



        /// <summary>
        /// Dictionary für den schnellen Zugriff auf die Personen IDs
        /// </summary>
        private Dictionary<string, IdMapping> BuildSavedMappingDictionary(List<IdMapping> mappings)
        {
            Dictionary<string, IdMapping> savedIdMapping = new Dictionary<string, IdMapping>();

            if (mappings != null)
            {
                foreach (IdMapping mapping in mappings)
                {
                    savedIdMapping.Add(mapping.SourceSystemId + "-" + mapping.Id, mapping);
                }
            }

            return savedIdMapping;
        }





        /// <summary>
        /// Automatisches Mapping anhand des Namens und Geburtsdatum.
        /// Die Personen Id wird gar nicht mehr berücksichtigt und einfach überschrieben.
        /// Grund: Die Wahrscheinlichkeit, dass innerhalb einer Einrichtung der gleiche Name 
        /// mit dem gleichen gleichen Geburtstag und unterschiedlichen Ids vorkommt, ist sehr gering.
        /// Wenn, dann kann das über die die gespeicherten Id Mappings korrigiert werden.
        /// </summary>
        private void MapBirthdayNameIds(List<StatLpReport> reports)
        {
            // Ein gemeinsames Dictionary für alle Sourcen
            Dictionary<string, string> nameBirthdayDictionary = new Dictionary<string, string>();

            foreach (StatLpReport report in reports)
            {
                // Zuerst legen wir nur ein Dictionary mit den Personendaten an
                foreach (Person person in report.Persons)
                {
                    string nameBirthday = person.BirthdayD.ToShortDateString() + "-" + person.FamilyName + "-" + person.GivenName;

                    if (!nameBirthdayDictionary.ContainsKey(nameBirthday))
                    {
                        // Wenn noch keine Clearing ID existiert, dann verwenden wir die bestehende der Person
                        nameBirthdayDictionary.Add(nameBirthday, person.ClearingId);
                    }
                    else
                    {
                        // Wenn es eine Person mit genau der Kombination gibt, dann verwenden wie die gespeicherte Clearing Id
                        person.ClearingId = nameBirthdayDictionary[nameBirthday];
                    }
                }
            }
        }


        /// <summary>
        /// Gleiche Source und ID zwischen den Meldungen erhalten dieselbe Clearing ID
        /// </summary>
        private void MapSourceAndId(List<StatLpReport> reports)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (StatLpReport report in reports)
            {
                foreach (Person person in report.Persons)
                {
                    string sourceIdKey = GetMappingKey(report, person);

                    if (!dictionary.ContainsKey(sourceIdKey))
                    {
                        string clearingId = System.Guid.NewGuid().ToString();
                        person.ClearingId = clearingId;
                        dictionary.Add(sourceIdKey, clearingId);
                    }
                    else
                    {
                        person.ClearingId = dictionary[sourceIdKey];
                    }
                }
            }
        }





        /// <summary>
        /// Alle Clearing Ids zurücksetzen, damit neu gemappt werden kann
        /// </summary>
        private void ResetExistingClearingIds(List<StatLpReport> reports)
        {
            foreach (StatLpReport report in reports)
            {
                foreach (Person person in report.Persons)
                {
                    person.ClearingId = "";
                }
            }
        }



        private string GetMappingKey(StatLpReport report, Person person)
        {
            string mappingKey = report.SourceSystemId + "-" + person.Id;
            return mappingKey;
        }
    }
}
