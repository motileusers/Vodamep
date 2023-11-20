using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Agp
{
    public class DiagnosisgroupProvider : CodeProviderBase
    {
        private static volatile DiagnosisgroupProvider instance;
        private static object syncRoot = new Object();

        public static DiagnosisgroupProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DiagnosisgroupProvider();
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