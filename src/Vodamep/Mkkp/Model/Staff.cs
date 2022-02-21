using System;
using Vodamep.ReportBase;

namespace Vodamep.Mkkp.Model
{
    public partial class Staff : IStaff
    {
        public string GetDisplayName()
        {
            return PersonNameBuilder.FullNameOrId(this.GivenName, this.FamilyName, this.Id);
        }
    }
}