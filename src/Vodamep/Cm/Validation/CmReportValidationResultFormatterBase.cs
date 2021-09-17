using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.Cm.Model;

namespace Vodamep.Cm.Validation
{
    public class CmReportValidationResultFormatterBase
    {
        protected readonly ResultFormatterTemplate _template;
        protected readonly bool _ignoreWarnings;

        public CmReportValidationResultFormatterBase(ResultFormatterTemplate template, bool ignoreWarnings = false)
        {
            _template = template;
            _ignoreWarnings = ignoreWarnings;

            _strategies = new[]
            {
                new GetNameByPatternStrategy(GetIdPattern(nameof(CmReport.Persons)), GetNameOfPerson),
                new GetNameByPatternStrategy(GetIdPattern(nameof(CmReport.Activities)), GetNameOfActivity),
                new GetNameByPatternStrategy(GetIdPattern(nameof(CmReport.ClientActivities)), GetNameOfClientActivity),

                new GetNameByPatternStrategy($"^{nameof(CmReport.To)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(CmReport.ToD)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(CmReport.From)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(CmReport.FromD)}$",(a,b) => string.Empty),
            };

        }

        private static string GetIdPattern(string propertyName) => $@"{propertyName}\[(?<id>\d+)\]";

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

        private readonly GetNameByPatternStrategy[] _strategies;

        private string GetNameOfPerson(CmReport report, int index)
        {
            if (report.Persons.Count > index && index >= 0)
            {
                var e = report.Persons[index];
                return $"Person: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }
      
        private string GetNameOfActivity(CmReport report, int index)
        {
            if (report.Activities.Count > index && index >= 0)
            {
                var e = report.Activities[index];
                return $"Aktivität {e.DateD:dd.MM.yyyy}{_template.Linefeed}{_template.Linefeed} {_template.Linefeed}";
            }

            return string.Empty;
        }

        private string GetNameOfClientActivity(CmReport report, int index)
        {
            if (report.ClientActivities.Count > index && index >= 0)
            {
                var e = report.ClientActivities[index]; 
                return $"Klienten Aktivität {e.Date.AsDate():dd.MM.yyyy}{_template.Linefeed}  {GetNameOfPersonById(report, e.PersonId)}{_template.Linefeed}";
            }

            return string.Empty;
        }

        private string GetNameOfPersonById(CmReport report, string id)
        {
            var e = report.Persons.FirstOrDefault(x => x.Id == id);

            if (e == null)
                return string.Empty;

            return $"{e.FamilyName} {e.GivenName}";
        }

        protected string GetInfo(CmReport report, string propertyName)
        {
            foreach (var strategy in _strategies)
            {
                var (Success, Info) = strategy.GetInfo(report, propertyName);

                if (Success) return Info;
            }

            return propertyName;
        }

        private class GetNameByPatternStrategy
        {
            private readonly Regex _pattern;
            private readonly Func<CmReport, int, string> _resolveInfo;

            public GetNameByPatternStrategy(string pattern, Func<CmReport, int, string> resolveInfo)
            {
                _pattern = new Regex(pattern);
                _resolveInfo = resolveInfo;
            }

            public (bool Success, string Info) GetInfo(CmReport report, string propertyName)
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
