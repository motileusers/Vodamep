using System;

namespace Vodamep.Data.AgpCodeProvider
{
    public class ActivityTypeProvider : CodeProviderBase
    {
        private static volatile ActivityTypeProvider instance;
        private static object syncRoot = new Object();

        public static ActivityTypeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ActivityTypeProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Data.Agp.activitytypes.csv";
    }
}
