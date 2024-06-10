using System.Collections;
using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public class MinutesPerDiagnosisModelFactory : ISummaryModelFactory<MkkpReport, MinutesPerDiagnosisModel>
    {
        public Task<MinutesPerDiagnosisModel?> Create(IEnumerable<MkkpReport> reports)
        {

            var from = reports.Select(x => x.FromD).Min();
            var to = reports.Select(x => x.ToD).Max();

            List<(string DiagnosisGroups, ActivityScope Scope, int Minutes)> result = [];

            foreach (var report in reports)
            {
                var diagnosisGroups = report
                    .Persons.Select(x => new { PersonId = x.Id, DiagnosisGroups = x.Diagnoses.OrderBy(x => x).ToArray() })
                    .ToDictionary(x => x.PersonId, x => string.Join(",", x.DiagnosisGroups.Select(xx => xx.Localize())));

                result.AddRange(report.Activities
                    .GroupBy(x => (diagnosisGroups[x.PersonId], x.ActivityScope))
                    .Select(x =>
                    (
                        x.Key.Item1,
                        x.Key.ActivityScope,
                        x.Sum(x => x.Minutes)
                    )));
            }

            var values = result.OrderBy(x => x.DiagnosisGroups).GroupBy(x => (x.DiagnosisGroups, x.Scope)).Select(x => (x.Key.DiagnosisGroups, x.Key.Scope, x.Sum(x => x.Minutes))).ToArray();

            return Task.FromResult<MinutesPerDiagnosisModel?>(new MinutesPerDiagnosisModel(from, to, values));
        }

        async Task<object?> ISummaryModelFactory.Create(IEnumerable reports)
        {
            var r = await this.Create(reports != null ? reports.OfType<MkkpReport>() : []);

            return r;
        }

    }
}
