using System;

namespace Vodamep.StatLp.Model
{
    public partial class Admission
    {
        public DateTime AdmissionDateD { get => this.AdmissionDate.AsDate(); set => this.AdmissionDate = value.AsTimestamp(); }
    }
}