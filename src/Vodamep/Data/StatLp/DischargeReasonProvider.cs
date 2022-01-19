using System;

namespace Vodamep.Data.StatLp
{
    public class DischargeReasonProvider : CodeProviderBase
    {
        private static volatile DischargeReasonProvider instance;
        private static object syncRoot = new Object();

        public static DischargeReasonProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DischargeReasonProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.StatLp.discharge_reason.csv";
    }
}
