using FluentValidation;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using Vodamep.Data.Dummy;
using Vodamep.Mkkp.Model;
using Vodamep.Mkkp.Validation;
using Xunit;
using Enum = System.Enum;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class MkkpValidationSteps
    {

        private MkkpReportValidationResult _result;
        private Activity _dummyActivity;

        public MkkpValidationSteps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new MkkpDisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            var date = DateTime.Today.AddMonths(-1);
            this.Report = MkkpDataGenerator.Instance.CreateMkkpReport("", date.Year, date.Month, 1, 1, false, false);

            this.AddDummyActivity(Report.Persons[0].Id, Report.Staffs[0].Id);
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
            else if (type == nameof(TravelTime))
                this.Report.TravelTimes[0].SetValue(name, value);
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

            var s = this.Report.AddDummyStaff(true);

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
            var ts = (field.Accessor.GetValue(m) as Timestamp);

            if (ts == null) throw new Exception();

            ts.Seconds = ts.Seconds + 60 * 60;
            field.Accessor.SetValue(m, ts);
        }

        [Given(@"die Diagnose\(n\) ist auf '(.*)' gesetzt")]
        public void GivenTheDiagnosisGroupIsSetTo(string value)
        {
            this.Report.Persons[0].Diagnoses.Clear();

            if (value.Contains(','))
            {
                var diagnosis = value.Split(',').Select(x => (DiagnosisGroup)Enum.Parse(typeof(DiagnosisGroup), x));
                this.Report.Persons[0].Diagnoses.AddRange(diagnosis);
            }
            else if (Enum.TryParse(value, out DiagnosisGroup diagnosis))
            {
                this.Report.Persons[0].Diagnoses.Add(diagnosis);
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

        [Given(@"es werden zusätzliche Reisezeiten für einen Mitarbeiter eingetragen")]
        public void GivenTravelTimesAreAdded()
        {
            var existingTravelTime = this.Report.TravelTimes.FirstOrDefault();

            this.Report.TravelTimes.Add(new TravelTime
            {
                Id = existingTravelTime.Id,
                Date = existingTravelTime.Date,
                DateD = existingTravelTime.DateD,
                Minutes = 125,
                StaffId = existingTravelTime.StaffId
            });
        }

        [Given(@"es werden zusätzliche Leistungen pro Klient an einem Tag eingetragen")]
        public void GivenAdditonalActivitiesPerClientAndDay()
        {
            var existingActivity = this.Report.Activities.First();
            existingActivity.Minutes = 250;

            this.Report.Activities.Add(new Activity()
            {
                Id = existingActivity.Id,
                Date = existingActivity.Date,
                DateD = existingActivity.DateD,
                Minutes = 125,
                StaffId = existingActivity.StaffId,
                PersonId = existingActivity.PersonId,
                PlaceOfAction = PlaceOfAction.ResidencePlace,
                Entries = { ActivityType.MedicalDiet}
            });
        }

        [Given(@"die Leistungstypen '(.*)' sind für eine Aktivität gesetzt")]
        public void GivenTheActivitiyTypesAreSetTo(string value)
        {
            this.Report.Activities[0].Entries.Clear();

            if (value.Contains(','))
            {
                var activityTypes = value.Split(',').Select(x => (ActivityType)Enum.Parse(typeof(ActivityType), x));
                this.Report.Activities[0].Entries.AddRange(activityTypes);
            }
            else if (Enum.TryParse(value, out ActivityType activityType))
            {
                this.Report.Activities[0].Entries.Add(activityType);
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

        [Given(@"zu einer Person sind keine Aktivitäten dokumentiert")]
        public void GivenAPersonWithoutActivities()
        {
            this.Report.Activities[0].PersonId = this.Report.Persons.First().Id + "id";
        }

        [Given(@"zu einer Mitarbeiterin sind keine Aktivitäten dokumentiert")]
        public void GivenAStaffMemberWithoutActivities()
        {
            this.Report.Activities[0].StaffId = this.Report.Staffs.First().Id + "id";
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

        [Then(@"enthält das escapte Validierungsergebnis den Fehler '(.*)'")]
        public void ThenTheResultContainsAnErrorRegex(string message)
        {
            var pattern = new Regex(Regex.Escape(message), RegexOptions.IgnoreCase);
            
            Assert.NotEmpty(this.Result.Errors.Where(x => x.Severity == Severity.Error && pattern.IsMatch(x.ErrorMessage)));
        }

        [Then(@"die Fehlermeldung lautet: '(.*)'")]
        public void ThenTheResultContainsJust(string message)
        {
            Assert.Equal(message, this.Result.Errors.Select(x => x.ErrorMessage).Distinct().Single());
        }

        private void AddDummyActivity(string personId, string staffId)
        {
            var random = new Random();

            var placeOfAction = ((PlaceOfAction[]) (Enum.GetValues(typeof(PlaceOfAction))))
                .Where(x => x != PlaceOfAction.UndefinedPlace)
                .ElementAt(random.Next(Enum.GetValues(typeof(PlaceOfAction)).Length - 1));

            var minutes = random.Next(1,100) * 5;
            _dummyActivity = new Activity() { Id = "1", Date = this.Report.From, PersonId = personId, StaffId = staffId, Minutes = minutes, PlaceOfAction = placeOfAction};
            _dummyActivity.Entries.Add(new[] { ActivityType.Body, ActivityType.MedicalDiet, ActivityType.MedicalWound });

            this.Report.Activities.Add(_dummyActivity);
        }

        private void RemoveDummyActivities()
        {
            if (_dummyActivity != null)
            {
                this.Report.Activities.Remove(_dummyActivity);
                _dummyActivity = null;
            }
        }
    }
}
