using System.Collections.Generic;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistory
    {

        public string ClearingId { get; set; }

        public Dictionary<string, string> PersonIdDictionary { get; set; } = new Dictionary<string, string>();

        public List<Admission> Admissions { get; private set; } = new List<Admission>();

        public List<Stay> Stays { get; private set; } = new List<Stay>();

        public List<Leaving> Leavings { get; private set; } = new List<Leaving>();

        public List<StayInfo> StayInfos { get; private set; } = new List<StayInfo>();

        public List<Attribute> Attributes { get; private set; } = new List<Attribute>();

        public Person Person { get; set; }

        public bool IsFromSentReport { get; set; }



        public string GetPersonName()
        {
            return this.Person?.GivenName + " " + this.Person?.FamilyName;
        }


        public void AddPersonId(string sourceSystemId, string id)
        {
            if (!this.PersonIdDictionary.ContainsKey(sourceSystemId))
                this.PersonIdDictionary.Add(sourceSystemId, id);
            else
                this.PersonIdDictionary[sourceSystemId] = id;
        }
    }
}