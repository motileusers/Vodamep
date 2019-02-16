using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Data;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class EmploymentActivityValidator : AbstractValidator<HkpvReport>
    {

        public EmploymentActivityValidator()
            : base()
        {
            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später:  new  { x.Activities, x.Staffs }
            this.RuleFor(x => new Tuple<IList<Activity>, IEnumerable<Staff>>(x.Activities, x.Staffs))
                .Custom((a, ctx) =>
               {
                   var activities = a.Item1;
                   var staffs = a.Item2;

                   // Dictionary für schnellen Zugriff
                   Dictionary<string, Staff> staffsDict = staffs.GroupBy(p => p.Id)
                                                                .ToDictionary(g => g.Key, g => g.First());

                   foreach (Activity activity in activities)
                   {
                       if (staffsDict.ContainsKey(activity.StaffId))
                       {
                           bool insideRange = false;
                           bool skipCheck = false;

                           Staff staff = staffsDict[activity.StaffId];
                           foreach (Employment employment in staff.Employments)
                           {
                               if (employment.FromD == null ||
                                    employment.ToD == null ||
                                    activity.DateD == DateTime.MinValue)
                               {
                                   skipCheck = true;
                                   break;
                               }
                               else
                               {
                                   if (activity.DateD >= employment.FromD &&
                                       activity.DateD <= employment.ToD)
                                   {
                                       insideRange = true;
                                       break;
                                   }
                               }
                           }

                           if (!insideRange && !skipCheck)
                           {
                               var index = activities.IndexOf(activity);
                               ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.InvalidEmploymentForActivity(activity, staff)));
                           }
                       }
                   }

               });
        }
    }
}
