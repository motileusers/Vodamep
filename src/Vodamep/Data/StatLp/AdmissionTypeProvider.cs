using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.StatLp
{
    public class AdmissionTypeProvider : CodeProviderBase
    {
        private static volatile AdmissionTypeProvider instance;
        private static object syncRoot = new Object();

        public static AdmissionTypeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AdmissionTypeProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.admission_type.csv";
    }
}
