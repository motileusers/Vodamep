using System;
using Google.Protobuf.Reflection;

namespace Vodamep.Data.Mkkp
{
    public class PlaceOfActionProvider : CodeProviderBase
    {
        private static volatile PlaceOfActionProvider instance;
        private static object syncRoot = new Object();

        public static PlaceOfActionProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PlaceOfActionProvider();
                    }
                }

                return instance;
            }
        }

        protected override FileDescriptor Descriptor => Vodamep.Mkkp.Model.MkkpReflection.Descriptor;

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.Mkkp.places_of_action.csv";
    }
}