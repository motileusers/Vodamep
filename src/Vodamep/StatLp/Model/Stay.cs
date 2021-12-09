using System;

namespace Vodamep.StatLp.Model
{
    public partial class Stay
    {
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime? ToD { 
            get => this.To != null ? this.To.AsDate() :(DateTime ?) null; 
            set => this.To = value.HasValue? value.Value.AsTimestamp() : null; 
        }

    }
}