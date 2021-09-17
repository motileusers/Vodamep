using System;

namespace Vodamep.StatLp.Model
{
    public partial class Attribute 
    {
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

    }
}