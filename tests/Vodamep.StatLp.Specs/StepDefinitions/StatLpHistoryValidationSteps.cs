using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
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
            this.Reports = new List<StatLpReport>();
        }

        public StatLpReport Report { get; private set; }
        public List<StatLpReport> Reports { get; private set; }

        public StatLpReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (StatLpReportValidationResult)Report.ValidateHistory();
                }

                return _result;
            }
        }

        [Given(@"Gesendete Meldung '(.*)' gilt vom '(.*)' bis '(.*)'")]
        public void GivenReportIsValidFromTo(int reportNumber, string dateFrom, string dateTo)
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);

            if (reportNumber == 1)
            {
                this.Report.FromD = from;
                this.Report.ToD = to;
            }
        }

        [Given(@"Gesendete Meldung (.*) von Person (.*) enthält das Attribut '(.*)' mit dem Wert '(.*)' mit Datum '(.*)'")]
        public void GivenPersonPropertyIsSetTo(int reportNumber, string personId, string attributeType, string value, string date)
        {
            if (reportNumber == 1)
            {
                var attribute = new Attribute
                {
                    PersonId = personId,
                    AttributeType = (AttributeType)Enum.Parse(typeof(AttributeType), attributeType),
                    Value = value,
                    FromD = DateTime.Parse(date)
                };

                this.Report.Attributes.Add(attribute);
            }
        }

        [Given(@"Gesendete Meldung (.*) enthält eine '(.*)' von Person (.*) vom '(.*)'")]
        public void GivenPropertyContainsItem(int reportNumber, string itemType, string personId, string date)
        {
            if (reportNumber == 1)
            {
                switch (itemType)
                {
                    case nameof(Admission):

                        var admission = new Admission();
                        admission.PersonId = personId;
                        admission.Valid = DateTime.Parse(date).AsTimestamp();
                        this.Report.Admissions.Add(admission);

                        break;

                    case nameof(Leaving):

                        var leaving = new Leaving();
                        leaving.PersonId = personId;
                        //leaving = DateTime.Parse(date).AsTimestamp();
                        this.Report.Leavings.Add(leaving);

                        break;
                }
            }
        }

        [Given(@"Gesendete Meldung (.*) enthält einen Aufenthalt von Person (.*) vom '(.*)' bis '(.*)'")]
        public void GivenPropertyContainsStay(int reportNumber, string personId, string @from, string to)
        {
            if (reportNumber == 1)
            {
                var stay = new Stay();
                stay.PersonId = personId;
                stay.From = DateTime.Parse(from).AsTimestamp();
                stay.To = DateTime.Parse(to).AsTimestamp();
                this.Report.Stays.Add(stay);
            }
        }

        [Then(@"enthält das History Validierungsergebnis keine Fehler")]
        public void ThenTheResultContainsNoError()
        {
            Assert.True(this.Result.IsValid);
            Assert.Empty(this.Result.Errors.Where(x => x.Severity == Severity.Error));
        }

        [Then(@"enthält das History Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheResultContainsAnError(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
