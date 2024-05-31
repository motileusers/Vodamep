using Vodamep.Mkkp.Model;
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

            _report = new MkkpReport
            {
                FromD = DateTime.Today,
                ToD = DateTime.Today
            };

            _report.Persons.Add(new Person { Id = "1", FamilyName = "Family", GivenName = "Given1" });
            _report.Persons.Add(new Person { Id = "2", FamilyName = "Family", GivenName = "Given2" });

            _report.Activities.Add(new Activity { PersonId = "1", DateD = DateTime.Today, ActivityScope = ActivityScope.PalliativeCareScope, Minutes = 100 });

            _report.Activities.Add(new Activity { PersonId = "1", DateD = DateTime.Today, ActivityScope = ActivityScope.ChildCareScope, Minutes = 10 });

            _report.Activities.Add(new Activity { PersonId = "2", DateD = DateTime.Today, ActivityScope = ActivityScope.ChildCareScope, Minutes = 200 });
        }

        [Fact]
        public void GetEntries_DefaultRegistry_ContainsDescription()
        {
            var r = SummaryRegistry.CreateDefault();

            var report = new MkkpReport
            {
                FromD = DateTime.Today,
                ToD = DateTime.Today
            };

            var e = r.GetEntries(report);

            Assert.Contains(MinutesPerActivityScopeSummaryFactory.GetDescription(), e);
        }


        [Fact]
        public async Task GetEntries_DefaultRegistry_CreateSummary()
        {
            var summaryDescription = MinutesPerActivityScopeSummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report);

            Assert.NotNull(summary);
            Assert.NotEmpty(summary.Text);

        }

        [Fact]
        public async Task GetEntries_DefaultRegistry_CreateSummary2()
        {
            var summaryDescription = SummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report);

            Assert.NotNull(summary);
            Assert.NotEmpty(summary.Text);

        }
    }
}