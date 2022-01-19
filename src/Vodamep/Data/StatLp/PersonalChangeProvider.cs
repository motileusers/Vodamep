using System;

namespace Vodamep.Data.StatLp
{
    public class PersonalChangeProvider : CodeProviderBase
    {
        private static volatile PersonalChangeProvider instance;
        private static object syncRoot = new Object();

        public static PersonalChangeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PersonalChangeProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Datasets.StatLp.personal_change.csv";
    }
}
