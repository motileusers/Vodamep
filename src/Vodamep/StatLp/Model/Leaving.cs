using System;

namespace Vodamep.StatLp.Model
{
    public partial class Leaving
    {
        public DateTime LeavingDateD { get => this.LeavingDate.AsDate(); set => this.LeavingDate = value.AsTimestamp(); }
    }
}