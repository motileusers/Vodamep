using System.Text;
using System.Xml.Linq;
using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public class MinutesPerActivityScopeSummaryFactory : ISummaryFactory<MinutesPerActivityScopeModel>
    {
        public static SummaryRegistryEntry<MkkpReport, MinutesPerActivityScopeModel, MinutesPerActivityScopeModelFactory, MinutesPerActivityScopeSummaryFactory> GetDescription() => new("Liste nach Personen");

        public Task<Summary> Create(MinutesPerActivityScopeModel model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Einsatzzeiten nach betreuten Personen");
            sb.AppendLine($"## {model.From:dd.MM.yyyy} - {model.To:dd.MM.yyyy} ");
            sb.AppendLine();

            WriteTable(sb, model);

            var result = new Summary(sb.ToString());

            return Task.FromResult(result);
        }


        private static void WriteTable(StringBuilder sb, MinutesPerActivityScopeModel model)
        {
            const int col1Width = 20;
            const int col2Width = 10;

            static string formatCol(string text, int len) => text.PadRight(len)[..len];

            var scopes = model.Values.Select(x => x.Scope).Distinct()
                .ToDictionary(x => x, x => x switch
                {
                    ActivityScope.ChildCareScope => "MKKP",
                    ActivityScope.PalliativeCareScope => "Palliativ",
                    _ => "???"
                });

            sb.AppendLine($"| {formatCol("Person", col1Width)} | {string.Join(" | ", scopes.Values.Select(x => formatCol(x, col2Width)))} |");
            sb.AppendLine($"| {new string('-', col1Width)} | {string.Join(" | ", scopes.Values.Select(_ => new string('-', col2Width)))} |");

            foreach (var (id, name) in model.Names)
            {
                var values = model.Values
                    .Where(x => x.Id == id)
                    .GroupBy(x => x.Scope)
                    .ToDictionary(x => x.Key, x => x.Sum(xx => xx.Minues));

                var valueColumns = scopes.Keys.Select(x => formatCol(values.TryGetValue(x, out var v) ? $"{v}" : "", col2Width));

                sb.AppendLine($"| {formatCol(name, col1Width)} | {string.Join(" | ", valueColumns)} |");
            }

            if (scopes.Count > 1)
            {
                var values = model.Values
                    .GroupBy(x => x.Scope)
                    .ToDictionary(x => x.Key, x => x.Sum(xx => xx.Minues));

                var valueColumns = scopes.Keys.Select(x => formatCol(values.TryGetValue(x, out var v) ? $"{v}" : "", col2Width));

                sb.AppendLine($"| {formatCol("Gesamt", col1Width)} | {string.Join(" | ", valueColumns)} |");
            }

            sb.AppendLine();
            sb.AppendLine($"Gesamtsumme aller Zeiten: {model.Values.Select(x => x.Minues).Sum()}");

        }
    }
}
