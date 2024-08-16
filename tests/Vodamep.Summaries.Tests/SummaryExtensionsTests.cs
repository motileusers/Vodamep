using Vodamep.Mkkp.Model;
using Vodamep.Summaries.Mkkp;
using Vodamep.Summaries.Mkkp.Tests;

namespace Vodamep.Summaries.Tests
{
    public class SummaryExtensionsTests
    {
        private readonly SummaryRegistry _registry;
        private readonly MkkpReport _report;

        public SummaryExtensionsTests()
        {
            _registry = SummaryRegistry.CreateDefault();

            _report = 
                SummaryRegistryTests.CreateReport();
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
    }
}