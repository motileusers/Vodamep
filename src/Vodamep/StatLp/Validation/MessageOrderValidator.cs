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
                var sendMessage = x.StatLpReport;

                var futureMessages = x.StatLpReports.Where(y => y.FromD >= x.StatLpReport.FromD).OrderBy(y => y.FromD);
                var nextFutureMessage = futureMessages.FirstOrDefault();
                var historyMessages = x.StatLpReports.Where(y => y.FromD <= x.StatLpReport.FromD).OrderByDescending(y => y.FromD);
                var lastHistoryMessage = historyMessages.FirstOrDefault();

                if (lastHistoryMessage != null && nextFutureMessage == null)
                {
                    if (sendMessage.FromD.AddMonths(-1) > lastHistoryMessage.FromD)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.FromD),
                                    Validationmessages.StatLpReportPersonHistoryMissingReports(
                                        lastHistoryMessage.ToD.AddDays(1).ToShortDateString(),
                                        sendMessage.FromD.AddDays(-1).ToShortDateString())));
                    }

                }
            });
        }
    }
}