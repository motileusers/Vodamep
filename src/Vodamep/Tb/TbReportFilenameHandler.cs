using Vodamep.ReportBase;

namespace Vodamep.Tb
{
    public class TbReportFilenameHandler : ReportFilenameHandler
    {
        public override string GetFileName(IReportBase report, bool asJson, bool compressed = true)
        {
            var filename = base.GetFileName(report, asJson, compressed);

            if (!filename.EndsWith(".zip") && !filename.EndsWith(".json"))
            {
                filename += ".tb";
            }

            return filename;
        }
    }
}