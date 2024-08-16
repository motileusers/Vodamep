using Vodamep.Agp.Model;

namespace Vodamep.Summaries.Agp.Tests
{
    public class SummaryRegistryTests
    {
        private readonly SummaryRegistry _registry;
        private readonly AgpReport _report;

        public SummaryRegistryTests()
        {
            _registry = SummaryRegistry.CreateDefault();

            _report = CreateReport();
        }

        [Fact]
        public async Task CreateSummary_Summary_OneAgpReport_ReturnsSummary()
        {
            var summaryDescription = SummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report);

            Assert.NotNull(summary);
            Assert.NotEmpty(summary.Text);
        }

        public static AgpReport CreateReport()
        {
            var report = new AgpReport
            {
                FromD = DateTime.Today,
                ToD = DateTime.Today
            };

            report.Persons.Add(new Person { Id = "p1", FamilyName = "Person", GivenName = "Eins", CareAllowance = CareAllowance.L5, Referrer = Referrer.SelfReferrer });
            report.Persons.Add(new Person { Id = "p2", FamilyName = "Person", GivenName = "Zwei" });

            report.Persons[0].Diagnoses.Add(DiagnosisGroup.DementiaDisease);
            report.Persons[1].Diagnoses.Add(DiagnosisGroup.AffectiveDisorder);

            report.Activities.Add(new Activity { PersonId = "p1", Id = "m1", DateD = DateTime.Today, PlaceOfAction = PlaceOfAction.ResidencePlace, Minutes = 100 });

            report.Activities.Last().Entries.Add(ActivityType.ObservationsAssessmentAt);
            report.Activities.Last().Entries.Add(ActivityType.GeriatricPsychiatricAt);

            report.Activities.Add(new Activity { PersonId = "p1", Id = "m1", DateD = DateTime.Today, PlaceOfAction = PlaceOfAction.ResidencePlace, Minutes = 10 });
            report.Activities.Last().Entries.Add(ActivityType.ObservationsAssessmentAt);
            report.Activities.Last().Entries.Add(ActivityType.GeriatricPsychiatricAt);

            report.Activities.Add(new Activity { PersonId = "p2", Id = "m1", DateD = DateTime.Today, PlaceOfAction = PlaceOfAction.ResidencePlace, Minutes = 200 });
            report.Activities.Last().Entries.Add(ActivityType.GeriatricPsychiatricAt);

            report.StaffActivities.Add(new StaffActivity
            {
                Id = "m1",
                ActivityType = StaffActivityType.QualificationSa,
                DateD = DateTime.Today,
                Minutes = 100
            });

            return report;
        }

    }
}