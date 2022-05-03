using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Google.Protobuf;
using Vodamep.ReportBase;

namespace Vodamep.Mkkp.Model
{
    public partial class MkkpReport : ITravelTimeReport
    {
        public ReportType ReportType => ReportType.Mkkp;

        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

        /// <summary>
        /// Liefert das Datum, zu dem der vorherige Report gesendet werden sollte
        /// </summary>
        public DateTime GetPreviousDate()
        {
            return this.FromD.AddMonths(-1);
        }


        IInstitution IReport.Institution => this.Institution;

        IList<IPerson> IReport.Persons => this.Persons.Select(x => x as IPerson).ToList();
        
        IList<IStaff> ITravelTimeReport.Staffs  => this.Staffs.Select(x => x as IStaff).ToList();
        IList<ITravelTime> ITravelTimeReport.TravelTimes  => this.TravelTimes.Select(x => x as ITravelTime).ToList();


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

            if (person != null)
            {
                client = $"{person.GivenName} {person.FamilyName}";
            }

            return client;
        }


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

        public string GetSHA256Hash() => SHAHasher.GetReportHash(this.ToByteArray());


        //public DiffResult Diff(MkkpReport report) => new MkkpReportDiffer().Diff(this, report);

        public List<DiffObject> DiffList(MkkpReport report) => new MkkpReportDiffer().DiffList(this, report);



    }
}