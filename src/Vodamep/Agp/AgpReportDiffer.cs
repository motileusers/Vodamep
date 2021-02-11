using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Vodamep.Hkpv;
using Vodamep.Mkkp.Model;
using Vodamep.ReportBase;

namespace Vodamep.Mkkp
{
    internal class AgpReportDiffer
    {
        public DiffResult Diff(object obj1, object obj2, int level = 0, bool ignoreChildLevels = false)
        {
            if (obj1 == null || obj2 == null)
                return null;

            var result = new DiffResult();

            var simpleProperties = obj1.GetType().GetProperties().Where(IsValueType).ToArray();
            foreach (var propertyInfo in simpleProperties)
            {
                var name = propertyInfo.Name;
                var value1 = propertyInfo.GetValue(obj1);
                var value2 = propertyInfo.GetValue(obj2);

                result.Children.Add(Diff(value1, value2, name, level));
            }

            result.Type = obj1.GetType();

            if (!ignoreChildLevels)
            {
                if (IsValueType(obj1.GetType()) && IsValueType(obj2.GetType()))
                {
                    result.Children.Add(Diff(obj1, obj2, "", level));
                }

                var objectProperties = obj1.GetType().GetProperties().Where(a => !IsValueType(a) && !a.PropertyType.IsGenericType).ToArray();
                foreach (var propertyInfo in objectProperties)
                {
                    if (propertyInfo.PropertyType == typeof(MessageParser<MkkpReport>) || propertyInfo.PropertyType == typeof(MessageDescriptor) || propertyInfo.PropertyType == typeof(Google.Protobuf.WellKnownTypes.Timestamp))
                    {
                        continue;
                    }

                    var subProperty1 = propertyInfo.GetValue(obj1);
                    var subProperty2 = propertyInfo.GetValue(obj2);

                    result.Children.Add(Diff(subProperty1, subProperty2, level + 1));
                }

                var collections = obj1.GetType().GetProperties().Where(a => a.PropertyType.IsGenericType);
                foreach (var propertyInfoCollection in collections)
                {
                    if (propertyInfoCollection.PropertyType.BaseType == typeof(MessageParser) || propertyInfoCollection.PropertyType == typeof(MessageDescriptor))
                    {
                        continue;
                    }

                    var list1 = propertyInfoCollection.GetValue(obj1) as IList;
                    var list2 = propertyInfoCollection.GetValue(obj2) as IList;

                    if (list1 == null)
                    {
                        //todo
                        continue;
                    }

                    if (list2 == null)
                    {
                        //todo
                        continue;
                    }

                    //check added elements
                    for (var i = 0; i < list2.Count; i++)
                    {
                        var addElement = this.FindNonExistingElements(list2[i], list1, Status.Added);

                        if (addElement == null)
                        {
                            if (list1.Count != list2.Count)
                            {
                                addElement = new DiffResult();
                                addElement.Status = Status.Added;
                                addElement.Value1 = list2[i];
                                addElement.Value2 = list2[i];

                            }
                            else
                            {
                                continue;
                            }
                        }

                        result.Children.Add(addElement);
                    }

                    //check deleted elements
                    for (var i = 0; i < list1.Count; i++)
                    {
                        var removedElement = this.FindNonExistingElements(list1[i], list2, Status.Removed);

                        if (removedElement == null)
                        {
                            if (list1.Count != list2.Count)
                            {
                                removedElement = new DiffResult();
                                removedElement.Status = Status.Removed;
                                removedElement.Value1 = list1[i];
                                removedElement.Value2 = list1[i];

                            }
                            else
                            {
                                continue;
                            }
                        }

                        result.Children.Add(removedElement);
                    }

                    //compare existing elements
                    for (var i = 0; i < list1.Count && i < list2.Count; i++)
                    {
                        if (!IsValueType(list1[i].GetType()))
                        {
                            result.Children.Add(Diff(list1[i], list2[i], level + 1));
                        }
                    }
                }
            }

            if (result.Status == Status.Unchanged)
            {
                foreach (var child in result.Children)
                {
                    if (child.Status != Status.Unchanged)
                    {
                        result.Status = Status.Changed;
                        break;
                    }
                }
            }

            result.Status = result.Status != Status.Unchanged ? result.Status :
                result.Children.Where(x => x != null).Any(y => y.Status != Status.Unchanged) ? Status.Changed :
                Status.Unchanged;

            return result;
        }

