using System;
using Vodamep.ReportBase;

namespace Vodamep.Cm.Model
{
    public partial class Activity : IPersonActivity
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsTimestamp(); }

        public string PersonId => this.CaseManagerId;
        public float Time => this.Minutes;
    }
}