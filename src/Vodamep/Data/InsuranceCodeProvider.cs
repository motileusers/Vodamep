using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data
{
    public sealed class InsuranceCodeProvider : CodeProviderBase
    {
        private static volatile InsuranceCodeProvider instance;
        private static object syncRoot = new Object();

        public static InsuranceCodeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new InsuranceCodeProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => null;

        protected override string ResourceName => "Datasets.InsuranceCode.csv";

        public override string Unknown => "UB";
    }
}
