﻿using FluentValidation;
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
    public class StatLpValidationSteps
    {

        private StatLpReportValidationResult _result;

        public StatLpValidationSteps()
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


        [Given(@"eine Meldung ist korrekt befüllt")]
        public void GivenAValidReport()
        {
            // nichts zu tun
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist nicht gesetzt")]
        public void GivenThePropertyIsDefault(string name, string type)
        {
            if (type == nameof(StatLpReport))
                this.Report.SetDefault(name);
            else if (type == nameof(Admission))
                this.Report.Admissions[0].SetDefault(name);
            else if (type == nameof(Leaving))
                this.Report.Leavings[0].SetDefault(name);
            else if (type == nameof(Person))
                this.Report.Persons[0].SetDefault(name);
            else
                throw new NotImplementedException();
        }

        [Given(@"die Liste von '(\w*)' ist leer")]
        public void GivenTheListPropertyIsEmpty(string type)
        {
            if (type == nameof(Admission))
                this.Report.Admissions.Clear();
            else if (type == nameof(Attribute))
                this.Report.Attributes.Clear();
            else if (type == nameof(Leaving))
                this.Report.Leavings.Clear();
            else if (type == nameof(Person))
            {
                //es müssen auch admissions und attributes gelöscht werden,
                //weil sonst die personen in den jeweiligen listen fehlen
                this.Report.Persons.Clear();

                this.Report.Admissions.Clear();
                this.Report.Attributes.Clear();
                this.Report.Stays.Clear();
            }
            else if (type == nameof(Stay))
                this.Report.Stays.Clear();
            else
                throw new NotImplementedException();
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist auf '(.*)' gesetzt")]
        public void GivenThePropertyIsSetTo(string name, string type, string value)
        {
            IMessage message;

            if (type == nameof(StatLpReport))
                message = this.Report;
            else if (type == nameof(Institution))
                message = this.Report.Institution;
            else if (type == nameof(Admission))
                message = this.Report.Admissions[0];
            else if (type == nameof(Attribute))
                message = this.Report.Attributes[0];
            else if (type == nameof(Leaving))
                message = this.Report.Leavings[0];
            else if (type == nameof(Person))
                message = this.Report.Persons[0];
            else if (type == nameof(Stay))
                message = this.Report.Stays[0];
            else
                throw new NotImplementedException();

            if (!string.IsNullOrEmpty(value))
                message.SetValue(name, value);
            else
                message.SetDefault(name);
        }

        [Given(@"die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '(\w*)' ist auf '(.*)' gesetzt")]
        public void GivenTheEnumLIstPropertyFromAdmissionIsSetTo(string type, string value)
        {
            //todo geht bestimmt eleeganter
            switch (type)
            {
                case "PersonalChanges":
                    this.SetPersonalChanges(value);
                    break;

                case "SocialChanges":
                    this.SetSocialChanges(value);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void SetPersonalChanges(string value)
        {
            this.Report.Admissions.First().PersonalChanges.Clear();

            if (value.Contains(','))
            {
                var personalChange = value.Split(',').Select(x => (PersonalChange)Enum.Parse(typeof(PersonalChange), x));
                this.Report.Admissions[0].PersonalChanges.AddRange(personalChange);
            }
            else if (Enum.TryParse(value, out PersonalChange activityType))
            {
                this.Report.Admissions[0].PersonalChanges.Add(activityType);
            }
            else if (value == "")
            {
                //nothing do do, already emptied yet
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void SetSocialChanges(string value)
        {
            this.Report.Admissions.First().SocialChanges.Clear();
            
            if (value.Contains(','))
            {
                var activityTypes = value.Split(',').Select(x => (SocialChange)Enum.Parse(typeof(SocialChange), x));
                this.Report.Admissions[0].SocialChanges.AddRange(activityTypes);
            }
            else if (Enum.TryParse(value, out SocialChange socialChange))
            {
                this.Report.Admissions[0].SocialChanges.Add(socialChange);
            }
            else if (value == "")
            {
                //nothing do do, already emptied yet
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [Given(@"die Datums-Eigenschaft '(\w*)' von '(\w*)' hat eine Uhrzeit gesetzt")]
        public void GivenThePropertyHasATime(string name, string type)
        {
            IMessage m;
            if (type == nameof(StatLpReport))
                m = this.Report;
            else if (type == nameof(Admission))
                m = this.Report.Admissions[0];
            else if (type == nameof(Attribute))
                m = this.Report.Attributes[0];
            else if (type == nameof(Person))
                m = this.Report.Persons[0];
            else if (type == nameof(Stay))
                m = this.Report.Stays[0];
            else
                throw new NotImplementedException();

            var field = m.GetField(name);
            var ts = (field.Accessor.GetValue(m) as Timestamp) ?? this.Report.From;

            ts.Seconds = ts.Seconds + 60 * 60;
            field.Accessor.SetValue(m, ts);
        }

        [Given(@"der Id einer Person ist nicht eindeutig")]
        public void GivenPersonIdNotUnique()
        {
            var p0 = this.Report.Persons[0];

            var p = this.Report.AddDummyPerson();

            p.Id = p0.Id;
            p.Id = p0.Id;
        }

        [Given(@"Bis ist vor Von bei einem Stay")]
        public void GivenToIsBeforeFromAtFirstStay()
        {
            var stay = this.Report.Stays[0];

            stay.From = stay.To.AsDate().AddDays(2).AsTimestamp();
        }

        [Then(@"*enthält (das Validierungsergebnis )?keine Fehler")]
        public void ThenTheResultContainsNoErrors(string dummy)
        {
            Assert.True(this.Result.IsValid);
            Assert.Empty(this.Result.Errors.Where(x => x.Severity == Severity.Error));
        }

        [Then(@"*enthält (das Validierungsergebnis )?keine Warnungen")]
        public void ThenTheResultContainsNoWarnings(string dummy)
        {
            Assert.Empty(this.Result.Errors.Where(x => x.Severity == Severity.Warning));
        }

        [Then(@"*enthält (das Validierungsergebnis )?genau einen Fehler")]
        public void ThenTheResultContainsOneError(object test)
        {
            Assert.False(this.Result.IsValid);
            Assert.Single(this.Result.Errors.Where(x => x.Severity == Severity.Error).Select(x => x.ErrorMessage).Distinct());
        }

        [Then(@"die Fehlermeldung lautet: '(.*)'")]
        public void ThenTheResultContainsJust(string message)
        {
            Assert.Equal(message, this.Result.Errors.Select(x => x.ErrorMessage).Distinct().Single());
        }

        [Then(@"enthält das Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheResultContainsAnError(string message)
        {
            var pattern = new Regex(message, RegexOptions.IgnoreCase);

            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Error && pattern.IsMatch(x.ErrorMessage)));
        }

        [Then(@"enthält das escapte Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheResultContainsAnErrorRegex(string message)
        {
            var pattern = new Regex(Regex.Escape(message), RegexOptions.IgnoreCase);

            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Error && pattern.IsMatch(x.ErrorMessage)));
        }

    }
}
