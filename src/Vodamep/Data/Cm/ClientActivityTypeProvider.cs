using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Cm
{
    public class ClientActivityTypeProvider : CodeProviderBase
    {
        private static volatile ClientActivityTypeProvider instance;
        private static object syncRoot = new Object();

        public static ClientActivityTypeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ClientActivityTypeProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Cm.Model.CmReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Cm.ClientActivityType.csv";
    }
}
