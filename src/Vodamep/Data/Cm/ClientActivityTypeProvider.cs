using System;

namespace Vodamep.Data.Cm
{
    public class ClientActivityTypeProvider : CodeProviderBase
    {
        private static volatile ClientActivityTypeProvider instance;
        private static object syncRoot = new Object();

        public static ClientActivityTypeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ClientActivityTypeProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Cm.clientactivitytypes.csv";
    }
}
