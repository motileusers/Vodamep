using PowerArgs;
using System;
using System.Collections.Generic;
using System.IO;
using Vodamep.Data;
using Vodamep.Data.Hkpv;

namespace Vodamep.Client
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    [ArgDescription("(dmc) Daten-Meldungs-Client:")]
    public class VodamepProgram
    {
        private HandlerFactory handlerFactory = new HandlerFactory();

        [ArgActionMethod, ArgDescription("Absenden der Meldung.")]
        public void Send(SendArgs args)
        {
            HandlerBase handler = this.handlerFactory.CreateFromType(args.Type);
            handler.Send(args);
        }

        [ArgActionMethod, ArgDescription("Prüfung der Meldung.")]
        public void Validate(ValidateArgs args)
        {
            HandlerBase handler = this.handlerFactory.CreateFromType(args.Type);
            handler.Validate(args);
        }

        [ArgActionMethod, ArgDescription("Meldung neu verpacken.")]
        public void PackFile(PackFileArgs args)
        {
            HandlerBase handler = this.handlerFactory.CreateFromType(args.Type);
            handler.PackFile(args);
        }

        [ArgActionMethod, ArgDescription("Meldung mit Testdaten erzeugen.")]
        public void PackRandom(PackRandomArgs args)
        {
            var handler = this.handlerFactory.CreateFromType(args.Type);
            handler.PackRandom(args);
        }

        [ArgActionMethod, ArgDescription("Listet erlaubte Werte.")]
        public void List(ListArgs args)
        {
            IEnumerable<string> lines = new List<string>();

            switch (args.Source)
            {
                case ListSources.Insurances:
                    lines = InsuranceCodeProvider.Instance.GetCSV();
                    break;

                case ListSources.CountryCodes:
                    lines = CountryCodeProvider.Instance.GetCSV();
                    break;

                case ListSources.Postcode_City:
                    lines = Vodamep.Data.PostcodeCityProvider.Instance.GetCSV();
                    break;

                case ListSources.Qualifications:
                    lines = QualificationCodeProvider.Instance.GetCSV();
                    break;

                default:
                    HandleFailure($"Source '{args.Source}' not implemented.");
                    return;
            }

            foreach (var line in lines)
                Console.WriteLine(line);
        }

        protected void HandleFailure(string message = null)
        {
            throw new Exception(message);
        }

    }

}
