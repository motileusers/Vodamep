namespace Vodamep.ReportBase
{
    public interface INamedPerson : IPerson
    {
        string FamilyName { get; }
        string GivenName { get; }
        string Country { get; }

    }
}