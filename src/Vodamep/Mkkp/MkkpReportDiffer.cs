using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Vodamep.Mkkp.Model;
using Vodamep.ReportBase;


namespace Vodamep.Mkkp
{
    internal class MkkpReportDiffer :  ActivityReportDifferBase<ActivityType>
    {
        public MkkpReportDiffer()
        {
            this.DiffFunctions.Add(typeof(RepeatedField<Person>), this.DiffPersons);
            this.DiffFunctions.Add(typeof(RepeatedField<Staff>), this.DiffStaffs);
            this.DiffFunctions.Add(typeof(RepeatedField<Activity>), this.DiffActivities);
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

            var activities1 = (obj1 as RepeatedField<Activity>)?.Select(x => x as IActivity<ActivityType>).ToList();
            var activities2 = (obj2 as RepeatedField<Activity>)?.Select(x => x as IActivity<ActivityType>).ToList();

            var list = new List<DiffObject>
            {
                base.DiffActivities(activities1, activities2),

                this.DiffActivitiesByActivityType(activities1, activities2,  (x) =>!string.IsNullOrWhiteSpace(x.StaffId), (x, y) => x.StaffId == y.StaffId && x.DateD == y.DateD, DifferenceIdType.StaffActivity),
                this.DiffActivitiesByActivityType(activities1, activities2,  (x) =>!string.IsNullOrWhiteSpace(x.PersonId), (x, y) => x.PersonId == y.PersonId && x.DateD == y.DateD, DifferenceIdType.PersonActivity),
            };

            return list;
        }


    }
}