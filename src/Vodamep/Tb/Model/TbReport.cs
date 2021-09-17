using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Vodamep.ReportBase;

namespace Vodamep.Tb.Model
{
    public partial class TbReport : IReport
    {
        public ReportType ReportType => ReportType.Tb;
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        IInstitution IReport.Institution => this.Institution;

        IList<IPerson> IReport.Persons => this.Persons.Select(x => x as IPerson).ToList();

        public static TbReport ReadFile(string file)
        {
            var report = new TbReportSerializer().DeserializeFile(file);
            return report;
        }

        public static TbReport Read(byte[] data)
        {
            var report = new TbReportSerializer().Deserialize(data);
            return report;
        }

        public static TbReport Read(Stream data)
        {
            var report = new TbReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new TbReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new TbReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new TbReportSerializer().WriteToStream(this, asJson, compressed);

        public string GetSHA256Hash()
        {
            using (var s = SHA256.Create())
            {
                var h = s.ComputeHash(this.ToByteArray());

                var sha256 = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(h));

                return sha256;
            }
        }
        public DiffResult Diff(TbReport report) => new TbReportDiffer().Diff(this, report);

        public List<DiffObject> DiffList(TbReport report) => new TbReportDiffer().DiffList(this, report);

    }
}