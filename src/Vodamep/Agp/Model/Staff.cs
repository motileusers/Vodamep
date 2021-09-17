using System;
using Vodamep.ReportBase;

namespace Vodamep.Agp.Model
{
    public partial class Staff : IStaff
    {
        public string GetDisplayName()
        {
            return $"{this.GivenName} {this.FamilyName}";
        }
    }
}