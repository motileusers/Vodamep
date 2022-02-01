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


        public override void ValidatePrevious(ValidatePreviousArgs args)
        {
            StatLpReport report = ReadReport(args.File);
            StatLpReport previous = ReadReport(args.PreviousFile);

            var result = report.Validate(previous);

            var formatter = new StatLpReportValidationResultFormatter(ResultFormatterTemplate.Text, args.IgnoreWarnings);
            var message = formatter.Format(report, result);

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