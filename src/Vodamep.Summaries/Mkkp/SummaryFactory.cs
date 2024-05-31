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

            var result = new Summary(sb.ToString());

            return Task.FromResult(result);
        }

        private static void WritePersonsDataTable1(StringBuilder sb, MkkpReport model)
        {
            const int colWidth1 = 20;
            const int colWidth2 = 10;

            static string formatCol(string text, int len) => text.PadRight(len)[..len];


            foreach (var person in model.Persons)
            {
                var columns = new[]
                {
                    formatCol(person.FamilyName, colWidth1),
                    formatCol(person.GivenName, colWidth1),
                    person.Birthday != null ? person.BirthdayD.ToString("dd.MM.yyyy") : string.Empty,
                    formatCol(person.Postcode, 4),
                    formatCol(person.City, colWidth2),
                    formatCol($"{person.Gender.Format()}", 4)
                };




                sb.AppendLine($"| {string.Join(" | ", columns)} |");
            }
        }

        private static void WritePersonsDataTable2(StringBuilder sb, MkkpReport model)
        {
            const int colWidth1 = 20;
            const int colWidth2 = 10;

            static string formatCol(string text, int len) => text.PadRight(len)[..len];


            foreach (var person in model.Persons)
            {
                var columns = new[]
                {
                    formatCol(person.HospitalDoctor, colWidth1),
                    formatCol(person.Insurance, colWidth1),
                    formatCol(person.LocalDoctor, colWidth1),
                    formatCol(person.OtherReferrer, colWidth1),
                    formatCol($"{person.Referrer}", colWidth1),
                    formatCol($"{person.CareAllowance}", colWidth1),
                    formatCol($"{person.Diagnoses}", colWidth1),
                };

                sb.AppendLine($"| {string.Join(" | ", columns)} |");
            }
        }
    }
}
