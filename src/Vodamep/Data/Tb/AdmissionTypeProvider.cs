using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Tb
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

        protected override FileDescriptor Descriptor => Vodamep.Tb.Model.TbReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Tb.AdmissionType.csv";
    }
}
