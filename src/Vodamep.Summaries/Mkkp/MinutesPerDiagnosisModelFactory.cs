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

            var diagnosisGroups = reports
                .SelectMany(x => x.Persons)
                .GroupBy(x => x.Id)
                .Select(x => (x.Key, x.Select(xx => xx.Diagnoses.ToArray()).Last()))
                .ToArray();

            var values = reports.SelectMany(x => x.Activities)
               .GroupBy(x => (x.PersonId, x.ActivityScope))
               .Select(x =>
               (
                    x.Key.PersonId,
                    x.Key.ActivityScope,
                    x.Sum(x => x.Minutes)
               )).ToArray();


            var result = new MinutesPerDiagnosisModel(from, to, diagnosisGroups, values);

            return Task.FromResult<MinutesPerDiagnosisModel?>(result);
        }

        async Task<object?> ISummaryModelFactory.Create(IEnumerable reports)
        {
            var r = await this.Create(reports != null ? reports.OfType<MkkpReport>() : []);

            return r;
        }

    }
}
