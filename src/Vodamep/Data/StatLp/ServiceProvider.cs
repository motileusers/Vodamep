using System;

namespace Vodamep.Data.StatLp
{
    public class ServiceProvider : CodeProviderBase
    {
        private static volatile ServiceProvider instance;
        private static object syncRoot = new Object();

        public static ServiceProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ServiceProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.StatLp.service.csv";
    }
}
