using Vodamep.ReportBase;

namespace Vodamep.Summaries
{
    public interface ISummaryModelFactory<TReport, TModel>
        where TReport : IReport
    {
        Task<TModel> Create(IEnumerable<TReport> reports);
    }
}