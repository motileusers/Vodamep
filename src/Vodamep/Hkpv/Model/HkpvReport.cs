using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Google.Protobuf;
using Vodamep.Data.Dummy;
using Vodamep.ReportBase;

namespace Vodamep.Hkpv.Model
{
    public partial class HkpvReport : IReportBase
    {
        public ReportType ReportType => ReportType.Hkpv;
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        IInstitution IReportBase.Institution => this.Institution;

        public static HkpvReport CreateDummyData()
        {
            var r = new HkpvReport()
            {
                Institution = new Institution()
                {
                    Id = "test",
                    Name = "Test"
                }
            };

            r.FromD = DateTime.Today.FirstDateInMonth().AddMonths(-1);
            r.ToD = r.FromD.LastDateInMonth();

            r.AddDummyPerson();
            r.AddDummyStaff();
            r.AddDummyActivities();

            return r.AsSorted();

        }

        public static HkpvReport ReadFile(string file)
        {
            var report = new HkpvReportSerializer().DeserializeFile(file);
            return report;
        }

        public static HkpvReport Read(byte[] data)
        {
            var report = new HkpvReportSerializer().Deserialize(data);
            return report;
        }

        public static HkpvReport Read(Stream data)
        {
            var report = new HkpvReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToStream(this, asJson, compressed);

        public string GetSHA256Hash()
        {
            using (var s = SHA256.Create())
            {
                var h = s.ComputeHash(this.ToByteArray());

                var sha256 = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(h));

                return sha256;
            }
        }
        public DiffResult Diff(HkpvReport report) => new HkpvReportDiffer().Diff(this, report);

        public List<DiffObject> DiffList(HkpvReport report) => new HkpvReportDiffer().DiffList(this, report);

    }
}