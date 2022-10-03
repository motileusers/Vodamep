using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Hkpv
{
    public sealed class PostcodeCityProvider : CodeProviderBase
    {
        private static volatile PostcodeCityProvider instance;
        private static object syncRoot = new Object();        

        public static PostcodeCityProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PostcodeCityProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Hkpv.Model.HkpvReflection.Descriptor;

        protected override string ResourceName => "Datasets.Hkpv.PostcodeCity.csv";

        public override string Unknown => "";
    }
}
