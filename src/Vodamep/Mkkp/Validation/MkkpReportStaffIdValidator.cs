using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class MkkpReportStaffIdValidator : AbstractValidator<MkkpReport>
    {
        public MkkpReportStaffIdValidator()
        {
            MkkpDisplayNameResolver displayNameResolver = new MkkpDisplayNameResolver();

            this.RuleFor(x => x.Staffs)
                .Custom((list, ctx) =>
                {
                    foreach (var id in list.Select(x => x.Id).OrderBy(x => x).GroupBy(x => x).Where(x => x.Count() > 1))
                    {
                        var item = list.Where(x => x.Id == id.Key).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(MkkpReport.Staffs)}[{index}]", Validationmessages.IdIsNotUnique));
                    }
                });

            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: new { x.Staffs, x.Activities, x.Consultations }
            this.RuleFor(x => new Tuple<IList<Staff>, IEnumerable<Activity>>(x.Staffs, x.Activities))
                .Custom((a, ctx) =>
                {
                    var staffs = a.Item1;
                    var activities = a.Item2;


                    var idStaffs = staffs.Select(x => x.Id).Distinct().ToArray();
                    var idActivities = (
                        activities.Select(x => x.StaffId)
                    ).Distinct().ToArray();

                    foreach (var id in idStaffs.Except(idActivities))
                    {
                        var item = staffs.Where(x => x.Id == id).First();
                        var index = staffs.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure(nameof(Staff), Validationmessages.ReportBaseWithoutActivity(displayNameResolver.GetDisplayName(nameof(Staff)), item.GetDisplayName())));

                    }

                    foreach (var id in idActivities.Except(idStaffs))
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(MkkpReport.Staffs), Validationmessages.IdIsMissing(id)));
                    }

                    foreach (var activity in activities)
                    {
                        if (!idStaffs.Contains(activity.StaffId))
                            ctx.AddFailure(new ValidationFailure(nameof(Activity), Validationmessages.ReportBaseActivitWithoutStaff(activity.Id, activity.StaffId)));
                    }
                });

        }
    }
}