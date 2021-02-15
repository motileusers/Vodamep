using PowerArgs;
using System;
using System.IO;
using Vodamep.Data;

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
            var type = this.CheckType(args.File);
            HandlerBase handler = this.handlerFactory.CreateFromType(type);
            handler.Send(args);
        }

        [ArgActionMethod, ArgDescription("Prüfung der Meldung.")]
        public void Validate(ValidateArgs args)
        {
            var type = this.CheckType(args.File);
            HandlerBase handler = this.handlerFactory.CreateFromType(type);
            handler.Validate(args);
        }

        [ArgActionMethod, ArgDescription("Meldung neu verpacken.")]
        public void PackFile(PackFileArgs args)
        {
            var type = this.CheckType(args.File);
            HandlerBase handler = this.handlerFactory.CreateFromType(type);
            handler.PackFile(args);
        }

        [ArgActionMethod, ArgDescription("Agp Meldung mit Testdaten erzeugen.")]
        public void PackRandomAgp(PackRandomAgpArgs args)
        {
            var handler = this.handlerFactory.CreateFromType(Type.Agp);
            handler.PackRandom(args);
        }

        [ArgActionMethod, ArgDescription("Hkpv Meldung mit Testdaten erzeugen.")]
        public void PackRandomHkpv(PackRandomHkpvArgs args)
        {
            var handler = this.handlerFactory.CreateFromType(Type.Hkpv);
            handler.PackRandom(args);
        }

        [ArgActionMethod, ArgDescription("Mkkp Meldung mit Testdaten erzeugen.")]
        public void PackRandomMkkp(PackRandomMkkpArgs args)
        {
            var handler = this.handlerFactory.CreateFromType(Type.Mkkp);
            handler.PackRandom(args);
        }

        [ArgActionMethod, ArgDescription("Listet erlaubte Werte.")]
        public void List(ListArgs args)
        {
            CodeProviderBase provider = null;

            switch (args.Source)
            {
                case ListSources.Insurances:
                    provider = InsuranceCodeProvider.Instance;
                    break;
                case ListSources.CountryCodes:
                    provider = CountryCodeProvider.Instance;
                    break;
                case ListSources.Postcode_City:
                    provider = Postcode_CityProvider.Instance;
                    break;
                case ListSources.Qualifications:
                    provider = QualificationCodeProvider.Instance;
                    break;
                default:
                    HandleFailure($"Source '{args.Source}' not implemented.");
                    return;
            }

            foreach (var line in provider?.GetCSV())
                Console.WriteLine(line);
        }

        private Type CheckType(string typeName)
        {
            Type type;

            switch (typeName.ToLower())
            {
                case "agp":
                    type = Type.Agp;
                    break;

                case "hkpv":
                    type = Type.Hkpv;
                    break;

                case "mkkp":
                    type = Type.Mkkp;
                    break;

                default:
                    type = Type.Hkpv;
                    break;
            }


            return type;
        }

        protected void HandleFailure(string message = null)
        {
            throw new Exception(message);
        }

    }

}
