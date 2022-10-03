using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data
{
    public class CareAllowanceArgeProvider : CodeProviderBase
    {
        private static volatile CareAllowanceArgeProvider instance;
        private static object syncRoot = new Object();

        public static CareAllowanceArgeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CareAllowanceArgeProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.CareAllowanceArge.csv";
    }
}
