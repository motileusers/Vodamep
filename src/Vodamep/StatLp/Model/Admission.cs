using System;

namespace Vodamep.StatLp.Model
{
    public partial class Admission
    {
        public DateTime ValidD { get => this.Valid.AsDate(); set => this.Valid = value.AsTimestamp(); }

        public DateTime PriorAdmissionD { get => this.PriorAdmission.AsDate(); set => this.PriorAdmission = value.AsTimestamp(); }
    }
}