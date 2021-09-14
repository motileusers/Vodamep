using System.Collections.Generic;
using Vodamep.StatLp.Validation;

namespace Vodamep.StatLp.Model
{
    public class StatLpReportHistory
    {
        public StatLpReport StatLpReport { get; set; }

        public IEnumerable<StatLpReport> StatLpReports { get; set; }

        public List<IdMapping> ExistingIdMappings { get; set; }
    }
}