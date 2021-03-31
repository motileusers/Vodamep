using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Vodamep.ReportBase;

namespace Vodamep.Cm.Model
{
    public partial class CmReport : IReportBase
    {
        public ReportType ReportType => ReportType.Cm;
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        IInstitution IReportBase.Institution => this.Institution;

        IList<IPerson> IReportBase.Persons => this.Persons.Select(x => x as IPerson).ToList();

        public static CmReport ReadFile(string file)
        {
            var report = new CmReportSerializer().DeserializeFile(file);
            return report;
        }

        public static CmReport Read(byte[] data)
        {
            var report = new CmReportSerializer().Deserialize(data);
            return report;
        }

        public static CmReport Read(Stream data)
        {
            var report = new CmReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new CmReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new CmReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new CmReportSerializer().WriteToStream(this, asJson, compressed);

        public string GetSHA256Hash()
        {
            using (var s = SHA256.Create())
            {
                var h = s.ComputeHash(this.ToByteArray());

                var sha256 = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(h));

                return sha256;
            }
        }
        public DiffResult Diff(CmReport report) => new CmReportDiffer().Diff(this, report);

        public List<DiffObject> DiffList(CmReport report) => new CmReportDiffer().DiffList(this, report);

    }
}