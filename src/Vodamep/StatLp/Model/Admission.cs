using System;

namespace Vodamep.StatLp.Model
{
    public partial class Admission
    {
        public Nullable<DateTime> AdmissionDateD { get => this.AdmissionDate.AsNullableDate(); set => this.AdmissionDate = value?.AsTimestamp(); }
    }
}