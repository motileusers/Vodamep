using System.Text;
using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public class MinutesPerDiagnosisSummaryFactory : ISummaryFactory<MinutesPerDiagnosisModel>
    {
        public static SummaryRegistryEntry<MkkpReport, MinutesPerDiagnosisModel, MinutesPerDiagnosisModelFactory, MinutesPerDiagnosisSummaryFactory> GetDescription() => new("Liste nach Diagnosen");

        public Task<Summary> Create(MinutesPerDiagnosisModel model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Einsatzzeiten nach Diagnosen");
            sb.AppendLine($"## {model.From:dd.MM.yyyy} - {model.To:dd.MM.yyyy} ");
            sb.AppendLine();

            WriteTable(sb, model);

            var result = new Summary(sb.ToString());

            return Task.FromResult(result);
        }


        private static void WriteTable(StringBuilder sb, MinutesPerDiagnosisModel model)
        {
            const int col1Width = 40;
            const int col2Width = 10;

            static string formatCol(string text, int len) => text.PadRight(len)[..len];

            var scopes = model.Values.Select(x => x.Scope).Distinct()
                .ToDictionary(x => x, x => x switch
                {
                    ActivityScope.ChildCareScope => "MKKP",
                    ActivityScope.PalliativeCareScope => "Palliativ",
                    _ => "???"
                });

            sb.AppendLine($"| {formatCol("Diagnosen", col1Width)} | {string.Join(" | ", scopes.Values.Select(x => formatCol(x, col2Width)))} |");
            sb.AppendLine($"| {new string('-', col1Width)} | {string.Join(" | ", scopes.Values.Select(_ => new string('-', col2Width)))} |");

            foreach (var line in model.Values.GroupBy(x => x.DiagnosisGroups))
            {
                var values = line
                    .GroupBy(x => x.Scope)
                    .ToDictionary(x => x.Key, x => Math.Round(x.Sum(xx => xx.Minues) / 60.0, 2));

                var valueColumns = scopes.Keys.Select(x => formatCol(values.TryGetValue(x, out var v) ? $"{v}" : "", col2Width));

                sb.AppendLine($"| {line.Key} | {string.Join(" | ", valueColumns)} |");
            }

            if (scopes.Count > 1)
            {
                var values = model.Values
                    .GroupBy(x => x.Scope)
                    .ToDictionary(x => x.Key, x => Math.Round(x.Sum(xx => xx.Minues) / 60.0, 2));

                var valueColumns = scopes.Keys.Select(x => formatCol(values.TryGetValue(x, out var v) ? $"{v}" : "", col2Width));

                sb.AppendLine($"| {formatCol("Gesamt", col1Width)} | {string.Join(" | ", valueColumns)} |");
            }

            sb.AppendLine();
            sb.AppendLine($"Gesamtsumme aller Zeiten: {Math.Round(model.Values.Select(x => x.Minues).Sum() / 60.0, 2)}");

        }
    }
}
