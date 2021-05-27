using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Data.Dummy;
using Vodamep.StatLp.Model;
using Vodamep.StatLp.Validation;

namespace Vodamep.Client
{
    public class StatLpHandler : HandlerBase
    {
        public override void PackFile(PackFileArgs args)
        {
            var report = ReadReport(args.File);

            var file = report.WriteToPath("", asJson: args.Json, compressed: !args.NoCompression);

            Console.WriteLine($"{file} wurde erzeugt.");
        }

        public override void PackRandom(PackRandomArgs args)
        {
            int? year = args.Year;
            if (year < 2000 || year > DateTime.Today.Year) year = null;

            int? month = args.Month;
            if (month < 1 || month > 12) month = null;

            var r = StatLpDataGenerator.Instance.CreateStatLpReport(args.InstitutionId, year, month, args.Persons, args.Staffs, args.AddActivities);

            var file = r.WriteToPath("", asJson: args.Json, compressed: !args.NoCompression);

            Console.WriteLine($"{file} wurde erzeugt.");
        }

        protected override void Send(SendArgs args, string file)
        {
            var report = ReadReport(file);

            var address = args.Address.Trim();

            address = address.EndsWith(@"\") ? address : $"{address}/";

            var sendResult = report.Send(new Uri(address), args.User, args.Password).Result;

            if (!string.IsNullOrEmpty(sendResult?.Message))
            {
                Console.WriteLine(sendResult.Message);
            }

            if (!string.IsNullOrEmpty(sendResult?.ErrorMessage))
            {
                Console.WriteLine(sendResult.ErrorMessage);
            }

            if (!(sendResult?.IsValid ?? false))
            {
                HandleFailure("Fehlgeschlagen. " + sendResult.Message);
            }
            else
            {
                Console.WriteLine("Erfolgreich");
            }

        }

        protected override void Validate(ValidateArgs args, string file)
        {
            var report = ReadReport(file);

            var result = report.Validate();

            var formatter = new StatLpReportValidationResultFormatter(ResultFormatterTemplate.Text, args.IgnoreWarnings);
            var message = formatter.Format(report, result);

            Console.WriteLine(message);
        }


        protected override void ValidateHistory(ValidateArgs args, string[] files)
        {
            List<StatLpReport> reports = new List<StatLpReport>();

            foreach (string file in files)
            {
                var report = ReadReport(file);
                reports.Add(report);
            }

            // der Reihenfolge nach sortieren
            reports = reports.OrderBy(x => x.From).ToList();

            // Es gibt immer einen aktiven Report, der mit einer Geschichte an Reports
            // geprüft wird. In unserem Fall ist das dann immer der letzte, den wir finden.
            StatLpReport lastReport = reports.LastOrDefault();
            reports.Remove(lastReport);

            var result = lastReport.ValidateHistory(reports);

            var formatter = new StatLpReportValidationResultFormatter(ResultFormatterTemplate.Text, args.IgnoreWarnings);
            var message = formatter.Format(lastReport, result);

            Console.WriteLine(message);
        }



        private StatLpReport ReadReport(string file)
        {
            try
            {
                return StatLpReport.ReadFile(file);
            }
            catch (Exception ex)
            {
                HandleFailure("Daten konnten nicht gelesen werden: " + ex.Message);
            }

            return null;
        }
    }
}