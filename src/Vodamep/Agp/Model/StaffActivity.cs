using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.ReportBase;

namespace Vodamep.Agp.Model
{
    public partial class StaffActivity : IItem
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsTimestamp(); }

        public string PersonId => this.StaffId;
    }
}