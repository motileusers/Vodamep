using System;
using Google.Protobuf.Reflection;

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

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.death_location.csv";
    }
}
