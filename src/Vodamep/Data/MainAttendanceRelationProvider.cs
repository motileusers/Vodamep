using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data
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

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.main_attendance_relation.csv";
    }
}
