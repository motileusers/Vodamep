using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data
{
    public class CareAllowanceProvider : CodeProviderBase
    {
        private static volatile CareAllowanceProvider instance;
        private static object syncRoot = new Object();

        public static CareAllowanceProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CareAllowanceProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.CareAllowance.csv";
    }
}
