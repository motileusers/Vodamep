using System;

namespace Vodamep.Data
{
    public sealed class GenderProvider : CodeProviderBase
    {
        private static volatile GenderProvider instance;
        private static readonly object syncRoot = new Object();        

        public static GenderProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GenderProvider();
                    }
                }

                return instance;
            }
        }

        protected override string ResourceName => "Datasets.gender.csv";
        public override string Unknown => "";
    }
}
