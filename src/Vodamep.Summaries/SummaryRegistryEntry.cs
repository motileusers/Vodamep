using Vodamep.ReportBase;

namespace Vodamep.Summaries
{
    public record SummaryRegistryEntry(ReportType Type, string Description)
    {

    }

    public record SummaryRegistryEntry<TReport, TSummaryFactory>(string Description)
       : SummaryRegistryEntry(new TReport().ReportType, Description)
       where TReport : IReport, new()
       where TSummaryFactory : ISummaryFactory<TReport>
    {

    }

    public record SummaryRegistryEntry<TReport, TSummaryModel, TSummaryModelFactory, TSummaryFactory>(string Description)
       : SummaryRegistryEntry(new TReport().ReportType, Description)
       where TReport : IReport, new()
       where TSummaryModelFactory : ISummaryModelFactory<TReport, TSummaryModel>
       where TSummaryFactory : ISummaryFactory<TSummaryModel>
    {

    }
}