        public List<DiffObject> DiffList(MkkpReport report1, MkkpReport report2)
        {
            var result = new List<DiffObject>();

            result.Add(this.FindChangedActivity(report1, report2));
            result.Add(this.DiffStaffActivity(report1, report2));
            result.Add(this.DiffPersonActivity(report1, report2));
            result.Add(this.DiffEmployments(report1, report2));

            result.AddRange(this.FindAddPersons(report1, report2));
            result.AddRange(this.FindChangedPersons(report1, report2));
            result.AddRange(this.FindMissingPersons(report1, report2));

            result.AddRange(this.FindAddStaff(report1, report2));
            result.AddRange(this.FindChangedStaff(report1, report2));
            result.AddRange(this.FindMissingStaffs(report1, report2));

            result.RemoveAll(x => x.Difference == Difference.Unchanged);

            return result;
        }

        private DiffObject DiffStaffActivity(MkkpReport report1, MkkpReport report2)
        {
            var result = new DiffObject();
            var sum1 = 0;
            var sum2 = 0;

            result.Section = Section.Summary;
            result.DifferenceId = DifferenceIdType.StaffActivity;
            result.Order = 1;
            result.Type = typeof(int);

            var isEntryTypeChanged = false;

            foreach (var activity in report1.Activities.Where(x => !string.IsNullOrWhiteSpace(x.StaffId)))
            {
                sum1 += activity.Entries.Count;

                var otherActivity = report2.Activities.FirstOrDefault(x => x.StaffId == activity.StaffId && x.DateD == activity.DateD);
                if (otherActivity != null)
                {
                    isEntryTypeChanged |= this.AreChanged(activity.Entries, otherActivity.Entries);
                }
            }

            foreach (var activity in report2.Activities.Where(x => !string.IsNullOrWhiteSpace(x.StaffId)))
            {
                sum2 += activity.Entries.Count;

                var otherActivity = report1.Activities.FirstOrDefault(x => x.StaffId == activity.StaffId && x.DateD == activity.DateD);
                if (otherActivity != null)
                {
                    isEntryTypeChanged |= this.AreChanged(activity.Entries, otherActivity.Entries);
                }
            }

            result.Difference = !isEntryTypeChanged && sum1 == sum2 ? Difference.Unchanged : Difference.Difference;
            result.Value1 = sum1;
            result.Value2 = sum2;

            return result;
        }

        private DiffObject DiffPersonActivity(MkkpReport report1, MkkpReport report2)
        {
            var result = new DiffObject();
            var sum1 = 0;
            var sum2 = 0;

            result.Section = Section.Summary;
            result.DifferenceId = DifferenceIdType.PersonActivity;
            result.Order = 2;
            result.Type = typeof(int);

            var isEntryTypeChanged = false;

            foreach (var activity in report1.Activities.Where(x => !string.IsNullOrWhiteSpace(x.PersonId)))
            {
                sum1 += activity.Entries.Count;

                var otherActivity = report2.Activities.FirstOrDefault(x => x.PersonId == activity.PersonId && x.DateD == activity.DateD);
                if (otherActivity != null)
                {
                    isEntryTypeChanged |= this.AreChanged(activity.Entries, otherActivity.Entries);
                }
            }

            foreach (var activity in report2.Activities.Where(x => !string.IsNullOrWhiteSpace(x.PersonId)))
            {
                sum2 += activity.Entries.Count;

                var otherActivity = report1.Activities.FirstOrDefault(x => x.PersonId == activity.PersonId && x.DateD == activity.DateD);
                if (otherActivity != null)
                {
                    isEntryTypeChanged |= this.AreChanged(activity.Entries, otherActivity.Entries);
                }
            }

            result.Difference = !isEntryTypeChanged && sum1 == sum2 ? Difference.Unchanged : Difference.Difference;
            result.Value1 = sum1;
            result.Value2 = sum2;

            return result;
        }


