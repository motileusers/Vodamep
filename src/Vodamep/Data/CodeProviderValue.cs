using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.Data
{
    public class CodeProviderValue
    {
        public Type ReportType { get; set; }

        public Type EnumType { get; set; }

        public string EnumValue { get; set; }

        public string ProtoValue { get; set; }

        public string Text { get; set; }
    }
}
