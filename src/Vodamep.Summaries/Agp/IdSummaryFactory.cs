using System.Text;
using Vodamep.Agp.Model;


namespace Vodamep.Summaries.Agp
{
    public class IdSummaryFactory : ISummaryFactory<AgpReport>, IWithEmployeeNameResolver
    {
        public Func<string, Task<string>> ResolveName { get; set; } = id => Task.FromResult(id);

        public static SummaryRegistryEntry<AgpReport, IdSummaryFactory> GetDescription() => new("Namen der Mitarbeiterinnen mit Id");

        public async Task<Summary> Create(AgpReport model)
        {

            var names = model.Activities.Select(x => x.Id)
                .Union(model.StaffActivities.Select(x => x.Id))
                .Distinct()
                .ToDictionary(x => x, _ => string.Empty);

            foreach (var entry in names)
            {
                names[entry.Key] = await ResolveName(entry.Key);
            }

            var sb = new StringBuilder();
            sb.AppendLine("# Liste Mitarbeiterinnen");
            sb.AppendLine($"## {model.FromD:dd.MM.yyyy} - {model.ToD:dd.MM.yyyy} ");
            sb.AppendLine();


            WriteDataTable(sb, names);

            var result = new Summary(sb.ToString());

            return result;
        }

        static string FormatCol(string text, int len) => text.PadRight(len)[..len];
        static IEnumerable<string> FormatCols(string[] cols, int[] widths) => Enumerable.Range(0, cols.Length).Select(x => x < widths.Length && widths[x] > 0 ? FormatCol(cols[x], widths[x]) : cols[x]);

        private static void WriteDataTable(StringBuilder sb, Dictionary<string, string> names)
        {
            int[] colWidths = [20, 20, 20];

            var headers = FormatCols([
                "Name",
                "Id"
            ], colWidths).ToArray();

            sb.AppendLine($"| {string.Join(" | ", headers)} |");

            sb.AppendLine($"| {string.Join(" | ", headers.Select(x => new string('-', x.Length)))} |");

            foreach (var person in names.OrderBy(x => x.Value))
            {
                var columns = new[]
                {
                    person.Value,
                    person.Key
                };

                sb.AppendLine($"| {string.Join(" | ", FormatCols(columns, colWidths))} |");
            }
        }


    }
}
