using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Cm
{
    public class ActivityTypeProvider : CodeProviderBase
    {
        private static volatile ActivityTypeProvider instance;
        private static object syncRoot = new Object();

        public static ActivityTypeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ActivityTypeProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Cm.Model.CmReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Cm.ActivityType.csv";
    }
}
