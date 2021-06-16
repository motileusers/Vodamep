using Vodamep.ReportBase;

namespace Vodamep.StatLp
{
    public class StatLpReportFilenameHandler : ReportFilenameHandler
    {
        public override string GetFileName(IReport report, bool asJson, bool compressed = true)
        {
            var filename = base.GetFileName(report, asJson, compressed);

            if (!filename.EndsWith(".zip") && !filename.EndsWith(".json"))
            {
                filename += ".statlp";
            }

            return filename;
        }
    }
}