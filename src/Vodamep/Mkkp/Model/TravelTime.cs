using System;

namespace Vodamep.Mkkp.Model
{
    public partial class TravelTime
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsTimestamp(); }

    }
}