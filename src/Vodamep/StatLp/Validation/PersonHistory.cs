using System.Collections.Generic;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistory
    {
        public string PersonId { get; set; }

        public List<Admission> Admissions { get; private set; } = new List<Admission>();

        public List<Stay> Stays { get; private set; } = new List<Stay>();

        public List<Leaving> Leavings { get; private set; } = new List<Leaving>();
    }
}