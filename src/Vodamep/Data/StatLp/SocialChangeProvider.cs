using System;
using Google.Protobuf.Reflection;

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

        protected override FileDescriptor Descriptor => Vodamep.Agp.Model.ApgReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.social_change.csv";
    }
}
