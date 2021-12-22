using System;

namespace Vodamep.Data.Agp
{
    public class StaffActivityTypeProvider : CodeProviderBase
    {
        private static volatile StaffActivityTypeProvider instance;
        private static object syncRoot = new Object();

        public static StaffActivityTypeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new StaffActivityTypeProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Data.Agp.staffactivitytypes.csv";
    }
}
