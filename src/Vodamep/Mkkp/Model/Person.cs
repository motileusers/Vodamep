using System;

namespace Vodamep.Mkkp.Model
{
    public partial class Person
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }

    }
}