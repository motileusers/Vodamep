using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Vodamep.Mkkp.Model;
using Vodamep.ReportBase;


namespace Vodamep.Mkkp
{
    internal class MkkpReportDiffer : ReportDifferBase
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

            var activities1 = obj1 as RepeatedField<Activity>;
            var activities2 = obj2 as RepeatedField<Activity>;

            var list = new List<DiffObject>
            {
                this.DiffActivities(activities1, activities2),

                this.DiffStaffActivity(activities1, activities2),
                this.DiffPersonActivity(activities1, activities2),
            };

            return list;
        }

        private DiffObject DiffActivities(RepeatedField<Activity> activities1, RepeatedField<Activity> activities2)
        {

            bool isChanged = false;

            for (int i = 0; i < activities1.Count; i++)
            {
                var activity = activities1[i];
                var otherActivity = activities2.FirstOrDefault(x => x.DateD == activity.DateD &&
                                                                    x.PersonId == activity.PersonId &&
                                                                    x.StaffId == activity.StaffId);

                if (otherActivity == null)
                {
                    isChanged = true;
                    break;
                }

                isChanged |= !activity.Entries.SequenceEqual(otherActivity.Entries);

            }

            for (int i = 0; i < activities2.Count; i++)
            {
                var activity = activities2[i];
                var otherActivity = activities1.FirstOrDefault(x => x.DateD == activity.DateD &&
                                                                    x.PersonId == activity.PersonId &&
                                                                    x.StaffId == activity.StaffId);

                if (otherActivity == null)
                {
                    isChanged = true;
                    break;
                }

                isChanged |= !activity.Entries.SequenceEqual(otherActivity.Entries);

            }


            return
                new DiffObject
                {
                    Section = Section.Summary,
                    DifferenceId = DifferenceIdType.Activity,
                    Order = 0,
                    //Difference = isChanged ? Difference.Difference : Difference.Unchanged,
                    Difference = Difference.Unchanged

                };
        }

        private DiffObject DiffStaffActivity(RepeatedField<Activity> activities1, RepeatedField<Activity> activities2)
        {
            var result = new DiffObject();
            var sum1 = 0;
            var sum2 = 0;

            result.Section = Section.Summary;
            result.DifferenceId = DifferenceIdType.StaffActivity;
            result.Order = 1;
            result.Type = typeof(int);

            var isEntryTypeChanged = false;

            foreach (var activity in activities1.Where(x => !string.IsNullOrWhiteSpace(x.StaffId)))
            {
                sum1 += activity.Entries.Count;

                var otherActivity = activities2.FirstOrDefault(x => x.StaffId == activity.StaffId && x.DateD == activity.DateD);
                if (otherActivity != null)
                {
                    isEntryTypeChanged |= this.AreChanged(activity.Entries, otherActivity.Entries);
                }
            }

            foreach (var activity in activities2.Where(x => !string.IsNullOrWhiteSpace(x.StaffId)))
            {
                sum2 += activity.Entries.Count;

                var otherActivity = activities1.FirstOrDefault(x => x.StaffId == activity.StaffId && x.DateD == activity.DateD);
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

        private DiffObject DiffPersonActivity(RepeatedField<Activity> activities1, RepeatedField<Activity> activities2)
        {
            var result = new DiffObject();
            var sum1 = 0;
            var sum2 = 0;

            result.Section = Section.Summary;
            result.DifferenceId = DifferenceIdType.PersonActivity;
            result.Order = 2;
            result.Type = typeof(int);

            var isEntryTypeChanged = false;

            foreach (var activity in activities1.Where(x => !string.IsNullOrWhiteSpace(x.PersonId)))
            {
                sum1 += activity.Entries.Count;

                var otherActivity = activities2.FirstOrDefault(x => x.PersonId == activity.PersonId && x.DateD == activity.DateD);
                if (otherActivity != null)
                {
                    isEntryTypeChanged |= this.AreChanged(activity.Entries, otherActivity.Entries);
                }
            }

            foreach (var activity in activities2.Where(x => !string.IsNullOrWhiteSpace(x.PersonId)))
            {
                sum2 += activity.Entries.Count;

                var otherActivity = activities1.FirstOrDefault(x => x.PersonId == activity.PersonId && x.DateD == activity.DateD);
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

    }
}