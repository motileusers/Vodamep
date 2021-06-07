using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class SumOfActivtiesMinutesPerStaffMustBeLowerThan10HoursValidator : AbstractValidator<MkkpReport>
    {
        //10 hours max
        private const int maxNoOfMinutes = 12 * 60;

        public SumOfActivtiesMinutesPerStaffMustBeLowerThan10HoursValidator()
        {
            this.RuleFor(x => new Tuple<IList<Staff>, IEnumerable<Activity>>(x.Staffs, x.Activities))
                .Custom((data, ctx) =>
                {
                    var staffs = data.Item1;
                    var activities = data.Item2;

                    var activitiesByStaffId = activities.GroupBy(y => y.StaffId)
                        .Select((group) => new { Key = group.Key, Items = group.ToList() });

                    foreach (var activityByStaffId in activitiesByStaffId)
                    {
                        var sumOfMinutes = activityByStaffId.Items.Sum(x => x.Minutes);

                        if (sumOfMinutes > maxNoOfMinutes)
                        {
                            var staff = staffs.Where(x => x.Id == activityByStaffId.Key).FirstOrDefault();

                            ctx.AddFailure(new ValidationFailure(nameof(MkkpReport.Activities), $"{staff?.FamilyName} {staff?.GivenName}: {Validationmessages.MaxSumOfMinutesPerStaffMemberIs12Hours}"));
                        }
                    }

                });
        }
    }

}
