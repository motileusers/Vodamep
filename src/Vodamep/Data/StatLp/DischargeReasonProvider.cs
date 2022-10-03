using System;
using Google.Protobuf.Reflection;

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

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.DischargeReason.csv";
    }
}
