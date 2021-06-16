using Vodamep.ReportBase;

namespace Vodamep.Agp
{
    public class AgpReportFilenameHandler : ReportFilenameHandler
    {
        public override string GetFileName(IReport report, bool asJson, bool compressed = true)
        {
            var filename = base.GetFileName(report, asJson, compressed);

            if (!filename.EndsWith(".zip") && !filename.EndsWith(".json"))
            {
                filename += ".agp";
            }

            return filename;
        }
    }
}