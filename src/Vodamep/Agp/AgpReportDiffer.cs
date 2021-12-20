using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Vodamep.Agp.Model;
using Vodamep.ReportBase;

namespace Vodamep.Agp
{
    internal class AgpReportDiffer : ActivityReportDifferBase<ActivityType>
    {

        public AgpReportDiffer()
        {
            this.DiffFunctions.Add(typeof(RepeatedField<Person>), this.DiffPersons);
            this.DiffFunctions.Add(typeof(RepeatedField<Staff>), this.DiffStaffs);
            this.DiffFunctions.Add(typeof(RepeatedField<Activity>), this.DiffActivities);
            this.DiffFunctions.Add(typeof(RepeatedField<StaffActivity>), this.DiffStaffActivities);

        }

        public List<DiffObject> DiffList(AgpReport report1, AgpReport report2)
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

        private IEnumerable<DiffObject> DiffStaffs(object obj1, object obj2)
        {
            var staffs1 = (obj1 as RepeatedField<Staff>);
            var staffs2 = (obj2 as RepeatedField<Staff>);

            return this.DiffItems(staffs1, staffs2, DifferenceIdType.Staff);
        }

        private IEnumerable<DiffObject> DiffPersons(object obj1, object obj2)
        {
            var persons1 = (obj1 as RepeatedField<Person>);
            var persons2 = (obj2 as RepeatedField<Person>);

            return DiffItems(persons1, persons2, DifferenceIdType.Person);
        }

        private IEnumerable<DiffObject> DiffActivities(object obj1, object obj2)
        {

            var activities1 = (obj1 as RepeatedField<Activity>)?.Select(x => x as IEntriesActivity<ActivityType>).ToList();
            var activities2 = (obj2 as RepeatedField<Activity>)?.Select(x => x as IEntriesActivity<ActivityType>).ToList();

            var list = new List<DiffObject>
            {
                base.DiffActivities(activities1, activities2),

                this.DiffActivitiesByActivityType(activities1, activities2,  (x) =>!string.IsNullOrWhiteSpace(x.StaffId), (x, y) => x.StaffId == y.StaffId && x.DateD == y.DateD, DifferenceIdType.StaffActivity),
                this.DiffActivitiesByActivityType(activities1, activities2,  (x) =>!string.IsNullOrWhiteSpace(x.PersonId), (x, y) => x.PersonId == y.PersonId && x.DateD == y.DateD, DifferenceIdType.PersonActivity),
            };

            return list;
        }


        private IEnumerable<DiffObject> DiffStaffActivities(object obj1, object obj2)
        {
            var staffs1 = (obj1 as RepeatedField<StaffActivity>).ToList();
            var staffs2 = (obj2 as RepeatedField<StaffActivity>).ToList();

            return DiffItems(staffs1, staffs2, DifferenceIdType.StaffActivity);
        }

    }
}