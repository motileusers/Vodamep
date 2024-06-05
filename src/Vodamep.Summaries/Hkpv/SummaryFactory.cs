using System.Text;
using Vodamep.Hkpv.Model;

namespace Vodamep.Summaries.Hkpv
{
    public class SummaryFactory : ISummaryFactory<HkpvReport>
    {
        public static SummaryRegistryEntry<HkpvReport, SummaryFactory> GetDescription() => new("Inhalt");

        public Task<Summary> Create(HkpvReport model)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Inhalt Datenmeldung");
            sb.AppendLine($"## {model.FromD:dd.MM.yyyy} - {model.ToD:dd.MM.yyyy} ");
            sb.AppendLine();

            sb.AppendLine("### Stammdaten");
            WritePersonsDataTable1(sb, model);

            //todo: Einsätze etc.

            var result = new Summary(sb.ToString());

            return Task.FromResult(result);
        }

        static string FormatCol(string text, int len) => text.PadRight(len)[..len];
        static IEnumerable<string> FormatCols(string[] cols, int[] widths) => Enumerable.Range(0, cols.Length).Select(x => x < widths.Length && widths[x] > 0 ? FormatCol(cols[x], widths[x]) : cols[x]);


        private static void WritePersonsDataTable1(StringBuilder sb, HkpvReport model)
        {
            int[] colWidths = [20, 20, 10, 4, 10, 10];

            var headers = FormatCols([
                "Nachame",
                "Vorname",
                "Geb.datum",
                "Plz",
                "Ort",
                "Geschlecht"
            ], colWidths).ToArray();


            sb.AppendLine($"| {string.Join(" | ", headers)} |");

            sb.AppendLine($"| {string.Join(" | ", headers.Select(x => new string('-', x.Length)))} |");

            foreach (var person in model.Persons.OrderBy(x => x.FamilyName).ThenBy(x => x.GivenName))
            {
                var columns = new[]
                {
                    person.FamilyName,
                    person.GivenName,
                    person.Birthday != null ? person.BirthdayD.ToString("dd.MM.yyyy") : string.Empty,
                    person.Postcode,
                    person.City,
                    $"{person.Gender.Format()}",
                };

                sb.AppendLine($"| {string.Join(" | ", FormatCols(columns, colWidths))} |");
            }
        }
    }
}
