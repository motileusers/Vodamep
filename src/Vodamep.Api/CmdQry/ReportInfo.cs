using System;
using Vodamep.ReportBase;

namespace Vodamep.Api.CmdQry
{
    public class ReportInfo : IEquatable<ReportInfo>
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Institution { get; set; }
        public string HashSHA256 { get; set; }
        public DateTime Created { get; set; }


        public static ReportInfo Create(IReportBase report, int id, DateTime created)
        {
            return new ReportInfo
            {
                Id = id,
                Created = created,
                Month = report.FromD.Month,
                Year = report.FromD.Year,
                Institution = report.Institution.Id,
                HashSHA256 = report.GetSHA256Hash()
            };
        }

        public bool Equals(ReportInfo other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            
            if (Month != other.Month) return false;
            if (Year != other.Year) return false;
            if (Institution != other.Institution) return false;
            if (HashSHA256 != other.HashSHA256) return false;
      
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            if (Id != 0) hash ^= Id.GetHashCode();
            if (Month != 0) hash ^= Month.GetHashCode();
            if (Year != 0) hash ^= Year.GetHashCode();
            if (!string.IsNullOrEmpty(Institution)) hash ^= Institution.GetHashCode();
            if (!string.IsNullOrEmpty(HashSHA256)) hash ^= Institution.GetHashCode();
            if (Created != DateTime.MinValue) hash ^= Created.GetHashCode();

            return hash;
        }
    }
}
