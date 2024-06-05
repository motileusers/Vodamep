using System.Collections;
using Vodamep.ReportBase;

namespace Vodamep.Summaries
{
    public interface ISummaryModelFactory
    {
        Task<object> Create(IEnumerable reports);
    }

    public interface ISummaryModelFactory<TReport, TModel> : ISummaryModelFactory
        where TReport : IReport
    {
        Task<TModel> Create(IEnumerable<TReport> reports);
    }
}