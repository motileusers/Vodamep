using System;
using Vodamep.ReportBase;

namespace Vodamep.Agp.Model
{
    public partial class Person : IItem
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }

    }
}