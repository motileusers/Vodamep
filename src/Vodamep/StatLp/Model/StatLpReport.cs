using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Vodamep.ReportBase;

namespace Vodamep.StatLp.Model
{
    public partial class StatLpReport : IReport
    {
        public ReportType ReportType => ReportType.StatLp;
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        IInstitution IReport.Institution => this.Institution;

        IList<IPerson> IReport.Persons => this.Persons.Select(x => x as IPerson).ToList();

        public string GetPersonName(string id)
        {
            string client = id;

            var person = this.Persons.FirstOrDefault(p => p.Id == id);

            if (person != null)
            {
                client = $"{person.GivenName} {person.FamilyName}";
            }

            return client;
        }

        public static StatLpReport ReadFile(string file)
        {
            var report = new StatLpReportSerializer().DeserializeFile(file);
            return report;
        }

        public static StatLpReport Read(byte[] data)
        {
            var report = new StatLpReportSerializer().Deserialize(data);
            return report;
        }

        public static StatLpReport Read(Stream data)
        {
            var report = new StatLpReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new StatLpReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new StatLpReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new StatLpReportSerializer().WriteToStream(this, asJson, compressed);

        public string GetSHA256Hash()
        {
            using (var s = SHA256.Create())
            {
                var h = s.ComputeHash(this.ToByteArray());

                var sha256 = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(h));

                return sha256;
            }
        }
        public DiffResult Diff(StatLpReport report) => new StatLpReportDiffer().Diff(this, report);

        public List<DiffObject> DiffList(StatLpReport report) => new StatLpReportDiffer().DiffList(this, report);

    }
}