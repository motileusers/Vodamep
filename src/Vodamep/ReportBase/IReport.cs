using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf.WellKnownTypes;

namespace Vodamep.ReportBase
{
    public interface IReport
    {
        ReportType ReportType { get; }
        DateTime FromD { get; }
        Timestamp From { get; }
        DateTime ToD { get; }
        Timestamp To { get; }
        IInstitution Institution { get; }


        /// <summary>
        /// Liefert das Datum, zu dem der vorherige Report gesendet werden sollte
        /// </summary>
        DateTime GetPreviousDate();


        MemoryStream WriteToStream(bool asJson = false, bool compressed = true);
        void WriteToFile(string filename, bool asJson = false, bool compressed = true);
        string GetSHA256Hash();

        IList<IPerson> Persons { get; }
      
    }
}