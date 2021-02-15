using System;
using Vodamep.Agp;
using Vodamep.Hkpv;
using Vodamep.Mkkp;

namespace Vodamep.ReportBase
{
    public class ReportFilenameHandlerFactory
    {
        public ReportFilenameHandler Create(string type)
        {
            switch (type.ToLower())
            {
                case "agp":
                    return new AgpReportFilenameHandler();

                case "hkpv":
                    return new HkpvReportFilenameHandler();

                case "mkkp":
                    return new MkkpReportFilenameHandler();
            }

            throw new NotSupportedException();
        }
    }
}