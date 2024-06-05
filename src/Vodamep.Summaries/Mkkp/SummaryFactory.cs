using System.Text;
using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    public class SummaryFactory : ISummaryFactory<MkkpReport>
    {
        public static SummaryRegistryEntry<MkkpReport, MkkpReport, MkkpReportMergeSummaryModelFactory, SummaryFactory> GetDescription() => new("Inhalt");

        public Task<Summary> Create(MkkpReport model)
        {
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
            WriteActivityTable(sb, model);
            sb.AppendLine();


            sb.AppendLine("### Fahrzeit");
            WriteTravelTimesTable(sb, model);
            sb.AppendLine();

            var result = new Summary(sb.ToString());

            return Task.FromResult(result);
        }

        private static void WriteActivityTable(StringBuilder sb, MkkpReport model)
        {
            var persons = model.Persons.ToDictionary(x => x.Id, x => $"{x.FamilyName} {x.GivenName}");
            var staffs = model.Staffs.ToDictionary(x => x.Id, x => $"{x.FamilyName} {x.GivenName}");

            int[] colWidths = [10, 20, 20, 10, 10, 10];

            var headers = FormatCols([
                "Datum",
                "Person",
                "MA",
                "Art",
                "Ort",
                "Zeit"
                ], colWidths).ToArray();

            sb.AppendLine($"| {string.Join(" | ", headers)} |");

            sb.AppendLine($"| {string.Join(" | ", headers.Select(x => new string('-', x.Length)))} |");


            foreach (var activity in model.Activities)
            {
                string[] cols = [
                    $"{activity.DateD:dd.MM.yyyy}",
                    model.GetClient(activity.PersonId),
                    model.GetStaffName(activity.StaffId),
                    string.Join(",",activity.Entries.Select(x => (int)x)),
                    $"{(int)activity.PlaceOfAction}",
                    $"{activity.Minutes}"
                    ];


                sb.AppendLine($"| {string.Join(" | ", FormatCols(cols, colWidths))} |");
            }

        }

        private static void WriteTravelTimesTable(StringBuilder sb, MkkpReport model)
        {

            var staffs = model.Staffs.ToDictionary(x => x.Id, x => $"{x.FamilyName} {x.GivenName}");

            int[] colWidths = [20, 10, 5];

            var headers = FormatCols([
                "MA",
                "Datum",
                "Zeit"
                ], colWidths).ToArray();

            sb.AppendLine($"| {string.Join(" | ", headers)} |");

            sb.AppendLine($"| {string.Join(" | ", headers.Select(x => new string('-', x.Length)))} |");


            foreach (var entriesByStaff in model.TravelTimes.GroupBy(x => x.StaffId))
            {
                string[] cols1 = [model.GetStaffName(entriesByStaff.Key), $"", $""];

                sb.AppendLine($"| {string.Join(" | ", FormatCols(cols1, colWidths))} |");

                foreach (var entry in entriesByStaff)
                {
                    string[] cols = [
                        "",
                        $"{entry.DateD:dd.MM.yyyy}",
                        $"{entry.Minutes}"
                    ];

                    sb.AppendLine($"| {string.Join(" | ", FormatCols(cols, colWidths))} |");
                }
            }
        }

        static string FormatCol(string text, int len) => text.PadRight(len)[..len];
        static IEnumerable<string> FormatCols(string[] cols, int[] widths) => Enumerable.Range(0, cols.Length).Select(x => x < widths.Length && widths[x] > 0 ? FormatCol(cols[x], widths[x]) : cols[x]);

        private static void WritePersonsDataTable1(StringBuilder sb, MkkpReport model)
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
                    person.Birthday != null && person.BirthdayD.Year > 1 ? person.BirthdayD.ToString("dd.MM.yyyy") : string.Empty,
                    person.Postcode,
                    person.City,
                    $"{person.Gender.Localize()}"
                };

                sb.AppendLine($"| {string.Join(" | ", FormatCols(columns, colWidths))} |");
            }
        }


        private static void WritePersonsDataTable2(StringBuilder sb, MkkpReport model)
        {
            int[] colWidths = [20, 20, 20, 20, 20, -1];

            var headers = FormatCols([
                "Name",
                "Arzt",
                "KH Arzt",
                "Zuweiser",
                "Pflegestufe",
                "Diagnosen"
            ], colWidths).ToArray();

            sb.AppendLine($"| {string.Join(" | ", headers)} |");

            sb.AppendLine($"| {string.Join(" | ", headers.Select(x => new string('-', x.Length)))} |");

            foreach (var person in model.Persons.OrderBy(x => x.FamilyName).ThenBy(x => x.GivenName))
            {
                var columns = new[]
                {
                    $"{person.FamilyName} {person.GivenName}",
                    person.LocalDoctor,
                    person.HospitalDoctor,
                    person.Referrer == Referrer.OtherReferrer ? person.OtherReferrer : person.Referrer.Localize(),
                    person.CareAllowance.Localize(),
                    string.Join(',', person.Diagnoses.Select(x => (int)x))  // als Text zu lange
                };

                sb.AppendLine($"| {string.Join(" | ", FormatCols(columns, colWidths))} |");
            }
        }
    }
}
