using System;
using System.IO;
using Google.Protobuf.WellKnownTypes;

namespace Vodamep.ReportBase
{
    public interface IPerson : IItem
    {
        DateTime BirthdayD { get; set; }
        Timestamp Birthday { get; set; }

        string GetDisplayName();
    }
}