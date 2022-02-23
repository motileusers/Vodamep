using System;
using Vodamep.ReportBase;

namespace Vodamep.Agp.Model
{
    public partial class Person : INationalityPerson
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }
        public string GetDisplayName()
        {
            string result = this.GivenName;
            if (!string.IsNullOrEmpty(this.GivenName) && !string.IsNullOrEmpty(this.FamilyName)) result += " ";
            result += this.FamilyName;

            if (string.IsNullOrEmpty(result)) result = this.Id;

            return result;
        }
    }
}