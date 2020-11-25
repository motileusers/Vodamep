using System;
using System.Collections.Generic;

namespace Vodamep.Hkpv
{
    public class HkpReportDiffResult
    {
        public Status Status { get; set; }

        public Type Type { get; set; }

        public string PropertyName { get; set; }

        public object Value1 { get; set; }

        public object Value2 { get; set; }

        public IList<HkpReportDiffResult> Children { get; set; } = new List<HkpReportDiffResult>();
    }
}