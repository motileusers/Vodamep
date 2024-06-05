using System.Collections;
using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public class MkkpReportMergeSummaryModelFactory : ISummaryModelFactory<MkkpReport, MkkpReport>
    {
        public Task<MkkpReport?> Create(IEnumerable<MkkpReport> reports) => Task.FromResult<MkkpReport?>(reports.ToArray().Merge());

        async Task<object?> ISummaryModelFactory.Create(IEnumerable reports)
        {
            var r = await this.Create(reports != null ? reports.OfType<MkkpReport>() : []);

            return r;
        }
    }

    // Es soll nur ein Report pro Summary zugelassen werden
    public class MkkpReportDontMergeSummaryModelFactory : ISummaryModelFactory<MkkpReport, MkkpReport>
    {
        public Task<MkkpReport?> Create(IEnumerable<MkkpReport> reports) => Task.FromResult(reports.Any() && !reports.Skip(1).Any() ? reports.Single() : null);

        async Task<object?> ISummaryModelFactory.Create(IEnumerable reports)
        {
            var r = await this.Create(reports != null ? reports.OfType<MkkpReport>() : []);

            return r;
        }
    }
}
