using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Vodamep.Data.Dummy;
using Vodamep.ReportBase;

namespace Vodamep.Agp.Model
{
    public partial class AgpReport : IReport
    {
        public ReportType ReportType => ReportType.Agp;
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        IInstitution IReport.Institution => this.Institution;

        IList<IPerson> IReport.Persons => this.Persons.Select(x => x as IPerson).ToList();

        public string GetStaffName(string id)
        {
            string client = id;

            var staff = this.Staffs.FirstOrDefault(p => p.Id == id);

            if (staff != null)
            {
                client = $"{staff.GivenName} {staff.FamilyName}";
            }

            return client;
        }

        public string GetClient(string id)
        {
            string client = id;

            var person = this.Persons.FirstOrDefault(p => p.Id == id);

            client = person?.GetDisplayName();

            return client;
        }

        public static AgpReport CreateDummyData()
        {
            var r = new AgpReport()
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

        public static AgpReport ReadFile(string file)
        {
            var report = new AgpReportSerializer().DeserializeFile(file);
            return report;
        }

        public static AgpReport Read(byte[] data)
        {
            var report = new AgpReportSerializer().Deserialize(data);
            return report;
        }

        public static AgpReport Read(Stream data)
        {
            var report = new AgpReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new AgpReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new AgpReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new AgpReportSerializer().WriteToStream(this, asJson, compressed);

        public string GetSHA256Hash() => SHAHasher.GetReportHash(this.ToByteArray());


        public List<DiffObject> DiffList(AgpReport report) => new AgpReportDiffer().DiffList(this, report);


       
    }
}