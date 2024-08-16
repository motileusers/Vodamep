using Vodamep.ReportBase;

namespace Vodamep.Summaries
{
    public class SummaryRegistry
    {

        private readonly List<SummaryRegistryEntry> _entries = [];
        private readonly List<(Type ReportType, Func<object, Task<object>> Configure)> _summaryConfigurations = [];

        public static SummaryRegistry CreateDefault()
        {
            var result = new SummaryRegistry();

            result.Add(Mkkp.SummaryFactory.GetDescription());
            result.Add(Mkkp.MinutesPerActivityScopeSummaryFactory.GetDescription());

            result.Add(Mkkp.MinutesPerDiagnosisSummaryFactory.GetDescription());

            //später result.Add(Hkpv.SummaryFactory.GetDescription());
            result.Add(Agp.SummaryFactory.GetDescription());
            result.Add(Agp.IdSummaryFactory.GetDescription());

            return result;
        }

        public void Add(SummaryRegistryEntry entry)
        {
            if (entry.GetType().IsGenericType
                && (entry.GetType().GetGenericTypeDefinition() == typeof(SummaryRegistryEntry<,,,>)
                || entry.GetType().GetGenericTypeDefinition() == typeof(SummaryRegistryEntry<,>))
                && !_entries.Contains(entry)
                )
            {
                _entries.Add(entry);
            }
        }

        public void AddConfiguration<T>(Func<ISummaryFactory<T>, Task<ISummaryFactory<T>>> configuration) where T : IReport
        {

            async Task<object> translate(object x)
            {
                if (x is ISummaryFactory<T> s)
                {
                    return await configuration(s);
                }

                return x;
            }

            _summaryConfigurations.Add((typeof(T), translate));
        }

        public SummaryRegistryEntry[] GetEntries(ReportType reportType) => _entries.Where(x => x.Type == reportType).ToArray();


        public async Task<Summary?> CreateSummary<T>(SummaryRegistryEntry entry, params T[] reports)
            where T : IReport
        {

            var model = await CreateModel(entry, reports);

            if (model != null)
            {
                var summary = await CreateSummary(entry, model);

                return summary;
            }

            return null;
        }


        private async Task<object?> CreateModel<T>(SummaryRegistryEntry entry, T[] reports)
            where T : IReport
        {

            if (entry.GetType().GetGenericTypeDefinition() == typeof(SummaryRegistryEntry<,>))
            {
                if (reports.Length == 1)
                {
                    return reports[0];
                }

                throw new NotImplementedException("merge multiple reports into one!");
            }

            if (entry.GetType().GetGenericTypeDefinition() == typeof(SummaryRegistryEntry<,,,>))
            {
                var summaryModelFactoryType = entry.GetType().GetGenericArguments()[2];

                if (Activator.CreateInstance(summaryModelFactoryType) is ISummaryModelFactory summaryModelFactory)
                {
                    var result = await summaryModelFactory.Create(reports);

                    return result;
                }
            }

            return null;
        }

        private async Task<Summary?> CreateSummary(SummaryRegistryEntry entry, object model)
        {
            var reportType = entry.GetType().GetGenericArguments().First();
            var summaryFactoryType = entry.GetType().GetGenericArguments().Last();
            var summaryFactory = Activator.CreateInstance(summaryFactoryType);

            foreach (var configuration in _summaryConfigurations.Where(x => x.ReportType == reportType))
            {
                if (summaryFactory != null)
                {
                    summaryFactory = await configuration.Configure(summaryFactory);
                }
            }

            if (summaryFactory != null)
            {
                var createMethodInfo = summaryFactoryType.GetMethod(nameof(ISummaryFactory<object>.Create)) ?? throw new ArgumentException();

                if (createMethodInfo.Invoke(summaryFactory, [model]) is Task<Summary> t)
                {
                    await t;

                    return t.Result;
                }
            }
            return null;
        }
    }
}
