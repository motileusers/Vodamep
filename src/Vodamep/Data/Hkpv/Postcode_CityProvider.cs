using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Hkpv
{
    public sealed class Postcode_CityProvider : CodeProviderBase
    {
        private static volatile Postcode_CityProvider instance;
        private static object syncRoot = new Object();        

        public static Postcode_CityProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Postcode_CityProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Hkpv.Model.HkpvReflection.Descriptor;

        protected override string ResourceName => "Datasets.Hkpv.postcode_cities.csv";

        public override string Unknown => "";
    }
}
