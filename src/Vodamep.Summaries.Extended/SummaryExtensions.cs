using Markdig;

namespace Vodamep.Summaries
{
    public static class SummaryExtensions
    {
        public static string ToHtml(this Summary summary, bool includeStyles = false)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            var content = Markdown.ToHtml(summary.Text, pipeline);

            return includeStyles ? WithTemplate(content) : content;
        }

        private static string WithTemplate(string htmlContent)
        {
            var template = GetTempate();

            return template.Replace("<!-- content -->", htmlContent);
        }

        private static string GetTempate()
        {
            var templateStream = typeof(SummaryExtensions).Assembly.GetManifestResourceStream("Vodamep.Summaries.Template.html");
            var stylesSteam = typeof(SummaryExtensions).Assembly.GetManifestResourceStream("Vodamep.Summaries.Styles.css");

            if (templateStream == null || stylesSteam == null)
            {
                return string.Empty;
            }

            using var templateReader = new StreamReader(templateStream);
            using var stylesReader = new StreamReader(stylesSteam);

            var template = templateReader.ReadToEnd();
            var style = stylesReader.ReadToEnd();

            return template.Replace("<!-- style -->", $"<style>{style}</style>");
        }

        public static string Diff(this Summary oldSummary, Summary newSummary, bool includeStyles = false)
        {
            var oldHtml = oldSummary.ToHtml();
            var newHtml = newSummary.ToHtml();

            var diff = new HtmlDiff.HtmlDiff(oldHtml, newHtml);

            string diffOutput = diff.Build();

            return includeStyles ? WithTemplate(diffOutput) : diffOutput;
        }
    }
}
