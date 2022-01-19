using System;

namespace Vodamep.Data.StatLp
{
    public class AdmissionTypeProvider : CodeProviderBase
    {
        private static volatile AdmissionTypeProvider instance;
        private static object syncRoot = new Object();

        public static AdmissionTypeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AdmissionTypeProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.StatLp.admission_type.csv";
    }
}
