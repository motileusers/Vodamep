using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Vodamep.ReportBase;

namespace Vodamep.Mohi.Model
{
    public partial class MohiReport : IReport
    {
        public ReportType ReportType => ReportType.Mohi;
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        IInstitution IReport.Institution => this.Institution;

        IList<IPerson> IReport.Persons => this.Persons.Select(x => x as IPerson).ToList();


        public static MohiReport ReadFile(string file)
        {
            var report = new MohiReportSerializer().DeserializeFile(file);
            return report;
        }

        public static MohiReport Read(byte[] data)
        {
            var report = new MohiReportSerializer().Deserialize(data);
            return report;
        }

        public static MohiReport Read(Stream data)
        {
            var report = new MohiReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new MohiReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new MohiReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new MohiReportSerializer().WriteToStream(this, asJson, compressed);

        public string GetSHA256Hash() => SHAHasher.GetReportHash(this.ToByteArray());

        public DiffResult Diff(MohiReport report) => new MohiReportDiffer().Diff(this, report);

        public List<DiffObject> DiffList(MohiReport report) => new MohiReportDiffer().DiffList(this, report);

    }
}