using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.StatLp
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

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.service.csv";
    }
}