        private DiffObject DiffEmployments(MkkpReport report1, MkkpReport report2)
        {
            //double standardHoursPerWeek = 40;

            var result = new DiffObject();
            var sum1 = 0.0;
            var sum2 = 0.0;

            result.Section = Section.Summary;
            result.DifferenceId = DifferenceIdType.Employment;
            result.Order = 3;
            result.Type = typeof(double);

            foreach (var staff in report1.Staffs)
            {
                //foreach (var employment in staff.Employments)
                //{
                //    sum1 += employment.HoursPerWeek / standardHoursPerWeek;
                //}
            }

            foreach (var staff in report2.Staffs)
            {
                //foreach (var employment in staff.Employments)
                //{
                //    sum2 += employment.HoursPerWeek / standardHoursPerWeek;
                //}
            }

            result.Difference = Math.Abs(sum1 - sum2) <= 0 ? Difference.Unchanged : Difference.Difference;
            result.Value1 = sum1;
            result.Value2 = sum2;

            return result;
        }

        private IEnumerable<DiffObject> FindAddPersons(MkkpReport report1, MkkpReport report2)
        {
            return report2.Persons.Where(x => report1.Persons.All(y => x.Id != y.Id))
                .Select(z =>
                    new DiffObject
                    {
                        Section = Section.Summary,
                        DifferenceId = DifferenceIdType.Person,
                        Order = 4,
                        Type = typeof(int),
                        Difference = Difference.Added,
                        Value1 = z.Id
                    });
        }

        private IEnumerable<DiffObject> FindChangedPersons(MkkpReport report1, MkkpReport report2)
        {
            var result = new List<DiffObject>();

            foreach (var person in report1.Persons)
            {
                var otherPerson = report2.Persons.FirstOrDefault(x => x.Id == person.Id);

                if (otherPerson == null)
                {
                    continue;
                }

                var diff = this.Diff(person, otherPerson, 0, true);

                if (diff.Status == Status.Changed)
                {
                    result.Add(new DiffObject
                    {
                        Section = Section.Summary,
                        DifferenceId = DifferenceIdType.Person,
                        Order = 5,
                        Difference = Difference.Difference,
                        Type = typeof(int),
                        Value1 = person.Id,
                    });
                }
            }

            return result;

        }

        private IEnumerable<DiffObject> FindMissingPersons(MkkpReport report1, MkkpReport report2)
        {
            return report1.Persons.Where(x => report2.Persons.All(y => x.Id != y.Id))
                .Select(z =>
                    new DiffObject
                    {
                        Section = Section.Summary,
                        DifferenceId = DifferenceIdType.Person,
                        Order = 6,
                        Type = typeof(int),
                        Difference = Difference.Missing,
                        Value1 = z.Id
                    });
        }

        private IEnumerable<DiffObject> FindAddStaff(MkkpReport report1, MkkpReport report2)
        {
            return report2.Staffs.Where(x => report1.Staffs.All(y => x.Id != y.Id))
                .Select(z =>
                    new DiffObject
                    {
                        Section = Section.Summary,
                        DifferenceId = DifferenceIdType.Staff,
                        Order = 4,
                        Type = typeof(int),
                        Difference = Difference.Added,
                        Value1 = z.Id
                    });
        }

        private IEnumerable<DiffObject> FindChangedStaff(MkkpReport report1, MkkpReport report2)
        {
            var result = new List<DiffObject>();

            foreach (var staff in report1.Staffs)
            {
                var otherStaff = report2.Staffs.FirstOrDefault(x => x.Id == staff.Id);

                if (otherStaff == null)
                {
                    continue;
                }

                var diff = this.Diff(staff, otherStaff);


                if (diff.Status == Status.Changed)
                {
                    result.Add(new DiffObject
                    {
                        Section = Section.Summary,
                        DifferenceId = DifferenceIdType.Staff,
                        Order = 5,
                        Difference = Difference.Difference,
                        Type = typeof(int),
                        Value1 = staff.Id,
                    });
                }
            }

            return result;

        }

        private IEnumerable<DiffObject> FindMissingStaffs(MkkpReport report1, MkkpReport report2)
        {
            return report1.Staffs.Where(x => report2.Staffs.All(y => x.Id != y.Id))
                .Select(z =>
                    new DiffObject
                    {
                        Section = Section.Summary,
                        DifferenceId = DifferenceIdType.Staff,
                        Order = 6,
                        Type = typeof(int),
                        Difference = Difference.Missing,
                        Value1 = z.Id
                    });
        }

