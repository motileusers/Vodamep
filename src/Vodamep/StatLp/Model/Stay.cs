using System;

namespace Vodamep.StatLp.Model
{
    public partial class Stay
    {
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }

    }
}