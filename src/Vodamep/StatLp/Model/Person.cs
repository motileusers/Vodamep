using System;
using Vodamep.ReportBase;

namespace Vodamep.StatLp.Model
{
    public partial class Person : IPerson
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }
        public string GetDisplayName()
        {
            return (string.IsNullOrWhiteSpace(this.GivenName) || string.IsNullOrWhiteSpace(this.FamilyName)) ?
                this.Id : $"{this.GivenName} {this.FamilyName}";
        }
    }
}