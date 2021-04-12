using Google.Protobuf.WellKnownTypes;

namespace Vodamep.ReportBase
{
    public interface IPersonDateActivity : IPersonActivity
    {
        Timestamp Date { get; }
    }
}