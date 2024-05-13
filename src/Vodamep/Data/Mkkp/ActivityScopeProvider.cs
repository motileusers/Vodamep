using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Mkkp
{
    public class ActivityScopeProvider : CodeProviderBase
    {
        private static volatile ActivityScopeProvider instance;
        private static object syncRoot = new Object();

        public static ActivityScopeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ActivityScopeProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Mkkp.Model.MkkpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Mkkp.ActivityScope.csv";
    }
}