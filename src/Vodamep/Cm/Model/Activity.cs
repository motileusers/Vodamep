using System;
using Vodamep.ReportBase;

namespace Vodamep.Cm.Model
{
    public partial class Activity
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsTimestamp(); }

        public float Time => this.Minutes;
    }
}