namespace Vodamep.ReportBase
{
    public abstract class ReportFilenameHandler
    {
        public virtual string GetFileName(IReport report, bool asJson, bool compressed = true)
        {

            var filename = $"{report.Institution.Id}_{report.ToD.Year}_{report.ToD.Month.ToString("00")}";

            if (compressed)
                return $"{filename}.zip";
            else if (asJson)
                return $"{filename}.json";
            else
                return filename;
        }
    }
}