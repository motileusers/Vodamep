using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Tb
{
    public class ServiceProvider : CodeProviderBase
    {
        private static volatile ServiceProvider instance;
        private static object syncRoot = new Object();

        public static ServiceProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ServiceProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Tb.Model.TbReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Tb.Service.csv";
    }
}
