using System;
using Google.Protobuf.WellKnownTypes;

namespace Vodamep.ReportBase
{
    public interface IClientActivity : IPersonId
    {
        float Minutes { get; }
        Timestamp Date { get; }
    }
}