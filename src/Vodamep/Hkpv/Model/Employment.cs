using System;
using System.Collections.Generic;
using System.Text;

namespace Vodamep.Hkpv.Model
{
    public partial class Employment
    {
        public Nullable<DateTime> FromD
        {
            get
            {
                if (this.From.AsDate() == DateTime.MinValue)
                    return null;
                else
                    return this.From.AsDate();
            }
            set
            {
                this.From = value.GetValueOrDefault().AsTimestamp();
            }
        }

        public Nullable<DateTime> ToD
        {
            get
            {
                if (this.To.AsDate() == DateTime.MinValue)
                    return null;
                else
                    return this.To.AsDate();
            }
            set
            {
                this.To = value.GetValueOrDefault().AsTimestamp();
            }
        }
    }
}
