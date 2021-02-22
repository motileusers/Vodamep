using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

                var diff = this.Diff(person, otherPerson, 0);

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

        public List<DiffObject> DiffList(IReportBase report1, IReportBase report2)
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

        public DiffResult Diff(object obj1, object obj2, int level = 0)
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

        private bool IsValueType(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsPrimitive ||
                   propertyInfo.PropertyType == typeof(string) ||
                   propertyInfo.PropertyType.IsValueType ||
                   propertyInfo.PropertyType == typeof(decimal) ||
                   propertyInfo.PropertyType == typeof(DateTime);
        }

    }
}