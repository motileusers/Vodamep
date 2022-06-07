using System.Collections.Generic;
using System.Linq;

namespace Vodamep.Agp.Model
{
    public static class AgpReportExtensions
    {
        public static AgpReport AddPerson(this AgpReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static AgpReport AddPersons(this AgpReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));

        public static AgpReport AsSorted(this AgpReport report)
        {
            var result = new AgpReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            result.Activities.AddRange(report.Activities.AsSorted());
            result.StaffActivities.AddRange(report.StaffActivities.AsSorted());

            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));

            return result;
        }


    }
}