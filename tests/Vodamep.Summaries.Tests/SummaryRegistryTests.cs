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

            _report = CreateReport();
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

        [Fact]
        public async Task ToHtml_IncludeStyles_ContainsStyles()
        {
            var summaryDescription = SummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report);

            var html = summary!.ToHtml(true);

            Assert.Contains("<style>", html);
        }

        [Fact]
        public async Task ToHtml_NotIncludeStyles_ContainsStyles()
        {
            var summaryDescription = SummaryFactory.GetDescription();

            var summary = await _registry.CreateSummary(summaryDescription, _report);

            var html = summary!.ToHtml(false);

            Assert.DoesNotContain("<style>", html);
        }

        [Fact]
        public async Task Diff_BirthdayIsAdded_BirthdayIsMarkedAsINS()
        {
            var summaryDescription = SummaryFactory.GetDescription();

            _report.Persons[0].Birthday = null;

            var summary = await _registry.CreateSummary(summaryDescription, _report);

            var r2 = _report.Clone();

            r2.Persons[0].BirthdayD = new DateTime(2024, 6, 3);

            var summary2 = await _registry.CreateSummary(summaryDescription, r2);

            var diff = summary!.Diff(summary2!, true);

            Assert.Contains("<ins class='diffins'>03.06.2024</ins>", diff);
        }


        public static MkkpReport CreateReport()
        {
            var report = new MkkpReport
            {
                FromD = DateTime.Today,
                ToD = DateTime.Today
            };

            report.Persons.Add(new Person { Id = "1", FamilyName = "Family", GivenName = "Given1" });
            report.Persons.Add(new Person { Id = "2", FamilyName = "Family", GivenName = "Given2" });

            report.Activities.Add(new Activity { PersonId = "1", DateD = DateTime.Today, ActivityScope = ActivityScope.PalliativeCareScope, Minutes = 100 });

            report.Activities.Add(new Activity { PersonId = "1", DateD = DateTime.Today, ActivityScope = ActivityScope.ChildCareScope, Minutes = 10 });

            report.Activities.Add(new Activity { PersonId = "2", DateD = DateTime.Today, ActivityScope = ActivityScope.ChildCareScope, Minutes = 200 });

            return report;
        }

    }
}