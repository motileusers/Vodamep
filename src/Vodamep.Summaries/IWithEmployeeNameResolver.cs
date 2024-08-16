namespace Vodamep.Summaries
{
    // Zusammen mit SummaryRegistry.AddConfiguration ermöglicht diese Schnittstelle,
    // Mitarbeiternamen anhand der Id aufzulösen
    // auch wenn diese nicht Teil des Paketes sind.
    public interface IWithEmployeeNameResolver
    {
        Func<string, Task<string>> ResolveName { get; set; }
    }
}
