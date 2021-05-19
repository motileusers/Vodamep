using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Google.Protobuf;
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

                    foreach (var error in SentReport.Validate().Errors)
                    {
                        _result.Errors.Add(error);
                    }

                    foreach (var existingReport in ExistingReports)
                    {
                        foreach (var error in existingReport.Validate().Errors)
                        {
                            _result.Errors.Add(error);
                        }

                    }
                }

                return _result;
            }
        }

        [Given(@"Im Meldungsbereich für eine Einrichtung befinden sich monatliche Meldungen vom '(.*)' bis '(.*)'")]
        public void GivenThereAreMonthlyReports(string dateFrom, string dateTo)
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);

            var reports = new List<StatLpReport>();

            for (var toDate = to.AddMonths(-1); toDate > from; toDate = toDate.AddMonths(-1))
            {
                var report = StatLpDataGenerator.Instance.CreateEmptyStatLpReport();
                report.FromD = new DateTime(toDate.Year, toDate.Month, 1);
                report.ToD = new DateTime(toDate.Year, toDate.Month, DateTime.DaysInMonth(toDate.Year, toDate.Month));
                reports.Add(report);
            }

            this.SentReport = StatLpDataGenerator.Instance.CreateEmptyStatLpReport();
            this.SentReport.FromD = new DateTime(to.Year, to.Month, 1);
            this.SentReport.ToD = to;
            this.ExistingReports = reports;
        }

        [Given(@"Im Meldungsbereich für eine Einrichtung befinden sich Meldungen von (.*) Klienten")]
        public void GivenThereArePersons(int numberOfPersons)
        {
            for (var i = 0; i < numberOfPersons; i++)
            {
                var person = StatLpDataGenerator.Instance.CreatePerson(i + 1, true);

                this.SentReport.Persons.Add(person);

                foreach (var existingReport in this.ExistingReports)
                {
                    existingReport.Persons.Add(person);
                }
            }
        }

        [Given(@"Im Meldungsbereich für eine Einrichtung wird jeder Klient (.*) x aufgenommen und entlassen")]
        public void GivenEveryClientsAdmissionStaysAndLeavings(int numberOfStays)
        {
            var random = new Random(DateTime.Now.Millisecond);

            var firstDate = this.ExistingReports.Last().FromD;

            var duration = this.SentReport.ToD - firstDate;

            var correctionFactor = 2;
            var roundedMonthLength = 35;
            var maxStayDuration = duration.Days / numberOfStays / correctionFactor / roundedMonthLength;

            float distanceBetweenStaysInMonth = (float)duration.Days / (float)(numberOfStays * maxStayDuration * roundedMonthLength);

            foreach (var person in SentReport.Persons)
            {
                for (var i = 0; i < numberOfStays - 1; i++)
                {
                    var stayDuration = random.Next(2, 4);

                    var fromDate = firstDate.AddMonths(i * (int)Math.Floor(distanceBetweenStaysInMonth));

                    var firstStay = this.CreateStay(fromDate, person.Id);
                    var firstReport = this.ExistingReports.First(x => x.FromD == firstStay.FromD);
                    var admission = this.CreateAdmission(firstStay);
                    firstReport.Admissions.Add(admission);
                    firstReport.Stays.Add(firstStay);

                    for (var j = 1; j < stayDuration - 1; j++)
                    {
                        var stay = this.CreateStay(fromDate.AddMonths(j), person.Id);
                        var report = this.ExistingReports.First(x => x.FromD == stay.FromD);
                        report.Stays.Add(stay);
                    }

                    var lastStay = this.CreateStay(fromDate.AddMonths(stayDuration - 1), person.Id);
                    var leaving = this.CreateLeaving(lastStay);

                    var lastReport = this.ExistingReports.First(x => x.FromD == lastStay.FromD);
                    lastReport.Stays.Add(lastStay);
                    lastReport.Leavings.Add(leaving);
                }

                var sentStay = this.CreateStay(this.SentReport.FromD, person.Id);
                var sentAdmission = this.CreateAdmission(sentStay);
                var sentLeaving = this.CreateLeaving(sentStay);

                this.SentReport.Admissions.Add(sentAdmission);
                this.SentReport.Stays.Add(sentStay);
                this.SentReport.Leavings.Add(sentLeaving);
            }
        }

        [Given(@"Im Meldungsbereich für eine Einrichtung wird jeder Klient (.*) x geändert")]
        public void GivenEveryClientIsChanged(int noOfChnges)
        {

        }

        private Stay CreateStay(DateTime from, string personId)
        {
            return new Stay
            {
                FromD = from,
                ToD = new DateTime(from.Year, from.Month, DateTime.DaysInMonth(from.Year, from.Month)),
                PersonId = personId
            };
        }

        private Admission CreateAdmission(Stay stay)
        {
            return StatLpDataGenerator.Instance.CreateAdmission(stay.PersonId, stay.From, false);
        }

        private Leaving CreateLeaving(Stay stay)
        {
            return StatLpDataGenerator.Instance.CreateLeaving(stay.PersonId, stay.FromD);
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
        public void GivenSentPropertyContainsItem(int reportNumber, string itemType, int personId, string date)
        {
            this.GivenPropertyContainsItem(-1, itemType, personId, date);
        }

        [Given(@"Existierende Meldung '(.*)' enthält eine '(.*)' von Person (.*) vom '(.*)'")]
        public void GivenExistingPropertyContainsItem(int reportNumber, string itemType, int personId, string date)
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

        [Given(@"Gesendete Meldung '(.*)': die Eigenschaft '(.*)' von '(.*)' ist auf '(.*)' gesetzt")]
        public void GivenSentMessageThePropertyIsSetTo(int reportNumber, string name, string type, string value)
        {
            this.GivenPropertyIsSetTo(-1, name, type, value);
        }

        [Given(@"Existierende Meldung '(.*)': die Eigenschaft '(.*)' von '(.*)' ist auf '(.*)' gesetzt")]
        public void GivenExistingMessageThePropertyIsSetTo(int reportNumber, string name, string type, string value)
        {
            this.GivenPropertyIsSetTo(reportNumber - 1, name, type, value);
        }

        [Given(@"Existierende Meldung (.*) gilt vom (.*) bis (.*) und ist eine Standard Meldung und enthält eine '(.*)' von Person (.*) vom (.*)")]
        public void GivenExistingMessageIsAStandardAdmissionMessage(int reportNumber, string validFrom, string validTo, string itemType, int personNumber, string admissionDate)
        {
            GivenMessageIsAStandardAdmissionMessage(reportNumber - 1, validFrom, validTo, itemType, personNumber, admissionDate);
        }

        [Given(@"Existierende Meldung (.*) gilt vom (.*) bis (.*) und ist eine Standard Meldung und enthält einen Aufenthalt")]
        public void GivenExistingMessageIsAStandardStayMessage(int reportNumber, string validFrom, string validTo)
        {
            GivenMessageIsAStandardStayMessage(reportNumber - 1, validFrom, validTo);
        }

        [Given(@"Gesendete Meldung: die Liste '(.*)' ist leer")]
        public void GivenTheListIsEmpty(string type)
        {
            GivenListIsIsEmpty(-1, type);
        }

        [Given(@"Existierende Meldung '(.*)': die Liste '(.*)' ist leer")]
        public void GivenTheListIsEmpty(int reportNumber, string type)
        {
            GivenListIsIsEmpty(reportNumber - 1, type);
        }

        [Then(@"dauert die Validierung aller Meldungen nicht länger als (.*) Sekunden")]
        public void ThenTheValidiationTakesMaxSeconds(int seconds)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var result = (StatLpReportValidationResult)SentReport.ValidateHistory(this.ExistingReports);
            stopWatch.Stop();

            Assert.True(seconds >= stopWatch.Elapsed.Seconds);
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

        private Person GetOrCreatePerson(StatLpReport report, int id)
        {
            var person = report.Persons.FirstOrDefault(x => x.Id == id.ToString()) ?? report.AddDummyPerson(id, false);

            return person;
        }

        private void GivenReportIsValidFromTo(int reportIndex, string dateFrom, string dateTo)
        {
            var from = DateTime.Parse(dateFrom);
            var to = DateTime.Parse(dateTo);

            var report = this.GetReportAndCreateNonExisting(reportIndex);

            report.FromD = from;
            report.ToD = to;

            var person = this.GetOrCreatePerson(report, 1);
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

        private void GivenPropertyContainsItem(int reportIndex, string itemType, int personId, string date)
        {
            var report = this.GetReportAndCreateNonExisting(reportIndex);

            var person = this.GetOrCreatePerson(report, personId);

            switch (itemType)
            {
                case nameof(Admission):
                    var admission = StatLpDataGenerator.Instance.CreateAdmission(personId.ToString(), DateTime.Parse(date).AsTimestamp());
                    report.Admissions.Add(admission);
                    break;

                case nameof(Leaving):

                    var leaving = StatLpDataGenerator.Instance.CreateLeaving(personId.ToString(), DateTime.Parse(date));
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

        private void GivenPropertyIsSetTo(int reportIndex, string name, string type, string value)
        {
            var report = this.GetReportAndCreateNonExisting(reportIndex);

            IMessage message;

            if (type == nameof(StatLpReport))
                message = report;
            else if (type == nameof(Institution))
                message = report.Institution;
            else if (type == nameof(Admission))
            { 
                if (!report.Admissions.Any())
                {
                    report.AddDummyAdmission(false);
                }
                message = report.Admissions[0];
            }
            else if (type == nameof(Leaving))
                message = report.Leavings[0];
            else if (type == nameof(Person))
            {
                if (!report.Persons.Any())
                {
                    report.AddDummyPerson(1, false);
                }
                message = report.Persons[0];
            }
            else if (type == nameof(Stay))
                message = report.Stays[0];
            else
                throw new NotImplementedException();

            if (!string.IsNullOrEmpty(value))
                message.SetValue(name, value);
            else
                message.SetDefault(name);
        }

        private void GivenMessageIsAStandardAdmissionMessage(int reportIndex, string validFrom, string validTo, string itemType, int personNumber, string admissionDate)
        {
            var report = this.GetReportAndCreateNonExisting(reportIndex);

            var from = DateTime.Parse(validFrom);
            var to = DateTime.Parse(validTo);

            report.FromD = from;
            report.ToD = to;

            var person = this.GetOrCreatePerson(report, personNumber);
            report.AddDummyPerson(Convert.ToInt32(personNumber), false);

            this.GivenPropertyContainsItem(reportIndex, itemType, personNumber, admissionDate);

            if (itemType == nameof(Admission))
            {
                this.GivenPropertyContainsStay(reportIndex, personNumber.ToString(), validFrom, validTo);
            }
        }

        private void GivenMessageIsAStandardStayMessage(int reportIndex, string validFrom, string validTo)
        {
            var report = this.GetReportAndCreateNonExisting(reportIndex);

            var from = DateTime.Parse(validFrom);
            var to = DateTime.Parse(validTo);

            report.FromD = from;
            report.ToD = to;

            report.AddDummyPerson(1, false);

            this.GivenPropertyContainsStay(reportIndex, report.Persons.First().Id, validFrom, validTo);
        }

        private void GivenListIsIsEmpty(int reportIndex, string type)
        {
            var report = this.GetReportAndCreateNonExisting(reportIndex);

            if (type == nameof(Person))
            {
                report.Persons.Clear();
            }
        }
    }
}
