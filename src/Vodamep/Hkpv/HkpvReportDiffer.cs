using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Vodamep.Hkpv.Model;
using Vodamep.Hkpv.Validation;

namespace Vodamep.Hkpv
{
    internal class HkpvReportDiffer
    {
        private HkpReportDiffResult Diff(object value1, object value2, string name, int level)
        {
            var result = new HkpReportDiffResult();

            result.PropertyName = name;
            result.Type = value1?.GetType();
            result.Value1 = value1;
            result.Value2 = value2;
            result.Status = value1?.ToString() == value2?.ToString() ? Status.Unchanged : Status.Changed;

            return result;
        }

        public HkpReportDiffResult Diff(object obj1, object obj2, int level = 0)
        {
            if (obj1 == null || obj2 == null)
                return null;

            var result = new HkpReportDiffResult();

            result.Type = obj1.GetType();

            if (IsValueType(obj1.GetType()) && IsValueType(obj2.GetType()))
            {
                result.Children.Add(Diff(obj1, obj2, "", level));
            }

            var simpleProperties = obj1.GetType().GetProperties().Where(IsValueType).ToArray();
            foreach (var propertyInfo in simpleProperties)
            {
                var name = propertyInfo.Name;
                var value1 = propertyInfo.GetValue(obj1);
                var value2 = propertyInfo.GetValue(obj2);

                result.Children.Add(Diff(value1, value2, name, level));
            }

            var objectProperties = obj1.GetType().GetProperties().Where(a => !IsValueType(a) && !a.PropertyType.IsGenericType).ToArray();
            foreach (var propertyInfo in objectProperties)
            {
                if (propertyInfo.PropertyType == typeof(MessageParser<HkpvReport>) || propertyInfo.PropertyType == typeof(MessageDescriptor) || propertyInfo.PropertyType == typeof(Google.Protobuf.WellKnownTypes.Timestamp))
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
                        continue;

                    result.Children.Add(addElement);
                }

                //check deleted elements
                for (var i = 0; i < list1.Count; i++)
                {
                    var removedElement = this.FindNonExistingElements(list1[i], list2, Status.Removed);

                    if (removedElement == null)
                        continue;

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

            if (result.Status == Status.Unchanged)
            {
                foreach (var child in  result.Children)
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

        private HkpReportDiffResult FindNonExistingElements(object item, IList others, Status status)
        {
            var othersFirstItem = others != null && others.Count > 0 ? others[0] : null;

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
                var result = new HkpReportDiffResult();
                
                result.Value1 = item;
                result.Type = item.GetType();
                result.Status = others.Contains(item) ? Status.Unchanged : status;

                return result;
            }

            var propertyDefinitions = new[] { "Id", "DateD" };

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

                    var result = new HkpReportDiffResult();
                    result.PropertyName = id.ToString();
                    return result;
                }
            }

            var foundResult = new HkpReportDiffResult();
            foundResult.Status = Status.Unchanged;
            foundResult.Value1 =  item;
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