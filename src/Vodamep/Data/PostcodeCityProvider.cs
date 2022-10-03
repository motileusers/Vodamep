using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data
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

        public override bool IsValid(string code)
        {
            return base.IsValid(code);
        }

        protected override FileDescriptor Descriptor => null;

        protected override string ResourceName => "Datasets.PostcodeCity.csv";

        public override string Unknown => "";
    }
}
