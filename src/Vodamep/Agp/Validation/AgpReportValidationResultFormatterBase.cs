using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.Agp.Model;

namespace Vodamep.Agp.Validation
{
    public abstract class AgpReportValidationResultFormatterBase
    {
        protected readonly ResultFormatterTemplate _template;
        protected readonly bool _ignoreWarnings;

        protected AgpReportValidationResultFormatterBase(ResultFormatterTemplate template, bool ignoreWarnings = false)
        {
            _template = template;
            _ignoreWarnings = ignoreWarnings;

            _strategies = new[]
            {
                new GetNameByPatternStrategy(GetIdPattern(nameof(AgpReport.Persons)), GetNameOfPerson),
                new GetNameByPatternStrategy(GetIdPattern(nameof(AgpReport.Activities)), GetNameOfActivity),

                new GetNameByPatternStrategy($"^{nameof(AgpReport.To)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(AgpReport.ToD)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(AgpReport.From)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(AgpReport.FromD)}$",(a,b) => string.Empty),
            };

        }

        protected string GetSeverityName(FluentValidation.Severity severity)
        {
            switch (severity)
            {
                case FluentValidation.Severity.Error:
                    return "Fehler";
                case FluentValidation.Severity.Warning:
                    return "Warnung";
                case FluentValidation.Severity.Info:
                    return "Information";
                default:
                    return severity.ToString();
            }
        }

        protected string GetInfo(AgpReport report, string propertyName)
        {
            foreach (var strategy in _strategies)
            {
                var (Success, Info) = strategy.GetInfo(report, propertyName);

                if (Success) return Info;
            }

            return propertyName;
        }

        private static string GetIdPattern(string propertyName) => $@"{propertyName}\[(?<id>\d+)\]";

        private readonly GetNameByPatternStrategy[] _strategies;

        private string GetNameOfPerson(AgpReport report, int index)
        {
            if (report.Persons.Count > index && index >= 0)
            {
                var e = report.Persons[index];
                //return $"Person: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }

        private string GetNameOfActivity(AgpReport report, int index)
        {
            if (report.Activities.Count > index && index >= 0)
            {
                var e = report.Activities[index];
                return $"Aktivität {e.DateD.ToString("dd.MM.yyyy")}{_template.Linefeed}  {String.Join(",", e.Entries)}{_template.Linefeed}  {GetNameOfPersonById(report, e.PersonId)}{_template.Linefeed}";
            }

            return string.Empty;
        }

        private string GetNameOfPersonById(AgpReport report, string id)
        {
            var e = report.Persons.Where(x => x.Id == id).FirstOrDefault();

            if (e == null)
                return string.Empty;

            //return $"{e.FamilyName} {e.GivenName}";
            return "";
        }


        private class GetNameByPatternStrategy
        {
            private readonly Regex _pattern;
            private readonly Func<AgpReport, int, string> _resolveInfo;

            public GetNameByPatternStrategy(string pattern, Func<AgpReport, int, string> resolveInfo)
            {
                _pattern = new Regex(pattern);
                _resolveInfo = resolveInfo;
            }

            public (bool Success, string Info) GetInfo(AgpReport report, string propertyName)
            {
                var m = this._pattern.Match(propertyName);

                if (m.Success)
                {
                    int.TryParse(m.Groups["id"].Value, out int id);

                    return (true, _resolveInfo(report, id));
                }
                return (false, string.Empty);
            }


        }
    }
}