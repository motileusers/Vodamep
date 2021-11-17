using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Data.Dummy;
using Vodamep.StatLp.Model;
using Vodamep.StatLp.Validation;
using Vodamep.ValidationBase;

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
            int year = args.Year;
            if (year < 2000 || year > DateTime.Today.Year) year = DateTime.Today.Year;

            var r = StatLpDataGenerator.Instance.CreateStatLpReport(args.InstitutionId, year, args.Persons);

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


        protected override void ValidateHistory(ValidateHistoryArgs args, string[] files)
        {
            List<StatLpReport> historyReports = new List<StatLpReport>();

            foreach (string file in files)
            {
                StatLpReport report = ReadReport(file);
                historyReports.Add(report);
            }

            // der Reihenfolge nach sortieren
            historyReports = historyReports.OrderBy(x => x.From).ToList();


            // Es gibt immer einen aktiven Report, der mit einer Geschichte an Reports
            // geprüft wird. Wir können aber viele Reports gegen die Historie prüfen lasen.
            List<StatLpReport> validateReports = new List<StatLpReport>();

            string[] checkFiles = GetFiles(args.File);
            foreach (string file in checkFiles)
            {
                StatLpReport report = ReadReport(file);
                validateReports.Add(report);
            }

            // todo: Clearing aus der DB auslesen
            ClearingExceptions clearingExceptions = new ClearingExceptions();



            validateReports = validateReports.OrderBy(x => x.FromD).ToList();

            foreach (StatLpReport report in validateReports)
            {
                var formatter = new StatLpReportValidationResultFormatter(ResultFormatterTemplate.Text, args.IgnoreWarnings);
                string message = "";

                List<StatLpReport> previousReports = historyReports.Where(x => x.FromD < report.FromD).ToList();

                if (previousReports.Count > 0)
                {
                    var result = report.ValidateHistory(previousReports, clearingExceptions);

                    message = formatter.Format(report, result);

                }
                else
                {
                    message = formatter.Format(report, null);
                }

                Console.WriteLine(message);

            }

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