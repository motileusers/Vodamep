using System;
using Vodamep.ReportBase;

namespace Vodamep.Hkpv.Model
{
    public partial class Person : IPerson
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }
        public string GetDisplayName()
        {
            return PersonNameBuilder.FullNameOrId(this.GivenName, this.FamilyName, this.Id);
        }
    }
}