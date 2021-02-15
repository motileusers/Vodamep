using System;
using Vodamep.Data.Dummy;
using Vodamep.Mkkp.Model;
using Vodamep.Mkkp.Validation;

namespace Vodamep.Client
{
    public class Mkkpandler : HandlerBase
    {
        public override void PackFile(PackFileArgs args)
        {
            var report = ReadReport(args.File);

            var file = report.WriteToPath("", asJson: args.Json, compressed: !args.NoCompression);

            Console.WriteLine($"{file} wurde erzeugt.");
        }

        public override void PackRandom(PackRandomArgs argsBase)
        {
            if (!(argsBase is PackRandomMkkpArgs args))
            {
                throw new ArgumentException();
            }

            var r = MkkpDataGenerator.Instance.CreateMkkpReport();

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

            var formatter = new MkkpReportValidationResultFormatter(ResultFormatterTemplate.Text, args.IgnoreWarnings);
            var message = formatter.Format(report, result);

            Console.WriteLine(message);
        }

        private MkkpReport ReadReport(string file)
        {
            try
            {
                return MkkpReport.ReadFile(file);
            }
            catch (Exception ex)
            {
                HandleFailure("Daten konnten nicht gelesen werden: " + ex.Message);
            }

            return null;
        }
    }
}