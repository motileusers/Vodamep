using System;
using Google.Protobuf.Reflection;


namespace Vodamep.Data
{
    public class DischargeLocationProvider : CodeProviderBase
    {
        private static volatile DischargeLocationProvider instance;
        private static object syncRoot = new Object();

        public static DischargeLocationProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DischargeLocationProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.discharge_location.csv";
    }
}
