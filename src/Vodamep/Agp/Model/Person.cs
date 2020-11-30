using System;

namespace Vodamep.Agp.Model
{
    public partial class Person
    {
        public DateTime BirthdayD { get => this.Birthday.AsDate(); set => this.Birthday = value.AsTimestamp(); }

    }
}