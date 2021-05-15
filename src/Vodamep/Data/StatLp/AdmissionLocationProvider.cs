using System;

namespace Vodamep.Data.StatLp
{
    public class AdmissionLocationProvider : CodeProviderBase
    {
        private static volatile AdmissionLocationProvider instance;
        private static object syncRoot = new Object();

        public static AdmissionLocationProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AdmissionLocationProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Data.StatLp.admission_location.csv";
    }
}
