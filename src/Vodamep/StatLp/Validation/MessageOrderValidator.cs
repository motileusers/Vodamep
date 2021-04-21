using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class MessageOrderValidator : AbstractValidator<StatLpReportHistory>
    {
        public MessageOrderValidator()
        {
            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                var firstMessage = x.StatLpReport;

                foreach (var report in x.StatLpReports.OrderByDescending(y => y.FromD))
                {
                    var firstMessageDate = firstMessage.FromD.AddMonths(-1);

                    if (report.FromD == firstMessageDate)
                    {
                        firstMessage = report;
                    }
                    else
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                            Validationmessages.StatLpReportPersonHistoryMissingReports(
                                report.ToD.AddDays(1).ToShortDateString(),
                                firstMessage.FromD.AddDays(-1).ToShortDateString())));
                    }

                }
            });
        }
    }
}