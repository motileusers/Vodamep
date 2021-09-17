using Vodamep.ReportBase;

namespace Vodamep.StatLp
{
    internal class StatLpReportDiffer : ReportDifferBase
    {
        public StatLpReportDiffer()
        {
            //this.DiffFunctions.Add(typeof(RepeatedField<Person>), this.DiffPersons);
            //this.DiffFunctions.Add(typeof(RepeatedField<Staff>), this.DiffStaffs);
            //this.DiffFunctions.Add(typeof(RepeatedField<Activity>), this.DiffActivities);
        }

        //private IEnumerable<DiffObject> DiffStaffs(object obj1, object obj2)
        //{
        //    var staffs1 = (obj1 as RepeatedField<Staff>);
        //    var staffs2 = (obj2 as RepeatedField<Staff>);

        //    var result = new List<DiffObject>();

        //    result.AddRange(this.DiffItems(staffs1, staffs2, DifferenceIdType.Staff));
        //    result.Add(this.DiffEmployments(staffs1, staffs2));

        //    return result;
        //}

        //private IEnumerable<DiffObject> DiffPersons(object obj1, object obj2)
        //{
        //    var persons1 = (obj1 as RepeatedField<Person>);
        //    var persons2 = (obj2 as RepeatedField<Person>);

        //    return DiffItems(persons1, persons2, DifferenceIdType.Person);
        //}

        //private IEnumerable<DiffObject> DiffActivities(object obj1, object obj2)
        //{

        //    var activities1 = (obj1 as RepeatedField<Activity>)?.Select(x => x as IActivity<ActivityType>).ToList();
        //    var activities2 = (obj2 as RepeatedField<Activity>)?.Select(x => x as IActivity<ActivityType>).ToList();

        //    var list = new List<DiffObject>
        //    {
        //        base.DiffActivities(activities1, activities2),

        //        this.DiffActivitiesByActivityType(activities1, activities2,  (x) =>!string.IsNullOrWhiteSpace(x.StaffId), (x, y) => x.StaffId == y.StaffId && x.DateD == y.DateD, DifferenceIdType.StaffActivity),
        //        this.DiffActivitiesByActivityType(activities1, activities2,  (x) =>!string.IsNullOrWhiteSpace(x.PersonId), (x, y) => x.PersonId == y.PersonId && x.DateD == y.DateD, DifferenceIdType.PersonActivity),
        //    };

        //    return list;
        //}

        //private DiffObject DiffEmployments(IEnumerable<Staff> staffs1, IEnumerable<Staff> staffs2)
        //{
        //    double standardHoursPerWeek = 40;

        //    var result = new DiffObject();
        //    var sum1 = 0.0;
        //    var sum2 = 0.0;

        //    result.Section = Section.Summary;
        //    result.DifferenceId = DifferenceIdType.Employment;
        //    result.Order = 3;
        //    result.Type = typeof(double);

        //    foreach (var staff in staffs1)
        //    {
        //        foreach (var employment in staff.Employments)
        //        {
        //            sum1 += employment.HoursPerWeek / standardHoursPerWeek;
        //        }
        //    }

        //    foreach (var staff in staffs2)
        //    {
        //        foreach (var employment in staff.Employments)
        //        {
        //            sum2 += employment.HoursPerWeek / standardHoursPerWeek;
        //        }
        //    }

        //    result.Difference = Math.Abs(sum1 - sum2) <= 0 ? Difference.Unchanged : Difference.Difference;
        //    result.Value1 = sum1;
        //    result.Value2 = sum2;

        //    return result;
        //}

    }
}