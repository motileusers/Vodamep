using System;

namespace Vodamep.Data.StatLp
{
    public class HousingReasonProvider : CodeProviderBase
    {
        private static volatile HousingReasonProvider instance;
        private static object syncRoot = new Object();

        public static HousingReasonProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new HousingReasonProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Data.StatLp.housing_reason.csv";
    }
}
