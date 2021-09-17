using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class SumOfActivtiesMinutesPerStaffMustBeLowerThan10HoursValidator : AbstractValidator<AgpReport>
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
                        var activitiesPeStaffIdAndDate = activityByStaffId.Items.GroupBy(z => z.DateD)
                            .Select(group => new { Date = group.Key, Items = group.ToList() });

                        foreach (var ac in activitiesPeStaffIdAndDate)
                        {
                            var sumOfMinutes = ac.Items.Sum(x => x.Minutes);

                            if (sumOfMinutes > maxNoOfMinutes)
                            {
                                var staff = staffs.FirstOrDefault(x => x.Id == activityByStaffId.Key);

                                ctx.AddFailure(new ValidationFailure(nameof(AgpReport.Activities), $"{staff?.FamilyName} {staff?.GivenName}: {Validationmessages.ReportBaseMaxSumOfMinutesPerStaffMemberIs12Hours(ac.Date.ToShortDateString())}"));
                            }
                        }
                    }

                });
        }
    }

}
