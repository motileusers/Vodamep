using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data
{
    public class CountryCodeProvider : CodeProviderBase
    {
        private static volatile CountryCodeProvider instance;
        private static object syncRoot = new Object();

        public static CountryCodeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CountryCodeProvider();
                    }
                }

                return instance;
            }
        }



        protected override FileDescriptor Descriptor => null;

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.german-iso-3166.csv";
    }
}
