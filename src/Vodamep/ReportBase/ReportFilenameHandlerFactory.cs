using System;
using Vodamep.Agp;
using Vodamep.Cm;
using Vodamep.Hkpv;
using Vodamep.Mkkp;
using Vodamep.Mohi;
using Vodamep.StatLp;
using Vodamep.Tb;

namespace Vodamep.ReportBase
{
    public class ReportFilenameHandlerFactory
    {

        public ReportFilenameHandler Create(ReportType type)
        {
            switch (type)
            {
                case ReportType.Agp:
                    return new AgpReportFilenameHandler();
                case ReportType.Cm:
                    return new CmReportFilenameHandler();
                case ReportType.Hkpv:
                    return new HkpvReportFilenameHandler();
                case ReportType.Mkkp:
                    return new MkkpReportFilenameHandler();
                case ReportType.Mohi:
                    return new MohiReportFilenameHandler();
                case ReportType.StatLp:
                    return new StatLpReportFilenameHandler();
                case ReportType.Tb:
                    return new TbReportFilenameHandler();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            throw new NotSupportedException();
        }
    }
}