        private DiffObject FindChangedActivity(MkkpReport report1, MkkpReport report2)
        {

            bool isChanged = false;

            for (int i = 0; i < report1.Activities.Count; i++)
            {
                var activity = report1.Activities[i];
                var otherActivity = report2.Activities.FirstOrDefault(x => x.DateD == activity.DateD &&
                                                                          x.PersonId == activity.PersonId &&
                                                                          x.StaffId == activity.StaffId);

                if (otherActivity == null)
                {
                    isChanged = true;
                    break;
                }

                isChanged |= !activity.Entries.SequenceEqual(otherActivity.Entries);

            }

            for (int i = 0; i < report2.Activities.Count; i++)
            {
                var activity = report2.Activities[i];
                var otherActivity = report1.Activities.FirstOrDefault(x => x.DateD == activity.DateD &&
                                                                           x.PersonId == activity.PersonId &&
                                                                           x.StaffId == activity.StaffId);

                if (otherActivity == null)
                {
                    isChanged = true;
                    break;
                }

                isChanged |= !activity.Entries.SequenceEqual(otherActivity.Entries);

            }


            return new DiffObject
            {
                Section = Section.Summary,
                DifferenceId = DifferenceIdType.Activity,
                Order = 0,
                //Difference = isChanged ? Difference.Difference : Difference.Unchanged,
                Difference = Difference.Unchanged
            };
        }

        private bool AreChanged(IEnumerable<ActivityType> activities1, IEnumerable<ActivityType> activities2)
        {
            if (activities1 == null || activities2 == null)
            {
                return true;
            }

            var list1 = activities1.ToList();
            var list2 = activities2.ToList();

            if (list1.Count != list2.Count)
            {
                return true;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                var a = list1[i].ToString();
                var b = list2[i].ToString();

                if (list1[i].ToString().CompareTo(list2[i].ToString()) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        private DiffResult Diff(object value1, object value2, string name, int level)
        {
            var result = new DiffResult();

            result.PropertyName = name;
            result.Type = value1?.GetType();
            result.Value1 = value1;
            result.Value2 = value2;
            result.Status = value1?.ToString() == value2?.ToString() ? Status.Unchanged : Status.Changed;

            return result;
        }

        private DiffResult FindNonExistingElements(object item, IList others, Status status)
        {
            var othersFirstItem = others != null && others.Count > 0 ? others[0] : null;

            var type = item.GetType();

            if (othersFirstItem == null)
            {
                return null;
            }

            if (item == null || item.GetType() != othersFirstItem.GetType())
            {
                return null;
            }

            var itemProperties = item.GetType().GetProperties().ToArray();

            if (IsValueType(item.GetType()))
            {
                var result = new DiffResult();

                result.Value1 = item;
                result.Type = item.GetType();
                result.Status = others.Contains(item) ? Status.Unchanged : status;

                return result;
            }

            var propertyDefinitions = new[] { "Id", "DateD", "From" };

            foreach (var propertyDefinition in propertyDefinitions)
            {
                var propertyInfo = itemProperties.FirstOrDefault(x => x.Name == propertyDefinition);

                if (propertyInfo != null)
                {
                    var id = propertyInfo.GetValue(item);

                    foreach (var othersItem in others)
                    {
                        var othersId = propertyInfo.GetValue(othersItem);
                        if (othersId.ToString() == id.ToString())
                        {
                            return null;
                        }
                    }

                    var result = new DiffResult();
                    result.PropertyName = id.ToString();
                    return result;
                }
            }

            var foundResult = new DiffResult();
            foundResult.Status = Status.Unchanged;
            foundResult.Value1 = item;
            foundResult.Value2 = item;

            return foundResult;
        }

        private bool IsValueType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsPrimitive ||
                   propertyInfo.PropertyType == typeof(string) ||
                   propertyInfo.PropertyType.IsValueType ||
                   propertyInfo.PropertyType == typeof(decimal) ||
                   propertyInfo.PropertyType == typeof(DateTime);
        }

        private bool IsValueType(Type type)
        {
            return type.IsPrimitive ||
                   type == typeof(string) ||
                   type.IsValueType ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime);
        }
    }
}