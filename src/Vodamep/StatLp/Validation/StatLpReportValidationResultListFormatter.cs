using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{
    public class StatLpReportValidationResultListFormatter
    {

        private readonly ResultFormatterTemplate _template;
        private readonly bool _ignoreWarnings;

        public StatLpReportValidationResultListFormatter(ResultFormatterTemplate template, bool ignoreWarnings = false)
        {
            _template = template;
            _ignoreWarnings = ignoreWarnings;

            //_strategies = new[]
            //{
            //    new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Persons)), GetNameOfPerson),
            //    new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Staffs)), GetNameOfStaff),
            //    new GetNameByPatternStrategy(GetIdPattern(nameof(HkpvReport.Activities)), GetNameOfActivity),

            //    new GetNameByPatternStrategy($"^{nameof(HkpvReport.To)}$",(a,b) => string.Empty),
            //    new GetNameByPatternStrategy($"^{nameof(HkpvReport.ToD)}$",(a,b) => string.Empty),
            //    new GetNameByPatternStrategy($"^{nameof(HkpvReport.From)}$",(a,b) => string.Empty),
            //    new GetNameByPatternStrategy($"^{nameof(HkpvReport.FromD)}$",(a,b) => string.Empty),
            //};

        }

        public IEnumerable<string> Format(StatLpReport report, ValidationResult validationResult)
        {
            var result = new List<string>();

            var severities = validationResult.Errors
                .Where(x => !_ignoreWarnings || x.Severity == FluentValidation.Severity.Error)
                .OrderBy(x => x.Severity);

            //foreach (var severity in severities)
            //{
            //    string message = "";

            //    string info = this.GetInfo(report, severity.PropertyName);
            //    message += info;

            //    if (!String.IsNullOrWhiteSpace(info))
            //        message += " - ";

            //    message += severity.ErrorMessage;


            //    string value = "";
            //    if (severity.AttemptedValue?.GetType() == typeof(DateTime))
            //    {
            //        DateTime dateTime = (DateTime)severity.AttemptedValue;
            //        value += dateTime.ToShortDateString();
            //    }
            //    else
            //    {
            //        value = severity.AttemptedValue?.ToString();
            //    }

                
            //    if (!String.IsNullOrWhiteSpace(value))
            //    {
            //        message += " - ";
            //        message += value;
            //    }

            //    result.Add(message);
            //}

            return result;
        }

        private static string GetIdPattern(string propertyName) => $@"{propertyName}\[(?<id>\d+)\]";


        private readonly GetNameByPatternStrategy[] _strategies;

        private string GetNameOfPerson(StatLpReport report, int index)
        {
            //if (report.Persons.Count > index && index >= 0)
            //{
            //    var e = report.Persons[index];
            //    return $"Person: {e.FamilyName} {e.GivenName}";
            //}

            return string.Empty;
        }
        private string GetNameOfStaff(StatLpReport report, int index)
        {
            //if (report.Staffs.Count > index && index >= 0)
            //{
            //    var e = report.Staffs[index];
            //    return $"Mitarbeiter: {e.FamilyName} {e.GivenName}";
            //}

            return string.Empty;
        }

        private string GetNameOfActivity(StatLpReport report, int index)
        {
            //if (report.Activities.Count > index && index >= 0)
            //{
            //    var e = report.Activities[index];
            //    return $"Aktivität {e.DateD.ToString("dd.MM.yyyy")}{_template.Linefeed}  {String.Join(",", e.Entries)}{_template.Linefeed}  {GetNameOfPersonById(report, e.PersonId)}{_template.Linefeed}  {GetNameOfStaffById(report, e.StaffId)}";
            //}

            return string.Empty;
        }

        private string GetNameOfPersonById(StatLpReport report, string id)
        {
            //var e = report.Persons.Where(x => x.Id == id).FirstOrDefault();

            //if (e == null)
            //    return string.Empty;

            //return $"{e.FamilyName} {e.GivenName}";

            return string.Empty;
        }

        private string GetNameOfStaffById(StatLpReport report, string id)
        {
            //var e = report.Staffs.Where(x => x.Id == id).FirstOrDefault();

            //if (e == null)
            //    return string.Empty;

            //return $"{e.FamilyName} {e.GivenName}";

            return string.Empty;
        }

        private string GetInfo(StatLpReport report, string propertyName)
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
