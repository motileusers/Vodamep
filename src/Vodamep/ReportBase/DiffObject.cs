using System;
using Vodamep.Hkpv;

namespace Vodamep.ReportBase
{
    public class DiffObject
    {
        public Section Section { get; set; }

        public DifferenceIdType DifferenceId { get; set; }

        public int Order { get; set; }

        public string DataDescription { get; set; }

        public string DataId { get; set; }

        public Type Type { get; set; }

        public Difference Difference { get; set; }

        public object Value1 { get; set; }

        public object Value2 { get; set; }
    }
}