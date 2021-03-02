using System;
using System.Collections.Generic;


namespace Vodamep.ReportBase
{
    public class DiffResult
    {
        public Status Status { get; set; }

        public Type Type { get; set; }

        public string PropertyName { get; set; }

        public object Value1 { get; set; }

        public object Value2 { get; set; }

        public IList<DiffResult> Children { get; set; } = new List<DiffResult>();
    }
}