using System.Collections.Generic;
using Vodamep.StatLp.Validation;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Model
{
    public class StatLpReportHistory
    {
        public StatLpReport StatLpReport { get; set; }

        public IEnumerable<StatLpReport> StatLpReports { get; set; }

        public ClearingExceptions ClearingExceptions { get; set; }

    }
}