using System;

namespace Vodamep.Agp.Model
{
    public partial class TravelTime
    {
        public DateTime DateD { get => this.Date.AsDate(); set => this.Date = value.AsTimestamp(); }

    }
}