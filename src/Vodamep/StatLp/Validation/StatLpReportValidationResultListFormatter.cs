using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{

    
    public class StatLpReportValidationResultListFormatter : StatReportValidationResultFormatterBase
    {

    

        public StatLpReportValidationResultListFormatter(ResultFormatterTemplate template, bool ignoreWarnings = false)
        : base(template, ignoreWarnings)
        {
        }

        public IEnumerable<string> Format(StatLpReport report, ValidationResult validationResult)
        {
            var result = new List<string>();

            var severities = validationResult.Errors
                .Where(x => !_ignoreWarnings || x.Severity == FluentValidation.Severity.Error)
                .OrderBy(x => x.Severity);

            foreach (var severity in severities)
            {
                string message = "";

                string info = this.GetInfo(report, severity.PropertyName);
                message += info;

                if (!String.IsNullOrWhiteSpace(info))
                    message += " - ";

                message += severity.ErrorMessage;


                string value = "";
                if (severity.AttemptedValue?.GetType() == typeof(DateTime))
                {
                    DateTime dateTime = (DateTime)severity.AttemptedValue;
                    value += dateTime.ToShortDateString();
                }
                else
                {
                    value = severity.AttemptedValue?.ToString();
                }


                if (!String.IsNullOrWhiteSpace(value))
                {
                    message += " - ";
                    message += value;
                }

                result.Add(message);
            }

            return result;
        }
    }
}
