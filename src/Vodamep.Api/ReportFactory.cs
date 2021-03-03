using System;
using System.IO;
using Vodamep.Agp.Model;
using Vodamep.Cm.Model;
using Vodamep.Hkpv.Model;
using Vodamep.Mkkp.Model;
using Vodamep.Mohi.Model;
using Vodamep.ReportBase;
using Vodamep.StatLp.Model;
using Vodamep.Tb.Model;

namespace Vodamep.Api
{
    public class ReportFactory
    {
        public IReportBase Create(ReportType reportType, Stream reportStream)
        {
            var bytes = this.Convert(reportStream);

            IReportBase report = null;

            //#extend 
            switch (reportType)
            {
                case ReportType.Agp:
                    report = AgpReport.Read(bytes);
                    break;
                case ReportType.Cm:
                    report = CmReport.Read(bytes);
                    break;
                case ReportType.Hkpv:
                    report = HkpvReport.Read(bytes);
                    break;
                case ReportType.Mkkp:
                    report = MkkpReport.Read(bytes);
                    break;
                case ReportType.Mohi:
                    report = MohiReport.Read(bytes);
                    break;
                case ReportType.StatLp:
                    report = StatLpReport.Read(bytes);
                    break;
                case ReportType.Tb:
                    report = TbReport.Read(bytes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reportType), reportType, null);
            }

            return report;

        }

        private byte[] Convert(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);

                return (ms.ToArray());
            }
        }
    }
}