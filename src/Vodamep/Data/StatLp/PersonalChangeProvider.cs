using System;
using Google.Protobuf.Reflection;

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

        protected override FileDescriptor Descriptor => Vodamep.StatLp.Model.StatLpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.StatLp.PersonalChange.csv";
    }
}
