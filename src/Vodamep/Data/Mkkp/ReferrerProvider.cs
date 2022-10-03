using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Mkkp
{
    public class ReferrerProvider : CodeProviderBase
    {
        private static volatile ReferrerProvider instance;
        private static object syncRoot = new Object();

        public static ReferrerProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ReferrerProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Mkkp.Model.MkkpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Mkkp.Referrer.csv";
    }
}
