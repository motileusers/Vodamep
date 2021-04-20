using System.Collections.Generic;

namespace Vodamep.StatLp.Model
{
    public class StatLpReportHistory
    {
        public StatLpReport StatLpReport { get; set; }

        public IEnumerable<StatLpReport> StatLpReports { get; set; }
    }
}