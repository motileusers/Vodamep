using System;
using System.Collections.Generic;
using System.Text;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp
{

    public class History
    {
        public List<HistoryPerson> Persons { get; set; }
    }


    public class HistoryPerson
    {
        public Person Person { get; set; }

        public List<HistoryStay> Stays { get; set; }
    }


    public class HistoryStay
    {
        public Stay Stay { get; set; }

        // Die 4 Attribute sind immer dabei und zwar mit dem Wert, der zuletzt gesetzt wurde
        // kann auch schon Monate davor gemeldet worden sein

        public Nullable<AdmissionType> AdmissionType { get; set; }

        public Nullable<CareAllowance> CareAllowance { get; set; }

        public Nullable<CareAllowanceArge> CareAllowanceArge { get; set; }

        public Nullable<Finance> Finance { get; set; }




        // Immer dabei ist die zuletzt zu diesem Aufenthalt gültige Aufnahme (kann irgendwann, auch Monate, davor gewesen sein)
        // Die braucht man zum Gegenchecken von einzelnen Werten
        public Admission CorrespondingAdmission { get; set; }


        // Immer dabei ist auch eine evtl. Entlassung, die vor gültigen Aufnahme erfolgt ist
        // mir der können Zusammenhänge (Zeiträume) zwischen Entlassung und neuer Aufnahme geprüft werden
        public Admission LastLeaving { get; set; }


        // Wichtig sind auch die akutellen Daten, falls eine Aufnahme oder Entlassung direkt an diesem Aufenhalt hängt
        public Leaving CurrentLeaving { get; set; }

        public Leaving CurrentAdmission { get; set; }


        // Mit den beiden können wir direkt aus der History auf den Vorgänger und Nachfolger zugreifen (die Moldung vom Vormonat)
        public HistoryStay PreviousStay { get; set; }

        public HistoryStay NextStay { get; set; }
    }




    public class HistoryGenerator
    {
        /// <summary>
        /// Auflisten der Personengeschichte als fast flache Liste mit Vorgänger
        /// </summary>
        public History BuildHistory(List<StatLpReport> reports)
        {
            History history = new History();

            foreach (StatLpReport report in reports)
            {
                foreach (Stay stay in report.Stays)
                {
                    HistoryStay historyStay = new HistoryStay()
                    {
                        Stay = stay
                    };


                    // Entsprechende Admission zu diesem Stay suchen (können ja mehrere Admissions pro Person im Report sein)
                    historyStay.CorrespondingAdmission = null;


                    // Genau hier können auch Prüfungen über eine durchgängige Struktur gemacht werden,
                    // die hier beschrieben sind
                    // StatLpValidation_Person_History.feature

                }


            }

            return history;

        }

    }
}
