using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public class MkkpReportMergeSummaryModelFactory : ISummaryModelFactory<MkkpReport, MkkpReport>
    {
        public Task<MkkpReport> Create(IEnumerable<MkkpReport> reports) => Task.FromResult(reports.ToArray().Merge());
    }
}
