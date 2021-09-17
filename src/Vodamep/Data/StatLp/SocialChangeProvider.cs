using System;

namespace Vodamep.Data.StatLp
{
    public class SocialChangeProvider : CodeProviderBase
    {
        private static volatile SocialChangeProvider instance;
        private static object syncRoot = new Object();

        public static SocialChangeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SocialChangeProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Data.StatLp.social_change.csv";
    }
}
