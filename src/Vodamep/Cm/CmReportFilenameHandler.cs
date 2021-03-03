using Vodamep.ReportBase;

namespace Vodamep.Cm
{
    public class CmReportFilenameHandler : ReportFilenameHandler
    {
        public override string GetFileName(IReportBase report, bool asJson, bool compressed = true)
        {
            var filename = base.GetFileName(report, asJson, compressed);

            if (!filename.EndsWith(".zip") && !filename.EndsWith(".json"))
            {
                filename += ".cm";
            }

            return filename;
        }
    }
}