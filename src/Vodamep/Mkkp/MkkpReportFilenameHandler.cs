using Vodamep.ReportBase;

namespace Vodamep.Mkkp
{
    public class MkkpReportFilenameHandler : ReportFilenameHandler
    {
        public override string GetFileName(IReportBase report, bool asJson, bool compressed = true)
        {
            var filename = base.GetFileName(report, asJson, compressed);

            if (!filename.EndsWith(".zip") && !filename.EndsWith(".json"))
            {
                filename += ".mkkp";
            }

            return filename;
        }
    }
}