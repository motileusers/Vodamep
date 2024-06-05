using System.Collections;
using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public class MkkpReportMergeSummaryModelFactory : ISummaryModelFactory<MkkpReport, MkkpReport>
    {
        public Task<MkkpReport> Create(IEnumerable<MkkpReport> reports) => Task.FromResult(reports.ToArray().Merge());

        async Task<object> ISummaryModelFactory.Create(IEnumerable reports)
        {
            var r = await this.Create(reports != null ? reports.OfType<MkkpReport>() : []);

            return r;
        }
    }
}
