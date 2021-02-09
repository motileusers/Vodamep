using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Vodamep.Data.Dummy;
using Vodamep.Hkpv;
using Vodamep.ReportBase;

namespace Vodamep.Mkkp.Model
{
    public partial class MkkpReport : IReportBase
    {
        public string ReportType => "Mkkp";
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        IInstitution IReportBase.Institution => this.Institution;

        public static MkkpReport ReadFile(string file)
        {
            var report = new MkkpReportSerializer().DeserializeFile(file);
            return report;
        }

        public static MkkpReport Read(byte[] data)
        {
            var report = new MkkpReportSerializer().Deserialize(data);
            return report;
        }

        public static MkkpReport Read(Stream data)
        {
            var report = new MkkpReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new MkkpReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new MkkpReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new MkkpReportSerializer().WriteToStream(this, asJson, compressed);

        public string GetSHA256Hash()
        {
            using (var s = SHA256.Create())
            {
                var h = s.ComputeHash(this.ToByteArray());

                var sha256 = System.Net.WebUtility.UrlEncode(Convert.ToBase64String(h));

                return sha256;
            }
        }

        public DiffResult Diff(MkkpReport report) => new MkkpReportDiffer().Diff(this, report);

        public List<DiffObject> DiffList(MkkpReport report) => new MkkpReportDiffer().DiffList(this, report);



    }
}