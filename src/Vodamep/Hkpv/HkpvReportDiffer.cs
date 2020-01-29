using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv
{
    internal enum DiffStatus
    {
        UnModified,
        Added,
        Removed,
        Modified
    }

    internal class HkpvReportDiffer
    {
        private string CreateTabs(int level)
        {
            var tabString = "";
            for (int i = 0; i < level; i++)
            {
                tabString += "\t";
            }
            return tabString;
        }

        private string[] CreateResultLine(object value1, object value2, string name, int level)
        {
            var tabs = CreateTabs(level);
            var resultLine = new string[4];
            resultLine[1] = name;

            if (value1.ToString() == value2.ToString())
            {
                resultLine[0] = DiffStatus.UnModified.ToString();
                resultLine[2] = tabs + value1;
                resultLine[3] = tabs + value2;
            }
            else
            {
                resultLine[0] = DiffStatus.Modified.ToString();
                resultLine[2] = tabs + value1;
                resultLine[3] = tabs + value2;
            }

            return resultLine;
        }

        public IEnumerable<string[]> Diff(object obj1, object obj2, int level = 0)
        {
            if (obj1 == null && obj2 == null)
                return new List<string[]>(); ;

            if (obj1 == null)
                return new List<string[]>(); ;

            if (obj2 == null)
                return new List<string[]>(); ;

            var result = new List<string[]>();

            //var titlestring = CreateTabs(level);
            //titlestring += $"Typ: {obj1.GetType().Name}";
            ////result.Add(titlestring);

            if (IsValueType(obj1.GetType()) && IsValueType(obj2.GetType()))
            {
                result.Add(CreateResultLine(obj1, obj2, "", level));
            }

            var simpleProperties = obj1.GetType().GetProperties().Where(IsValueType).ToArray();
            foreach (var propertyInfo in simpleProperties)
            {
                var name = propertyInfo.Name;
                var value1 = propertyInfo.GetValue(obj1);
                var value2 = propertyInfo.GetValue(obj2);

                result.Add(CreateResultLine(value1, value2, name, level));
            }
            //result.Add(Environment.NewLine);

            var objectProperties = obj1.GetType().GetProperties().Where(a => !IsValueType(a) && !a.PropertyType.IsGenericType).ToArray();
            foreach (var propertyInfo in objectProperties)
            {
                if (propertyInfo.PropertyType == typeof(MessageParser<HkpvReport>) || propertyInfo.PropertyType == typeof(MessageDescriptor) || propertyInfo.PropertyType == typeof(Google.Protobuf.WellKnownTypes.Timestamp))
                {
                    continue;
                }

                var subProperty1 = propertyInfo.GetValue(obj1);
                var subProperty2 = propertyInfo.GetValue(obj2);

                result.AddRange(Diff(subProperty1, subProperty2, level + 1));
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

                for (var i = 0; i < list1.Count; i++)
                {
                    result.AddRange(Diff(list1[i], list2[i], level + 1));
                }
            }

            return result.ToArray();
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