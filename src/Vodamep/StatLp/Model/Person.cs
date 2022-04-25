using System;
using Vodamep.ReportBase;

namespace Vodamep.StatLp.Model
{
    public partial class Person : IPerson
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }
        public string GetDisplayName()
        {
            return PersonNameBuilder.FullNameOrId(this.GivenName, this.FamilyName, this.Id);
        }

        internal static string ConcatNameAndBirthday(Person p) => $"{p.FamilyName}|{p.GivenName}|{p.BirthdayD:yyyyMMdd}";
    }
};