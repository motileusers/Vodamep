namespace Vodamep.ReportBase
{
    public interface IStaff : IItem
    {
        string GivenName { get; }
        string FamilyName { get; }
        string GetDisplayName();
    }
}