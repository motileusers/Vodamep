using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Agp
{
    public class DiagnosisGroupProvider : CodeProviderBase
    {
        private static volatile DiagnosisGroupProvider instance;
        private static object syncRoot = new Object();

        public static DiagnosisGroupProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DiagnosisGroupProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Agp.Model.AgpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Agp.Diagnosisgroup.csv";
    }
}