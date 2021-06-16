namespace Vodamep.ReportBase
{
    public interface ITravelTime : IItem
    {
        int Minutes { get; }

        string StaffId { get; }
    }
}