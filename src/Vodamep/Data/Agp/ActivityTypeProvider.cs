using System;

namespace Vodamep.Data.Agp
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

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Agp.activitytypes.csv";
    }
}
