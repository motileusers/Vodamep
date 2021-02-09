using System;
using System.IO;
using Vodamep.Agp.Model;
using Vodamep.Hkpv.Model;
using Vodamep.Mkkp.Model;
using Vodamep.ReportBase;

namespace Vodamep.Api
{
    public class ReportBaseHandler
    {
        public IReportBase GetReport(string reportType, Stream reportStream)
        {
            var bytes = this.Convert(reportStream);

            IReportBase report = null;

            switch (reportType?.ToLower())
            {
                case "agp":
                     report = AgpReport.Read(bytes);
                    break;
                case "hkpv":
                    report = HkpvReport.Read(bytes);
                    break;
                case "mkkp":
                     report = MkkpReport.Read(bytes);
                    break;
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