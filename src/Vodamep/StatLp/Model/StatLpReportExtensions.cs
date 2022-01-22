using System;
using System.Collections.Generic;
using System.Linq;

namespace Vodamep.StatLp.Model
{
    public static class StatLpReportExtensions
    {

        public static StatLpReport AddPerson(this StatLpReport report, Person person) => report.InvokeAndReturn(m => m.Persons.Add(person));
        public static StatLpReport AddPersons(this StatLpReport report, IEnumerable<Person> persons) => report.InvokeAndReturn(m => m.Persons.AddRange(persons));
        public static StatLpReport AddAdmission(this StatLpReport report, Admission admission) => report.InvokeAndReturn(m => m.Admissions.Add(admission));
        public static StatLpReport AddAdmissions(this StatLpReport report, IEnumerable<Admission> admissions) => report.InvokeAndReturn(m => m.Admissions.AddRange(admissions));
        public static StatLpReport AddAttribute(this StatLpReport report, Attribute attribute) => report.InvokeAndReturn(m => m.Attributes.Add(attribute));
        public static StatLpReport AddAttributes(this StatLpReport report, IEnumerable<Attribute> attributes) => report.InvokeAndReturn(m => m.Attributes.AddRange(attributes));
        public static StatLpReport AddStay(this StatLpReport report, Stay stay) => report.InvokeAndReturn(m => m.Stays.Add(stay));
        public static StatLpReport AddStays(this StatLpReport report, IEnumerable<Stay> stays) => report.InvokeAndReturn(m => m.Stays.AddRange(stays));
        public static StatLpReport AddLeaving(this StatLpReport report, Leaving leaving) => report.InvokeAndReturn(m => m.Leavings.Add(leaving));
        public static StatLpReport AddLeavings(this StatLpReport report, IEnumerable<Leaving> leavings) => report.InvokeAndReturn(m => m.Leavings.AddRange(leavings));

        public static StatLpReport AsSorted(this StatLpReport report)
        {
            var result = new StatLpReport()
            {
                Institution = report.Institution,
                From = report.From,
                To = report.To
            };

            result.Admissions.AddRange(report.Admissions.OrderBy(x => x.PersonId).ThenBy(x => x.AdmissionDate));
            result.Attributes.AddRange(report.Attributes.OrderBy(x => x.PersonId).ThenBy(x => x.From).ThenBy(x => x.ValueCase));
            result.Leavings.AddRange(report.Leavings.OrderBy(x => x.PersonId).ThenBy(x => x.LeavingDateD));
            result.Persons.AddRange(report.Persons.OrderBy(x => x.Id));
            result.Stays.AddRange(report.Stays.OrderBy(x => x.PersonId).ThenBy(x => x.From));
            result.Aliases.AddRange(result.Aliases.OrderBy(x => x.Id1));

            return result;
        }

        public static IEnumerable<GroupedStay> GetGroupedStays(this StatLpReport report, string personId, GroupedStay.SameTypeyGroupMode sameTypeyGroupMode = GroupedStay.SameTypeyGroupMode.NotAllowed)
        {
            var result = new List<(DateTime From, DateTime To, Stay[] Stays)>();

            var stays = report.Stays.Where(x => x.PersonId == personId).OrderBy(x => x.From).ToArray();

            return stays.GetGroupedStays(sameTypeyGroupMode);
        }

        public static IEnumerable<GroupedStay> GetGroupedStays(this Stay[] stays, GroupedStay.SameTypeyGroupMode sameTypeyGroupMode = GroupedStay.SameTypeyGroupMode.NotAllowed)
        {
            if (stays.Length == 0)
                yield break;

            var current = new GroupedStay(stays[0].FromD, stays[0].ToD, new[] { stays[0] });

            foreach (var stay in stays.Skip(1))
            {
                var lastStay = current.Stays.Last();
                if (lastStay.PersonId != stay.PersonId)
                {
                    throw new Exception("Es können keine Aufenthalte unterschiedlicher Personen gruppiert werden!");
                }

                if (lastStay.FromD > stay.FromD)
                {
                    throw new Exception("Die Aufenthalte müssen vor dem Gruppieren nach Datum sortiert sein!");
                }

                if (current.To == null)
                {
                    throw new Exception("Die Aufenthalte dürfen sich nicht überschneiden!");
                }

                if (current.To.Value.AddDays(1) == stay.FromD)
                {
                    if (sameTypeyGroupMode != GroupedStay.SameTypeyGroupMode.Ignore && lastStay.Type == stay.Type)
                    {
                        if (sameTypeyGroupMode == GroupedStay.SameTypeyGroupMode.NotAllowed)
                        {
                            throw new Exception("Aufeinanderfolgende Aufenthalte müssen unterschiedliche Aufnahmearten haben!");
                        }

                        //zwei nacheinander folgende Aufenthalte mit gleichem Type sollen zu einem Aufenthalt vereint werden.
                        var newLastStay = new Stay(lastStay)
                        {
                            ToD = stay.ToD
                        };
                        var newStays = current.Stays.Take(current.Stays.Length - 1).Union(new[] { newLastStay }).ToArray();

                        current = new GroupedStay(current.From, stay.ToD, newStays);
                    }
                    else
                    {
                        current = new GroupedStay(current.From, stay.ToD, current.Stays.Union(new[] { stay }).ToArray());
                    }
                    continue;
                }
                else if (current.To.Value.AddDays(1) < stay.FromD)
                {
                    yield return current;
                    current = new GroupedStay(stay.FromD, stay.ToD, new[] { stay });
                    continue;
                }
                else if (current.To.Value.AddDays(1) >= stay.FromD)
                {
                    throw new Exception("Die Aufenthalte dürfen sich nicht überschneiden!");
                }

                throw new Exception("Die Aufenthalte können nicht verarbeitet werden!");
            }

            yield return current;
        }


    }
}