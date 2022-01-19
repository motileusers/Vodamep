using System;

namespace Vodamep.Data.StatLp
{
    public class MainAttendanceRelationProvider : CodeProviderBase
    {
        private static volatile MainAttendanceRelationProvider instance;
        private static object syncRoot = new Object();

        public static MainAttendanceRelationProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MainAttendanceRelationProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.StatLp.main_attendance_relation.csv";
    }
}
