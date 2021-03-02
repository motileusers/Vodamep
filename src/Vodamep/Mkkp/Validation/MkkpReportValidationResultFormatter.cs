using System.Linq;
using System.Text;
using FluentValidation.Results;
using Vodamep.Mkkp.Model;

namespace Vodamep.Mkkp.Validation
{
    public class MkkpReportValidationResultFormatter : MkkpReportValidationResultFormatterBase
    {
        
        public MkkpReportValidationResultFormatter(ResultFormatterTemplate template, bool ignoreWarnings = false) : base(template, ignoreWarnings)
        {
        }

        public string Format(MkkpReport report, ValidationResult validationResult)
        {

            var result = new StringBuilder();

            result.Append(_template.Header(report, validationResult));

            var severities = validationResult.Errors
                .Where(x => !_ignoreWarnings || x.Severity == FluentValidation.Severity.Error)
                .OrderBy(x => x.Severity)
                .GroupBy(x => x.Severity);

            foreach (var severity in severities)
            {
                result.Append(_template.HeaderSeverity(GetSeverityName(severity.Key)));

                var entries = severity.Select(x => new
                {
                    Info = this.GetInfo(report, x.PropertyName),
                    Message = x.ErrorMessage,
                    Value = x.AttemptedValue?.ToString()
                }).ToArray();

                foreach (var groupedInfos in entries.OrderBy(x => x.Info).GroupBy(x => x.Info))
                {
                    result.Append(_template.FirstLine((groupedInfos.Key, groupedInfos.First().Message, groupedInfos.First().Value)));

                    foreach (var info in groupedInfos.Skip(1))
                    {
                        result.Append(_template.Line((info.Message, info.Value)));
                    }
                }

                result.Append(_template.FooterSeverity(severity.ToString()));
            }
            return result.ToString();
        }


    }
}
