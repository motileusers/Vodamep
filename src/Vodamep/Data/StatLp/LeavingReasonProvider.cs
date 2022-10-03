using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.StatLp
{
    public class LeavingReasonProvider : CodeProviderBase
    {
        private static volatile LeavingReasonProvider instance;
        private static object syncRoot = new Object();

        public static LeavingReasonProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LeavingReasonProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.LeavingReason.csv";
    }
}
