using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public class MinutesPerActivityScopeModelFactory : ISummaryModelFactory<MkkpReport, MinutesPerActivityScopeModel>
    {
        public Task<MinutesPerActivityScopeModel> Create(IEnumerable<MkkpReport> reports)
        {

            var from = reports.Select(x => x.FromD).Min();
            var to = reports.Select(x => x.ToD).Max();

            var names = reports
                .SelectMany(x => x.Persons)
                .GroupBy(x => x.Id)
                .Select(x => (x.Key, x.Select(xx => $"{xx.FamilyName} {xx.GivenName}").Last()))
                .ToArray();

            var values = reports.SelectMany(x => x.Activities)
               .GroupBy(x => (x.PersonId, x.ActivityScope))
               .Select(x =>
               (
                    x.Key.PersonId,
                    x.Key.ActivityScope,
                    x.Sum(x => x.Minutes)
               )).ToArray();


            var result = new MinutesPerActivityScopeModel(from, to, names, values);

            return Task.FromResult(result);
        }
    }
}
