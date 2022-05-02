using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{
    public abstract class StatReportValidationResultFormatterBase
    {
        protected readonly ResultFormatterTemplate _template;
        protected readonly bool _ignoreWarnings;

        protected StatReportValidationResultFormatterBase(ResultFormatterTemplate template, bool ignoreWarnings = false)
        {
            _template = template;
            _ignoreWarnings = ignoreWarnings;

            _strategies = new[]
            {
                new GetNameByPatternStrategy(GetIdPattern(nameof(StatLpReport.Persons)), GetNameOfPerson),
                new GetNameByPatternStrategy(GetIdPattern(nameof(StatLpReport.Admissions)), GetNameOfAdmission),
                new GetNameByPatternStrategy(GetIdPattern(nameof(StatLpReport.Leavings)), GetNameOfLeavings),

                new GetNameByPatternStrategy($"^{nameof(StatLpReport.To)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(StatLpReport.ToD)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(StatLpReport.From)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(StatLpReport.FromD)}$",(a,b) => string.Empty),
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

        protected string GetInfo(StatLpReport report, string propertyName)
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

        private string GetNameOfPerson(StatLpReport report, int index)
        {
            if (report.Persons.Count > index && index >= 0)
            {
                var e = report.Persons[index];
                return $"Person: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }

        private string GetNameOfAdmission(StatLpReport report, int index)
        {
            if (report.Admissions.Count > index && index >= 0)
            {
                var e = report.Admissions[index];
                var p = report.Persons.Where(x => x.Id == e.PersonId).FirstOrDefault();
                return $"Aufnahmedaten von {p?.FamilyName} {p?.GivenName}, {e.AdmissionDateD:dd.MM.yyy}";
            }

            return string.Empty;
        }

        private string GetNameOfLeavings(StatLpReport report, int index)
        {
            if (report.Leavings.Count > index && index >= 0)
            {
                var l = report.Leavings[index];
                var p = report.Persons.Where(x => x.Id == l.PersonId).FirstOrDefault();
                return $"Entlassungsdaten von {p?.FamilyName} {p?.GivenName}, {l.LeavingDateD:dd.MM.yyy}";
            }

            return string.Empty;
        }


        private class GetNameByPatternStrategy
        {
            private readonly Regex _pattern;
            private readonly Func<StatLpReport, int, string> _resolveInfo;

            public GetNameByPatternStrategy(string pattern, Func<StatLpReport, int, string> resolveInfo)
            {
                _pattern = new Regex(pattern);
                _resolveInfo = resolveInfo;
            }

            public (bool Success, string Info) GetInfo(StatLpReport report, string propertyName)
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