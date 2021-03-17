using System;
using Vodamep.ReportBase;

namespace Vodamep.StatLp.Model
{
    public partial class Person : IItem
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }

    }
}