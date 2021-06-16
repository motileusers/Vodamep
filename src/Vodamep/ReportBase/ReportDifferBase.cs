using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Vodamep.ReportBase
{
    internal abstract class ReportDifferBase
    {
        protected Dictionary<Type, Func<object, object, IEnumerable<DiffObject>>> DiffFunctions = new Dictionary<Type, Func<object, object, IEnumerable<DiffObject>>>();

        protected IEnumerable<DiffObject> DiffItems(IEnumerable<IItem> items1, IEnumerable<IItem> items2, DifferenceIdType differenceId)
        {
            var result = new List<DiffObject>();

            result.AddRange(FindListChanges(items1, items2, differenceId, Difference.Added));
            result.AddRange(FindListChanges(items2, items1, differenceId, Difference.Missing));
            result.AddRange(FindChangedItems(items1, items2, differenceId));

            return result;
        }

        protected IEnumerable<DiffObject> FindListChanges(IEnumerable<IItem> items1, IEnumerable<IItem> items2, DifferenceIdType differenceId, Difference difference)
        {
            return items2.Where(x => items1.All(y => x.Id != y.Id))
                .Select(z =>
                    new DiffObject
                    {
                        Section = Section.Summary,
                        DifferenceId = differenceId,
                        Order = 4,
                        Type = typeof(int),
                        Difference = difference,
                        Value1 = z.Id
                    });
        }

        protected IEnumerable<DiffObject> FindChangedItems(IEnumerable<IItem> items1, IEnumerable<IItem> items2, DifferenceIdType differenceId)
        {
            var result = new List<DiffObject>();

            foreach (var person in items1)
            {
                var otherPerson = items2.FirstOrDefault(x => x.Id == person.Id);

                if (otherPerson == null)
                {
                    continue;
                }

                var diff = this.Diff(person, otherPerson);

                if (diff.Status == Status.Changed)
                {
                    result.Add(new DiffObject
                    {
                        Section = Section.Summary,
                        DifferenceId = differenceId,
                        Order = 5,
                        Difference = Difference.Difference,
                        Type = typeof(int),
                        Value1 = person.Id,
                    });
                }
            }

            return result;

        }

        public List<DiffObject> DiffList(IReport report1, IReport report2)
        {
            var result = new List<DiffObject>();

            if (report1.GetType() != report2.GetType())
            {
                result.Add(new DiffObject
                {
                    DataDescription = "Reports are different",
                    Value1 = report1.GetType(),
                    Value2 = report2.GetType(),
                    Difference = Difference.Difference
                });
            }
            else
            {
                if (DiffFunctions.ContainsKey(report1.GetType()))
                {
                    result.AddRange(DiffFunctions[report1.GetType()](report1, report2));
                }

                var properties = report1.GetType().GetProperties();

                foreach (var property in properties)
                {
                    if (DiffFunctions.ContainsKey(property.PropertyType))
                    {
                        result.AddRange(DiffFunctions[property.PropertyType](property.GetValue(report1), property.GetValue(report2)));
                    }
                }
            }

            result.RemoveAll(x => x.Difference == Difference.Unchanged);

            return result;
        }

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
                    if (propertyInfo.PropertyType == typeof(MessageParser) || propertyInfo.PropertyType == typeof(MessageDescriptor) || propertyInfo.PropertyType == typeof(Google.Protobuf.WellKnownTypes.Timestamp))
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