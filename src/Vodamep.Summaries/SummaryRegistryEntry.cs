using Vodamep.ReportBase;

namespace Vodamep.Summaries
{
    public record SummaryRegistryEntry(string Description)
    {

    }

    public record SummaryRegistryEntry<TReport, TSummaryFactory>(string Description)
       : SummaryRegistryEntry(Description)
       where TReport : IReport
       where TSummaryFactory : ISummaryFactory<TReport>
    {

    }

    public record SummaryRegistryEntry<TReport, TSummaryModel, TSummaryModelFactory, TSummaryFactory>(string Description)
       : SummaryRegistryEntry(Description)
       where TReport : IReport
       where TSummaryModelFactory : ISummaryModelFactory<TReport, TSummaryModel>
       where TSummaryFactory : ISummaryFactory<TSummaryModel>
    {

    }
}
