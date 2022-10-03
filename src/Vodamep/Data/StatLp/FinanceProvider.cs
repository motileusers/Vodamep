using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.StatLp
{
    public class FinanceProvider : CodeProviderBase
    {
        private static volatile FinanceProvider instance;
        private static object syncRoot = new Object();

        public static FinanceProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new FinanceProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.Finance.csv";
    }
}
