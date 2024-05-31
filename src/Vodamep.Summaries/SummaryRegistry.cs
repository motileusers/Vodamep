using Vodamep.ReportBase;

namespace Vodamep.Summaries
{
    public class SummaryRegistry
    {

        private readonly List<SummaryRegistryEntry> _entries = [];

        public static SummaryRegistry CreateDefault()
        {
            var result = new SummaryRegistry();

            result.Add(Mkkp.SummaryFactory.GetDescription());
            result.Add(Mkkp.MinutesPerActivityScopeSummaryFactory.GetDescription());

            return result;
        }

        public void Add(SummaryRegistryEntry entry)
        {
            if (entry.GetType().IsGenericType
                && entry.GetType().GetGenericTypeDefinition() == typeof(SummaryRegistryEntry<,,,>)
                && !_entries.Contains(entry)
                )
            {
                _entries.Add(entry);
            }
        }


        public IEnumerable<SummaryRegistryEntry> GetEntries(IReport report)
        {
            if (report == null)
            {
                yield break;
            }

            var reportType = report.GetType();

            foreach (var entry in _entries)
            {
                var type = entry.GetType();

                if (!type.IsGenericType)
                {
                    continue;
                }

                if (type.IsGenericType && type.GetGenericArguments().ElementAtOrDefault(0) == reportType)
                {
                    yield return entry;
                }
            }
        }


        public async Task<Summary?> CreateSummary<T>(SummaryRegistryEntry entry, T report)
            where T : IReport
        {

            var model = await CreateModel<T>(entry, [report]);

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
                var summaryModelFactory = Activator.CreateInstance(summaryModelFactoryType);

                var createModelMethodInfo = summaryModelFactoryType.GetMethod(nameof(ISummaryModelFactory<T, object>.Create)) ?? throw new ArgumentException();

                if (createModelMethodInfo.Invoke(summaryModelFactory, [reports]) is Task t)
                {
                    await t;

                    return ((dynamic)t).Result;
                }
            }

            return null;
        }

        private async Task<Summary?> CreateSummary(SummaryRegistryEntry entry, object model)
        {
            var summaryFactoryType = entry.GetType().GetGenericArguments().Last();
            var summaryFactory = Activator.CreateInstance(summaryFactoryType);

            var createMethodInfo = summaryFactoryType.GetMethod(nameof(ISummaryFactory<object>.Create)) ?? throw new ArgumentException();


            if (createMethodInfo.Invoke(summaryFactory, [model]) is Task<Summary> t)
            {
                await t;

                return t.Result;
            }

            return null;
        }

    }
}
