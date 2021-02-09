using System;
using Vodamep.Hkpv.Model;

namespace Vodamep.ReportBase
{
    public static class ReportFactory
    {
        public static IReportBase GetReportType(byte[] reportStream)
        {
            HkpvReport hkpvReport = null;

            try
            {
                hkpvReport = HkpvReport.Read(reportStream);
                return hkpvReport;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}