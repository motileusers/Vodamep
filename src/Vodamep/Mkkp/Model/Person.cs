using System;
using Vodamep.ReportBase;

namespace Vodamep.Mkkp.Model
{
    public partial class Person : INamedPerson
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }
        public string GetDisplayName()
        {
            return (string.IsNullOrWhiteSpace(this.GivenName) || string.IsNullOrWhiteSpace(this.FamilyName)) ? 
                this.Id : $"{this.GivenName} {this.FamilyName}";
        }
    }
}