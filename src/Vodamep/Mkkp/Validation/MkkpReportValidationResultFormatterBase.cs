using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.Mkkp.Model;

namespace Vodamep.Mkkp.Validation
{
    public abstract class MkkpReportValidationResultFormatterBase 
    {
        protected readonly ResultFormatterTemplate _template;
        protected readonly bool _ignoreWarnings;

        protected MkkpReportValidationResultFormatterBase(ResultFormatterTemplate template, bool ignoreWarnings = false)
        {
            _template = template;
            _ignoreWarnings = ignoreWarnings;

            _strategies = new[]
            {
                new GetNameByPatternStrategy(GetIdPattern(nameof(MkkpReport.Persons)), GetNameOfPerson),
                new GetNameByPatternStrategy(GetIdPattern(nameof(MkkpReport.Staffs)), GetNameOfStaff),
                new GetNameByPatternStrategy(GetIdPattern(nameof(MkkpReport.Activities)), GetNameOfActivity),

                new GetNameByPatternStrategy($"^{nameof(MkkpReport.To)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(MkkpReport.ToD)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(MkkpReport.From)}$",(a,b) => string.Empty),
                new GetNameByPatternStrategy($"^{nameof(MkkpReport.FromD)}$",(a,b) => string.Empty),
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

        protected string GetInfo(MkkpReport report, string propertyName)
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

        private string GetNameOfPerson(MkkpReport report, int index)
        {
            if (report.Persons.Count > index && index >= 0)
            {
                var e = report.Persons[index];
                return $"Person: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }
        private string GetNameOfStaff(MkkpReport report, int index)
        {
            if (report.Staffs.Count > index && index >= 0)
            {
                var e = report.Staffs[index];
                return $"Mitarbeiter: {e.FamilyName} {e.GivenName}";
            }

            return string.Empty;
        }

        private string GetNameOfActivity(MkkpReport report, int index)
        {
            if (report.Activities.Count > index && index >= 0)
            {
                var e = report.Activities[index];
                return $"Aktivität {e.DateD.ToString("dd.MM.yyyy")}{_template.Linefeed}  {String.Join(",", e.Entries)}{_template.Linefeed}  {GetNameOfPersonById(report, e.PersonId)}{_template.Linefeed}  {GetNameOfStaffById(report, e.StaffId)}";
            }

            return string.Empty;
        }

        private string GetNameOfPersonById(MkkpReport report, string id)
        {
            var e = report.Persons.Where(x => x.Id == id).FirstOrDefault();

            if (e == null)
                return string.Empty;

            return $"{e.FamilyName} {e.GivenName}";
        }

        private string GetNameOfStaffById(MkkpReport report, string id)
        {
            var e = report.Staffs.Where(x => x.Id == id).FirstOrDefault();

            if (e == null)
                return string.Empty;

            return $"{e.FamilyName} {e.GivenName}";
        }

        private class GetNameByPatternStrategy
        {
            private readonly Regex _pattern;
            private readonly Func<MkkpReport, int, string> _resolveInfo;

            public GetNameByPatternStrategy(string pattern, Func<MkkpReport, int, string> resolveInfo)
            {
                _pattern = new Regex(pattern);
                _resolveInfo = resolveInfo;
            }

            public (bool Success, string Info) GetInfo(MkkpReport report, string propertyName)
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