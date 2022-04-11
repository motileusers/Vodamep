using System;

namespace Vodamep.Data
{
    public class CareAllowanceProvider : CodeProviderBase
    {
        private static volatile CareAllowanceProvider instance;
        private static object syncRoot = new Object();

        public static CareAllowanceProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CareAllowanceProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.care_allowance.csv";
    }
}
