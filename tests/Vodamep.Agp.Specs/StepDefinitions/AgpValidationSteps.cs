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
using Vodamep.Agp.Model;
using Vodamep.Agp.Validation;
using Xunit;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class AgpValidationSteps
    {

        private AgpReportValidationResult _result;
        private Activity _dummyActivities;

        // todo

        public AgpValidationSteps()
        {
        }

        public AgpReport Report { get; private set; }

        public AgpReportValidationResult Result
        {
            get
            {
                if (_result == null)
                {
                    _result = (AgpReportValidationResult)Report.Validate();
                }

                return _result;
            }
        }


        [Given(@"eine Meldung ist korrekt befüllt")]
        public void GivenAValidReport()
        {
            // nichts zu tun
        }

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist auf '(.*)' gesetzt")]
        public void GivenThePropertyIsSetTo(string name, string type, string value)
        {
            if (type == nameof(AgpReport))
                this.Report.SetValue(name, value);
            else if (type == nameof(Person))
                this.Report.Persons[0].SetValue(name, value);
            else if (type == nameof(Staff))
                this.Report.Staffs[0].SetValue(name, value);
            //else if (type == nameof(Employment))
            //    this.Report.Staffs[0].Employments[0].SetValue(name, value);
            else if (type == nameof(Activity))
                foreach (var a in this.Report.Activities)
                    a.SetValue(name, value);

            else
                throw new NotImplementedException();
        }

        [Given(@"der Id einer Person ist nicht eindeutig")]
        public void GivenPersonIdNotUnique()
        {
            var p0 = this.Report.Persons[0];

            var p = this.Report.AddDummyPerson();

            p.Id = p0.Id;
            p.Id = p0.Id;
        }

        [Given(@"der Id einer Mitarbeiterin ist nicht eindeutig")]
        public void GivenStaffIdNotUnique()
        {
            var s0 = this.Report.Staffs[0];

            var s = this.Report.AddDummyStaff();

            s.Id = s0.Id;
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

    }
}
