using System;

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

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.Agp.diagnosisgroups.csv";
    }
}