using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using Vodamep.Data.Dummy;
namespace Vodamep.Hkpv.Model
{
    public partial class HkpvReport
    {
        public DateTime FromD { get => this.From.AsDate(); set => this.From = value.AsTimestamp(); }

        public DateTime ToD { get => this.To.AsDate(); set => this.To = value.AsTimestamp(); }


        public static HkpvReport CreateDummyData()
        {
            var r = new HkpvReport()
            {
                Institution = new Institution()
                {
                    Id = "test",
                    Name = "Test"
                }
            };

            r.FromD = DateTime.Today.FirstDateInMonth().AddMonths(-1);
            r.ToD = r.FromD.LastDateInMonth();

            r.AddDummyPerson();
            r.AddDummyStaff();
            r.AddDummyActivities();

            return r.AsSorted();

        }

        public static HkpvReport ReadFile(string file)
        {
            var report = new HkpvReportSerializer().DeserializeFile(file);
            return report;
        }

        public static HkpvReport Read(byte[] data)
        {
            var report = new HkpvReportSerializer().Deserialize(data);
            return report;
        }

        public static HkpvReport Read(Stream data)
        {
            var report = new HkpvReportSerializer().Deserialize(data);
            return report;
        }

        public string WriteToPath(string path, bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToPath(this, path, asJson, compressed);

        public void WriteToFile(string filename, bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToFile(this, filename, asJson, compressed);

        public MemoryStream WriteToStream(bool asJson = false, bool compressed = true) => new HkpvReportSerializer().WriteToStream(this, asJson, compressed);

        public string[] Diff(HkpvReport report)
        {
            return Diff(this, report);
        }

        private string[] Diff(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null)
                return new string[2];

            if (obj1 == null)
                return new string[2];

            if (obj2 == null)
                return new string[2];

            var result = new string[2];

            var simpleproperties = obj1.GetType().GetProperties().Where(IsValueType).ToArray();
            foreach (var propertyInfo in simpleproperties)
            {
                var name = propertyInfo.Name;

                var value1 = propertyInfo.GetValue(obj1);
                var value2 = propertyInfo.GetValue(obj2);
            }

            var objectProperties = obj1.GetType().GetProperties().Where(a => !IsValueType(a) && !a.PropertyType.IsGenericType).ToArray();
            foreach (var propertyInfo in objectProperties)
            {
                if (propertyInfo.PropertyType == typeof(MessageParser<HkpvReport>) || propertyInfo.PropertyType == typeof(MessageDescriptor))
                {
                    continue;
                }

                var name = propertyInfo.Name;

                var subProperty1 = propertyInfo.GetValue(obj1);
                var subProperty2 = propertyInfo.GetValue(obj2);

                Diff(subProperty1, subProperty2);

            }

            var collections = obj1.GetType().GetProperties().Where(a => a.PropertyType.IsGenericType);
            foreach (var propertyInfoCollection in collections)
            {
                if (propertyInfoCollection.PropertyType.BaseType == typeof(MessageParser) || propertyInfoCollection.PropertyType == typeof(MessageDescriptor))
                {
                    continue;
                }

                var enumerable = propertyInfoCollection.PropertyType == typeof(IEnumerable);

             
            }

            return result;
        }

        private bool IsValueType(PropertyInfo a)
        {
            return a.PropertyType.IsPrimitive ||
                   a.PropertyType == typeof(string) ||
                   a.PropertyType.IsValueType ||
                   a.PropertyType == typeof(decimal) ||
                   a.PropertyType == typeof(DateTime);
        }
    }
}