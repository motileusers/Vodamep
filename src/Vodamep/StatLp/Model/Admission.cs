using System;

namespace Vodamep.StatLp.Model
{
    public partial class Admission
    {
        public DateTime AdmissionDateD { get => this.AdmissionDate.AsDate(); set => this.AdmissionDate = value.AsTimestamp(); }

        public DateTime OriginAdmissionDateD { get => this.OriginAdmissionDate.AsDate(); set => this.OriginAdmissionDate = value.AsTimestamp(); }

    }
}