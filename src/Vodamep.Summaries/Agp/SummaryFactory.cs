using System.Text;
using Vodamep.Agp.Model;


namespace Vodamep.Summaries.Agp
{
    public class SummaryFactory : ISummaryFactory<AgpReport>, IWithEmployeeNameResolver
    {
        public Func<string, Task<string>> ResolveName { get; set; } = id => Task.FromResult(id);

        public static SummaryRegistryEntry<AgpReport, SummaryFactory> GetDescription() => new("Inhalt");

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
            sb.AppendLine("# Inhalt Datenmeldung");
            sb.AppendLine($"## {model.FromD:dd.MM.yyyy} - {model.ToD:dd.MM.yyyy} ");
            sb.AppendLine();

            sb.AppendLine("### Stammdaten");
            WritePersonsDataTable1(sb, model);
            sb.AppendLine();
            WritePersonsDataTable2(sb, model);
            sb.AppendLine();

            sb.AppendLine("### Einsätze");
            WriteActivities(sb, model, names);
            sb.AppendLine();

            sb.AppendLine("### Mitarbeitereinsätze");
            WriteStaffActivities(sb, model, names);
            sb.AppendLine();

            var result = new Summary(sb.ToString());

            return result;
        }

        static string FormatCol(string text, int len) => text.PadRight(len)[..len];
        static IEnumerable<string> FormatCols(string[] cols, int[] widths) => Enumerable.Range(0, cols.Length).Select(x => x < widths.Length && widths[x] > 0 ? FormatCol(cols[x], widths[x]) : cols[x]);

        private static void WritePersonsDataTable1(StringBuilder sb, AgpReport model)
        {
            int[] colWidths = [20, 20, 10, 4, 10, 10, 10, 10];

            var headers = FormatCols([
                "Nachame",
                "Vorname",
                "Geb.datum",
                "Plz",
                "Ort",
                "Geschl.",
                "Staatsb.",
                "Vers."
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
                    person.Gender.Format(),
                    person.Nationality,
                    person.Insurance,
                };

                sb.AppendLine($"| {string.Join(" | ", FormatCols(columns, colWidths))} |");
            }
        }

        private static void WritePersonsDataTable2(StringBuilder sb, AgpReport model)
        {
            int[] colWidths = [20, 20, 20, 20];

            var headers = FormatCols([
                "Name",
                "Pflegesufe",
                "Zuweiser",
                "Diagnose"
            ], colWidths).ToArray();


            sb.AppendLine($"| {string.Join(" | ", headers)} |");

            sb.AppendLine($"| {string.Join(" | ", headers.Select(x => new string('-', x.Length)))} |");

            foreach (var person in model.Persons.OrderBy(x => x.FamilyName).ThenBy(x => x.GivenName))
            {
                var columns = new[]
                {
                    $"{person.FamilyName} {person.GivenName}",
                    person.CareAllowance.Format(),
                    person.Referrer == Referrer.OtherReferrer ? person.OtherReferrer :  person.Referrer.Format(),
                    
                    
                    string.Join(", ", person.Diagnoses.Select(x => x.Format()))
                };

                sb.AppendLine($"| {string.Join(" | ", FormatCols(columns, colWidths))} |");
            }
        }

        private static void WriteActivities(StringBuilder sb, AgpReport model, Dictionary<string, string> names)
        {
            int[] colWidths = [10, 20, 20, 30, 5, 20];

            var headers = FormatCols([
                "Datum",
                "Name",
                "Einsatzort",
                "Einsatzart",
                "Zeit",
                "Mitarbeiterin"
            ], colWidths).ToArray();


            sb.AppendLine($"| {string.Join(" | ", headers)} |");

            sb.AppendLine($"| {string.Join(" | ", headers.Select(x => new string('-', x.Length)))} |");

            var personNames = new SortedDictionary<string, string>(model.Persons.ToDictionary(x => x.Id, x => $"{x.FamilyName} {x.GivenName}"));

            foreach (var activity in model.Activities.OrderBy(x => x.Date))
            {
                var columns = new[]
                {
                    activity.DateD.ToString("dd.MM.yyyy"),
                    personNames[activity.PersonId],
                    activity.PlaceOfAction.Format(),
                    string.Join(", ", activity.Entries.Select(x => x.Format())),
                    $"{activity.Minutes}",
                    names[activity.Id]
                };

                sb.AppendLine($"| {string.Join(" | ", FormatCols(columns, colWidths))} |");
            }
        }

        private static void WriteStaffActivities(StringBuilder sb, AgpReport model, Dictionary<string, string> names)
        {
            int[] colWidths = [10, 20, 20, 5];

            var headers = FormatCols([
                "Datum",
                "Mitarbeiterin",
                "Einsatzart",
                "Zeit"
            ], colWidths).ToArray();


            sb.AppendLine($"| {string.Join(" | ", headers)} |");

            sb.AppendLine($"| {string.Join(" | ", headers.Select(x => new string('-', x.Length)))} |");

            foreach (var activity in model.StaffActivities)
            {
                var columns = new[]
                {
                    activity.DateD.ToString("dd.MM.yyyy"),
                    names[activity.Id],
                    activity.ActivityType.Format(),
                    $"{activity.Minutes}"
                };

                sb.AppendLine($"| {string.Join(" | ", FormatCols(columns, colWidths))} |");
            }
        }
    }
}
