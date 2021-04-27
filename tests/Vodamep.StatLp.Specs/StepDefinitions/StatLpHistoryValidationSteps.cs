using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
            this.SentReport = StatLpDataGenerator.Instance.CreateEmptyStatLpReport();
            this.ExistingReports = new List<StatLpReport>();
        }

        public StatLpReport SentReport { get; private set; }
        public List<StatLpReport> ExistingReports { get; private set; }

        public StatLpReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (StatLpReportValidationResult)SentReport.ValidateHistory(this.ExistingReports);
                }

                return _result;
            }
        }

        [Given(@"Gesendete Meldung '(.*)' gilt vom '(.*)' bis '(.*)'")]
        public void GivenSentReportIsValidFromTo(int reportNumber, string dateFrom, string dateTo)
        {
            this.GivenReportIsValidFromTo(-1, dateFrom, dateTo);
        }

        [Given(@"Existierende Meldung '(.*)' gilt vom '(.*)' bis '(.*)'")]
        public void GivenExistingReportIsValidFromTo(int reportNumber, string dateFrom, string dateTo)
        {
            this.GivenReportIsValidFromTo(reportNumber - 1, dateFrom, dateTo);
        }

        [Given(@"Gesendete Meldung '(.*)' von Person (.*) enthält das Attribut '(.*)' mit dem Wert '(.*)' mit Datum '(.*)'")]
        public void GivenSentPersonPropertyIsSetTo(int reportNumber, string personId, string attributeType, string value, string date)
        {
            this.GivenPersonPropertyIsSetTo(-1, personId, attributeType, value, date);
        }

        [Given(@"Existierende Meldung '(.*)' von Person (.*) enthält das Attribut '(.*)' mit dem Wert '(.*)' mit Datum '(.*)'")]
        public void GivenExistingPersonPropertyIsSetTo(int reportNumber, string personId, string attributeType, string value, string date)
        {
            this.GivenPersonPropertyIsSetTo(reportNumber - 1, personId, attributeType, value, date);
        }

        [Given(@"Gesendete Meldung '(.*)' enthält eine '(.*)' von Person (.*) vom '(.*)'")]
        public void GivenSentPropertyContainsItem(int reportNumber, string itemType, string personId, string date)
        {
            this.GivenPropertyContainsItem(-1, itemType, personId, date);
        }

        [Given(@"Existierende Meldung '(.*)' enthält eine '(.*)' von Person (.*) vom '(.*)'")]
        public void GivenExistingPropertyContainsItem(int reportNumber, string itemType, string personId, string date)
        {
            this.GivenPropertyContainsItem(reportNumber - 1, itemType, personId, date);
        }

        [Given(@"Gesendete Meldung '(.*)' enthält einen Aufenthalt von Person (.*) vom '(.*)' bis '(.*)'")]
        public void GivenSentPropertyContainsStay(int reportNumber, string personId, string @from, string to)
        {
            this.GivenPropertyContainsStay(-1, personId, from, to);
        }

        [Given(@"Existierende Meldung '(.*)' enthält einen Aufenthalt von Person (.*) vom '(.*)' bis '(.*)'")]
        public void GivenExistingPropertyContainsStay(int reportNumber, string personId, string @from, string to)
        {
            this.GivenPropertyContainsStay(reportNumber - 1, personId, from, to);
        }

        [Then(@"enthält das History Validierungsergebnis keine Fehler")]
        public void ThenTheResultContainsNoError()
        {
            Assert.True(this.Result.IsValid);
            Assert.Empty(this.Result.Errors.Where(x => x.Severity == Severity.Error));
        }

        [Then(@"enthält das escapte History Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheEscapedResultContainsAnErrorRegex(string message)
        {
            var pattern = new Regex(Regex.Escape(message), RegexOptions.IgnoreCase);

            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Error && pattern.IsMatch(x.ErrorMessage)));
        }

        [Then(@"enthält das History Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheResultContainsAnError(string message)
        {
            var pattern = new Regex(message, RegexOptions.IgnoreCase);

            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Error && pattern.IsMatch(x.ErrorMessage)));
        }

        private StatLpReport GetReportAndCreateNonExisting(int index)
        {
            if (index < 0)
            {
                return this.SentReport;
            }
            else
            {
                while (index >= this.ExistingReports.Count)
                {
                    this.ExistingReports.Add(StatLpDataGenerator.Instance.CreateEmptyStatLpReport());
                }

                return this.ExistingReports[index];
            }
        }

        private void GivenReportIsValidFromTo(int reportIndex, string dateFrom, string dateTo)
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);

            var report = this.GetReportAndCreateNonExisting(reportIndex);

            report.FromD = from;
            report.ToD = to;
        }

        private void GivenPersonPropertyIsSetTo(int reportIndex, string personId, string attributeType, string value, string date)
        {
            var attribute = new Attribute
            {
                PersonId = personId,
                AttributeType = (AttributeType)Enum.Parse(typeof(AttributeType), attributeType),
                Value = value,
                FromD = DateTime.Parse(date)
            };

            this.GetReportAndCreateNonExisting(reportIndex).Attributes.Add(attribute);
        }

        private void GivenPropertyContainsItem(int reportIndex, string itemType, string personId, string date)
        {
            var report = this.GetReportAndCreateNonExisting(reportIndex);

            switch (itemType)
            {
                case nameof(Admission):

                    var admission = new Admission();
                    admission.PersonId = personId;
                    admission.Valid = DateTime.Parse(date).AsTimestamp();
                    report.Admissions.Add(admission);

                    break;

                case nameof(Leaving):

                    var leaving = new Leaving();
                    leaving.PersonId = personId;
                    //leaving = DateTime.Parse(date).AsTimestamp();
                    report.Leavings.Add(leaving);

                    break;

            }
        }

        private void GivenPropertyContainsStay(int reportIndex, string personId, string @from, string to)
        {
            var report = this.GetReportAndCreateNonExisting(reportIndex);

            var stay = new Stay();
            stay.PersonId = personId;
            stay.From = DateTime.Parse(from).AsTimestamp();
            stay.To = DateTime.Parse(to).AsTimestamp();
            report.Stays.Add(stay);
        }
    }
}
