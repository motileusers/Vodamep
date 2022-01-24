using System;

namespace Vodamep.Data.StatLp
{
    public class DeathLocationProvider : CodeProviderBase
    {
        private static volatile DeathLocationProvider instance;
        private static object syncRoot = new Object();

        public static DeathLocationProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DeathLocationProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.StatLp.death_location.csv";
    }
}
