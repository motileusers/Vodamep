using FluentValidation;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using Vodamep.Data;
using Vodamep.Data.Dummy;
using Vodamep.Mkkp.Model;
using Vodamep.Mkkp.Validation;
using Xunit;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class MkkpValidationSteps
    {

        private MkkpReportValidationResult _result;
        private Activity _dummyActivities;

        public MkkpValidationSteps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new MkkpDisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            var date = DateTime.Today.AddMonths(-1);
            this.Report = MkkpDataGenerator.Instance.CreateMkkpReport(date.Year, date.Month, 1, 1, false);

            this.AddDummyActivities(Report.Persons[0].Id, Report.Staffs[0].Id);

            var consultation = new Activity() { Date = this.Report.From, StaffId = Report.Staffs[0].Id };
            consultation.Entries.Add(ActivityType.MedicalInjection);
            this.Report.Activities.Add(consultation);
        }

        public MkkpReport Report { get; private set; }

        public MkkpReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (MkkpReportValidationResult)Report.Validate();
                }

                return _result;
            }
        }

        [Given(@"eine Meldung ist korrekt befüllt")]
        public void GivenAValidReport()
        {
            // nichts zu tun
        }

        [Given(@"der Id einer Person ist nicht eindeutig")]
        public void GivenPersonIdNotUnique()
        {
            var p0 = this.Report.Persons[0];

            var p = this.Report.AddDummyPerson();

            p.Id = p0.Id;
            p.Id = p0.Id;
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist nicht gesetzt")]
        public void GivenThePropertyIsDefault(string name, string type)
        {
            if (type == nameof(MkkpReport))
                this.Report.SetDefault(name);
            else if (type == nameof(Person))
                this.Report.Persons[0].SetDefault(name);
            else if (type == nameof(Staff))
                this.Report.Staffs[0].SetDefault(name);
            else if (type == nameof(Activity))
                foreach (var a in this.Report.Activities)
                    a.SetDefault(name);
            else
                throw new NotImplementedException();
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist auf '(.*)' gesetzt")]
        public void GivenThePropertyIsSetTo(string name, string type, string value)
        {
            if (type == nameof(MkkpReport))
                this.Report.SetValue(name, value);
            else if (type == nameof(Person))
                this.Report.Persons[0].SetValue(name, value);
            else if (type == nameof(Staff))
                this.Report.Staffs[0].SetValue(name, value);
            else if (type == nameof(Activity))
                foreach (var a in this.Report.Activities)
                    a.SetValue(name, value);

            else
                throw new NotImplementedException();
        }

        [Given(@"der Id einer Mitarbeiterin ist nicht eindeutig")]
        public void GivenStaffIdNotUnique()
        {
            var s0 = this.Report.Staffs[0];

            var s = this.Report.AddDummyStaff();

            s.Id = s0.Id;
        }


        [Given(@"die Datums-Eigenschaft '(\w*)' von '(\w*)' hat eine Uhrzeit gesetzt")]
        public void GivenThePropertyHasATime(string name, string type)
        {
            IMessage m;
            if (type == nameof(MkkpReport))
                m = this.Report;
            else if (type == nameof(Person))
                m = this.Report.Persons[0];
            else if (type == nameof(Staff))
                m = this.Report.Staffs[0];
            else if (type == nameof(Activity))
                m = this.Report.Activities[0];
            else
                throw new NotImplementedException();

            var field = m.GetField(name);
            var ts = (field.Accessor.GetValue(m) as Timestamp) ?? this.Report.From;

            ts.Seconds = ts.Seconds + 60 * 60;
            field.Accessor.SetValue(m, ts);
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

        [Then(@"enthält das Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheResultContainsAnError(string message)
        {
            var pattern = new Regex(message, RegexOptions.IgnoreCase);

            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Error && pattern.IsMatch(x.ErrorMessage)));
        }

        [Then(@"die Fehlermeldung lautet: '(.*)'")]
        public void ThenTheResultContainsJust(string message)
        {
            Assert.Equal(message, this.Result.Errors.Select(x => x.ErrorMessage).Distinct().Single());
        }

        private void AddDummyActivities(string personId, string staffId)
        {

            _dummyActivities = new Activity() { Date = this.Report.From, PersonId = personId, StaffId = staffId };
            _dummyActivities.Entries.Add(new[] { ActivityType.Body, ActivityType.MedicalDiet, ActivityType.MedicalWound });

            this.Report.Activities.Add(_dummyActivities);
        }

        private void RemoveDummyActivities()
        {
            if (_dummyActivities != null)
            {
                this.Report.Activities.Remove(_dummyActivities);
                _dummyActivities = null;
            }
        }
    }
}
