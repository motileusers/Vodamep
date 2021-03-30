using System;
using Vodamep.ReportBase;

namespace Vodamep.Cm.Model
{
    public partial class Person : IPerson
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }

    }
}