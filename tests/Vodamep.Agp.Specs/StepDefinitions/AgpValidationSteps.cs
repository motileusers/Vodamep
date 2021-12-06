using FluentValidation;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Vodamep.Agp.Model;
using Vodamep.Agp.Validation;
using Vodamep.Data.Dummy;
using Enum = System.Enum;

namespace Vodamep.Specs.Agp.StepDefinitions
{

    [Binding]
    public class AgpValidationSteps 
    {
        private readonly ReportContext _context;

        public AgpValidationSteps(ReportContext context)            
        {
            _context = context;
            
            _context.GetPropertiesByType = this.GetPropertiesByType;

            var loc = new AgpDisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            var date = new DateTime(2021, 05, 01);
            _context.Report = AgpDataGenerator.Instance.CreateAgpReport("", date.Year, date.Month, 1, 1, false, false);
            this.AddDummyActivity(this.Report.Persons[0].Id, this.Report.Staffs[0].Id);
            this.AddDummyStaffActivity(this.Report.Staffs[0].Id);

            _context.Validator = new AgpReportValidator();
        }

        private AgpReport Report => _context.Report as AgpReport;

        private IEnumerable<IMessage> GetPropertiesByType(string type)
        {
            return type switch
            {
                nameof(Person) => new[] { this.Report.Persons[0] },
                nameof(Staff) => new[] { this.Report.Staffs[0] },
                nameof(StaffActivity) => this.Report.StaffActivities,
                nameof(Activity) => this.Report.Activities,
                _ => Array.Empty<IMessage>(),
            };
        }

        [Given(@"es ist ein 'AgpReport'")]
        public void GivenItIsAAgpReport()
        {

        }

        [Given(@"der Id einer AGP-Person ist nicht eindeutig")]
        public void GivenPersonIdNotUnique()
        {
            var p0 = this.Report.Persons[0];

            var p = this.Report.AddDummyPerson();

            p.Id = p0.Id;
            p.Id = p0.Id;
        }

        [Given(@"der Id einer AGP-Mitarbeiterin ist nicht eindeutig")]
        public void GivenStaffIdNotUnique()
        {
            var s0 = this.Report.Staffs[0];

            var s = this.Report.AddDummyStaff();

            s.Id = s0.Id;
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

        [Given(@"es werden zusätzliche Reisezeiten für einen AGP-Mitarbeiter eingetragen")]
        public void GivenTravelTimesAreAdded()
        {            
            var existingTravelTime = this.Report.StaffActivities.First();
            this.Report.StaffActivities.Add(new StaffActivity
            {
                Id = existingTravelTime.Id,
                Date = existingTravelTime.Date,
                DateD = existingTravelTime.DateD,
                Minutes = 125,
                StaffId = existingTravelTime.StaffId,
                ActivityType = StaffActivityType.TravelingSa
            });
        }

        [Given(@"es werden zusätzliche Leistungen pro AGP-Klient an einem Tag eingetragen")]
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
                Entries = { ActivityType.GeriatricPsychiatricAt }
            });
        }

        [Given(@"die Leistungstypen '(.*)' sind für eine AGP-Aktivität gesetzt")]
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

        [Given(@"zu einer AGP-Person sind keine AGP-Aktivitäten dokumentiert")]
        public void GivenAPersonWithoutActivities()
        {
            this.Report.Activities[0].PersonId = this.Report.Persons.First().Id + "id";
        }

        [Given(@"zu einer AGP-Mitarbeiterin sind keine AGP-Aktivitäten dokumentiert")]
        public void GivenAStaffMemberWithoutActivities()
        {
            this.Report.Activities[0].StaffId = this.Report.Staffs.First().Id + "id";
        }

        private void AddDummyActivity(string personId, string staffId)
        {
            var random = new Random();

            var placeOfAction = ((PlaceOfAction[])(Enum.GetValues(typeof(PlaceOfAction))))
                .Where(x => x != PlaceOfAction.UndefinedPlace)
                .ElementAt(random.Next(Enum.GetValues(typeof(PlaceOfAction)).Length - 1));

            var minutes = random.Next(1, 100) * 5;

            var activity = new Activity() { Id = "1", Date = this.Report.From, PersonId = personId, StaffId = staffId, Minutes = minutes, PlaceOfAction = placeOfAction };
            activity.Entries.Add(new[] { ActivityType.ClearingAt, ActivityType.ContactPartnerAt, ActivityType.GuidanceClientAt });

            this.Report.Activities.Add(activity);
        }


        private void AddDummyStaffActivity(string staffId)
        {
            int minutes = 45;

            var activity = new StaffActivity() { Id = "1", Date = this.Report.From, StaffId = staffId, Minutes = minutes, ActivityType = StaffActivityType.NetworkingSa };

            this.Report.StaffActivities.Add(activity);
        }

    }
}
