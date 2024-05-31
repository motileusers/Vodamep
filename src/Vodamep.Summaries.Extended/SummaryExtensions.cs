using Markdig;

namespace Vodamep.Summaries
{
    public static class SummaryExtensions
    {
        public static string ToHtml(this Summary summary)
        {
            return Markdown.ToHtml(summary.Text);
        }

        public static string Diff(this Summary oldSummary, Summary newSummary) { 
        
            var oldHtml = oldSummary.ToHtml();
            var newHtml = newSummary.ToHtml();


            var diffHelper = new HtmlDiff.HtmlDiff(oldHtml, newHtml);
            string diffOutput = diffHelper.Build();

            return diffOutput;

        }

    }
}
