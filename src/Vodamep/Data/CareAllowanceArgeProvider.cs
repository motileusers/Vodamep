﻿using System;

namespace Vodamep.Data
{
    public class CareAllowanceArgeProvider : CodeProviderBase
    {
        private static volatile CareAllowanceArgeProvider instance;
        private static object syncRoot = new Object();

        public static CareAllowanceArgeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CareAllowanceArgeProvider();
                    }
                }

                return instance;
            }
        }

        public override string Unknown => "";

        protected override string ResourceName => "Datasets.care_allowance_arge.csv";
    }
}
