using System;
using System.IO;
using Google.Protobuf.WellKnownTypes;

namespace Vodamep.ReportBase
{
    public interface IReportBase
    {
        string ReportType { get; }
        DateTime FromD { get; }
        Timestamp From { get; }
        IInstitution Institution { get; }
        MemoryStream WriteToStream(bool asJson = false, bool compressed = true);
        void WriteToFile(string filename, bool asJson = false, bool compressed = true);
        string GetSHA256Hash();
    }
}