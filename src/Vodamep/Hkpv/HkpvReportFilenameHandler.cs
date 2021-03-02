using Vodamep.ReportBase;

namespace Vodamep.Hkpv
{
    public class HkpvReportFilenameHandler : ReportFilenameHandler
    {
        public override string GetFileName(IReportBase report, bool asJson, bool compressed = true)
        {
            var filename = base.GetFileName(report, asJson, compressed);

            if (!filename.EndsWith(".zip") && !filename.EndsWith(".json"))
            {
                filename += ".hkpv";
            }

            return filename;
        }
    }
}