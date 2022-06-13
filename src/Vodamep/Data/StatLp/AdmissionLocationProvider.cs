using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.StatLp
{
    public class AdmissionLocationProvider : CodeProviderBase
    {
        private static volatile AdmissionLocationProvider instance;
        private static object syncRoot = new Object();

        public static AdmissionLocationProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AdmissionLocationProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.admission_location.csv";
    }
}
