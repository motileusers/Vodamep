using System;

namespace Vodamep.Data
{
    public class MainAttendanceClosenessProvider : CodeProviderBase
    {
        private static volatile MainAttendanceClosenessProvider instance;
        private static object syncRoot = new Object();

        public static MainAttendanceClosenessProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MainAttendanceClosenessProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.main_attendance_closeness.csv";
    }
}
