using System;
using System.Collections.Generic;
using System.Linq;

namespace Vodamep.ReportBase
{
    internal abstract class ActivityReportDifferBase<T>  : ReportDifferBase where T : Enum
    {
        protected DiffObject DiffActivities(IEnumerable<IActivity<T>> activities1, IEnumerable<IActivity<T>> activities2)
        {

            bool isChanged = false;

            for (int i = 0; i < activities1.Count(); i++)
            {
                var activity = activities1.ElementAt(i);
                var otherActivity = activities2.FirstOrDefault(x => x.DateD == activity.DateD &&
                                                                    x.PersonId == activity.PersonId &&
                                                                    x.StaffId == activity.StaffId);

                if (otherActivity == null)
                {
                    isChanged = true;
                    break;
                }

                isChanged |= !activity.EntriesT.SequenceEqual(otherActivity.EntriesT);

            }

            for (int i = 0; i < activities2.Count(); i++)
            {
                var activity = activities2.ElementAt(i);
                var otherActivity = activities1.FirstOrDefault(x => x.DateD == activity.DateD &&
                                                                    x.PersonId == activity.PersonId &&
                                                                    x.StaffId == activity.StaffId);

                if (otherActivity == null)
                {
                    isChanged = true;
                    break;
                }

                isChanged |= !activity.EntriesT.SequenceEqual(otherActivity.EntriesT);

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

        protected DiffObject DiffActivitiesByActivityType(IEnumerable<IActivity<T>> activities1, IEnumerable<IActivity<T>> activities2,
            Func<IActivity<T>, bool> activityFilter, Func<IActivity<T>, IActivity<T>, bool> activityFindCriteria, DifferenceIdType differenceIdType)
        {
            var result = new DiffObject();
            var sum1 = 0;
            var sum2 = 0;

            result.Section = Section.Summary;
            result.DifferenceId = differenceIdType;
            result.Order = 1;
            result.Type = typeof(int);

            var isEntryTypeChanged = false;

            foreach (var activity in activities1.Where(activityFilter))
            {
                sum1 += activity.EntriesT.Count();

                var otherActivitys = activities2.Where(x => activityFindCriteria(x, activity));
                var otherActivity = activities2.FirstOrDefault(x => activityFindCriteria(x, activity));
               
                if (otherActivity != null)
                {
                    isEntryTypeChanged |= this.AreChanged(activity.EntriesT, otherActivity.EntriesT);
                }
            }

            foreach (var activity in activities2.Where(activityFilter))
            {
                sum2 += activity.EntriesT.Count();

                var otherActivity = activities1.FirstOrDefault(x => activityFindCriteria(x, activity));
                if (otherActivity != null)
                {
                    isEntryTypeChanged |= this.AreChanged(activity.EntriesT, otherActivity.EntriesT);
                }
            }

            result.Difference = !isEntryTypeChanged && sum1 == sum2 ? Difference.Unchanged : Difference.Difference;
            result.Value1 = sum1;
            result.Value2 = sum2;

            return result;
        }

        private bool AreChanged(IEnumerable<T> activities1, IEnumerable<T> activities2)
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