using System.Collections.Generic;
using System.Linq;

namespace Vodamep.Hkpv.Model
{
    public static class HkpvReportExtensions
    {
        public static HkpvReport AddPerson(this HkpvReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static HkpvReport AddPersons(this HkpvReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));
        
        public static HkpvReport AsSorted(this HkpvReport report)
        {
            var result = new HkpvReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            result.Activities.AddRange(report.Activities.AsSorted());

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));
            result.Staffs.AddRange(report.Staffs.OrderBy(x => x.Id));

            return result;
        }

    }
}