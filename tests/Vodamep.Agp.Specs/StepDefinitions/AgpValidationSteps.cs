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
using Vodamep.Agp.Model;
using Vodamep.Agp.Validation;
using Xunit;
using Enum = System.Enum;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class AgpValidationSteps
    {

        private AgpReportValidationResult _result;

        public AgpValidationSteps()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de");

            var loc = new AgpDisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            var date = DateTime.Today.AddMonths(-1);
            this.Report = AgpDataGenerator.Instance.CreateAgpReport(date.Year, date.Month, 1, 1, false);
            this.AddDummyActivity(this.Report.Persons[0].Id, this.Report.Staffs[0].Id);
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

        [Given(@"die Eigenschaft '(\w*)' von '(\w*)' ist nicht gesetzt")]
        public void GivenThePropertyIsDefault(string name, string type)
        {
            if (type == nameof(AgpReport))
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
            if (type == nameof(AgpReport))
                this.Report.SetValue(name, value);
            else if (type == nameof(Person))
                this.Report.Persons[0].SetValue(name, value);
            else if (type == nameof(Staff))
                this.Report.Staffs[0].SetValue(name, value);
            else if (type == nameof(TravelTime))
                foreach (var a in this.Report.TravelTimes)
                    a.SetValue(name, value);
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

        [Given(@"die Datums-Eigenschaft '(\w*)' von '(\w*)' hat eine Uhrzeit gesetzt")]
        public void GivenThePropertyHasATime(string name, string type)
        {
            IMessage m;
            if (type == nameof(AgpReport))
                m = this.Report;
            else if (type == nameof(Mkkp.Model.Person))
                m = this.Report.Persons[0];
            else if (type == nameof(Mkkp.Model.Staff))
                m = this.Report.Staffs[0];
            else if (type == nameof(Mkkp.Model.Activity))
                m = this.Report.Activities[0];
            else
                throw new NotImplementedException();

            var field = m.GetField(name);
            var ts = (field.Accessor.GetValue(m) as Timestamp) ?? this.Report.From;

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
            var existingTravelTime = this.Report.TravelTimes.First();
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
            existingActivity.Minutes = 125;

            this.Report.Activities.Add(new Activity()
            {
                Id = existingActivity.Id,
                Date = existingActivity.Date,
                DateD = existingActivity.DateD,
                Minutes = 125,
                StaffId = existingActivity.StaffId,
                PersonId = existingActivity.PersonId,
                PlaceOfAction = PlaceOfAction.BasePlace,
                Entries = { ActivityType.GeriatricPsychiatric }
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

        private void AddDummyActivity(string personId, string staffId)
        {
            var random = new Random();

            var placeOfAction = ((PlaceOfAction[]) (Enum.GetValues(typeof(PlaceOfAction))))
                .Where(x => x != PlaceOfAction.UndefinedPlace)
                .ElementAt(random.Next(Enum.GetValues(typeof(PlaceOfAction)).Length - 1));

            var minutes = random.Next(1,100) * 5;
          
            var activity = new Activity() { Date = this.Report.From, PersonId = personId, StaffId = staffId, Minutes = minutes, PlaceOfAction = placeOfAction };
            activity.Entries.Add(new[] { ActivityType.Clearing, ActivityType.ContactPartner, ActivityType.GuidancePartner });

            this.Report.Activities.Add(activity);
        }

    }
}
