using Vodamep.Mkkp.Model;
using Vodamep.ReportBase;
using Vodamep.Summaries.Mkkp;

namespace Vodamep.Summaries.Tests
{
    public class SummaryRegistryTests
    {
        private readonly SummaryRegistry _registry;
        private readonly MkkpReport _report;

        public SummaryRegistryTests()
        {
            _registry = SummaryRegistry.CreateDefault();

            _report = CreateReport();
        }

        [Fact]
        public void GetEntries_DefaultRegistry_ContainsDescription()
        {
            var r = SummaryRegistry.CreateDefault();

            var e = r.GetEntries(ReportType.Mkkp);

            Assert.Contains(MinutesPerActivityScopeSummaryFactory.GetDescription(), e);
        }

        [Fact]
        public async Task CreateSummary_MinutesPerActivityScopeSummary_OneMkkpReport_ReturnsSummary()
        {
            var summaryDescription = MinutesPerActivityScopeSummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report);

            Assert.NotNull(summary);
            Assert.NotEmpty(summary.Text);
        }

        [Fact]
        public async Task CreateSummary_MinutesPerActivityScopeSummary_MultipleMkkpReport_ReturnsSummary()
        {
            var summaryDescription = MinutesPerActivityScopeSummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report, _report);

            Assert.NotNull(summary);
            Assert.NotEmpty(summary.Text);
        }

        [Fact]
        public async Task CreateSummary_MinutesPerDiagnosisSummaryFactory_MultipleMkkpReport_ReturnsSummary()
        {
            var summaryDescription = MinutesPerDiagnosisSummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report, _report);

            Assert.NotNull(summary);
            Assert.NotEmpty(summary.Text);
        }

        [Fact]
        public async Task CreateSummary_Summary_OneMkkpReport_ReturnsSummary()
        {
            var summaryDescription = SummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report);

            Assert.NotNull(summary);
            Assert.NotEmpty(summary.Text);
        }

        [Fact]
        public async Task CreateSummary_Summary_MultipleMkkpReport_ReturnsEmptySummary()
        {
            var summaryDescription = SummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report, _report);

            Assert.Null(summary);
        }

        public static MkkpReport CreateReport()
        {
            var report = new MkkpReport
            {
                FromD = DateTime.Today,
                ToD = DateTime.Today
            };

            report.Persons.Add(new Person { Id = "p1", FamilyName = "Person", GivenName = "Eins", CareAllowance = CareAllowance.L5, Referrer = Referrer.KhDornbirnReferrer, HospitalDoctor = "Dr. SehrSehrLanger Name" });
            report.Persons.Add(new Person { Id = "p2", FamilyName = "Person", GivenName = "Zwei" });

            report.Persons[0].Diagnoses.Add(DiagnosisGroup.HeartDisease);
            report.Persons[1].Diagnoses.Add(DiagnosisGroup.PalliativeCare3);

            report.Staffs.Add(new Staff { Id = "s1", FamilyName = "Mitarbeiterin", GivenName = "Eins" });

            report.Activities.Add(new Activity { PersonId = "p1", DateD = DateTime.Today, ActivityScope = ActivityScope.PalliativeCareScope, Minutes = 100 });

            report.Activities.Add(new Activity { PersonId = "p1", DateD = DateTime.Today, ActivityScope = ActivityScope.ChildCareScope, Minutes = 10 });

            report.Activities.Add(new Activity { PersonId = "p2", DateD = DateTime.Today, ActivityScope = ActivityScope.ChildCareScope, Minutes = 200 });

            report.TravelTimes.Add(new TravelTime { StaffId = "s1", DateD = DateTime.Today, Minutes = 10 });
            return report;
        }

    }
}