using System;

namespace Vodamep.Cm.Model
{
    public partial class Activity
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsTimestamp(); }

    }
}