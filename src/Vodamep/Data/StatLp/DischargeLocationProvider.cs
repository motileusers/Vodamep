using System;

namespace Vodamep.Data
{
    public class DischargeLocationProvider : CodeProviderBase
    {
        private static volatile DischargeLocationProvider instance;
        private static object syncRoot = new Object();

        public static DischargeLocationProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DischargeLocationProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.StatLp.discharge_location.csv";
    }
}
