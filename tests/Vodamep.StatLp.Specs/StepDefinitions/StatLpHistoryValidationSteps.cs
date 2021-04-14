using FluentValidation;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using TechTalk.SpecFlow;
using Vodamep.Data.Dummy;
using Vodamep.StatLp.Model;
using Vodamep.StatLp.Validation;
using Xunit;
using Attribute = Vodamep.StatLp.Model.Attribute;
using Enum = System.Enum;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class StatLpHistoryValidationSteps
    {

        private StatLpReportValidationResult _result;

        public StatLpHistoryValidationSteps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new DisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            var date = new DateTime(2021, 02, 01);
            this.Report = StatLpDataGenerator.Instance.CreateStatLpReport("", date.Year, date.Month, 1, 1, false);
        }

        public StatLpReport Report { get; private set; }

        public StatLpReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (StatLpReportValidationResult)Report.Validate();
                }

                return _result;
            }
        }

        [Given(@"Gesendete Meldung (.*) gilt vom (.*)\.(.*)\.(.*) bis (.*)\.(.*)\.(.*)")]
        public void AngenommenGesendeteMeldungGiltVom_Bis_(int p0, int p1, int p2, int p3, int p4, int p5, int p6)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"Gesendete Meldung (.*) von Person (.*) enthält das Attribut '(.*)' mit dem Wert '(.*)' mit Datum '(.*)'")]
        public void AngenommenGesendeteMeldungVonPersonEnthaltDasAttributMitDemWertMitDatum(int p0, int p1, string p2, string p3, Decimal p4)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"Gesendete Meldung (.*) enthält eine Aufnahme von Person (.*) vom (.*)\.(.*)\.(.*)")]
        public void AngenommenGesendeteMeldungEnthaltEineAufnahmeVonPersonVom_(int p0, int p1, int p2, int p3, int p4)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"Gesendete Meldung (.*) enthält eine Aufenthalt von Person (.*) vom (.*)\.(.*)\.(.*) bis (.*)\.(.*)\.(.*)")]
        public void AngenommenGesendeteMeldungEnthaltEineAufenthaltVonPersonVom_Bis_(int p0, int p1, int p2, int p3, int p4, int p5, int p6, int p7)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"enthält das History Validierungsergebnis den Fehler '(.*)'")]
        public void DannEnthaltDasHistoryValidierungsergebnisDenFehler(string p0)
        {
            ScenarioContext.Current.Pending();
        }




    }
}
