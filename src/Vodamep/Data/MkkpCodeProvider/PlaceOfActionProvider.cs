using System;

namespace Vodamep.Data.MkkpCodeProvider
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

        public override string Unknown => "ZZ";

        protected override string ResourceName => "Data.Mkkp.places_of_action.csv";
    }
}