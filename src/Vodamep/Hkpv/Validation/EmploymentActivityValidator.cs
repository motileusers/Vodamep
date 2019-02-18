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
            this.RuleFor(x => x)
                .Custom((x, ctx) =>
               {
                   HkpvReport report = ctx.ParentContext.InstanceToValidate as HkpvReport;

                   var activities = report.Activities;
                   var staffs = report.Staffs;
                   DateTime from = report.FromD;
                   DateTime to = report.ToD;


                   // Dictionary für schnellen Zugriff
                   Dictionary<string, Staff> staffsDict = staffs.GroupBy(p => p.Id)
                                                                .ToDictionary(g => g.Key, g => g.First());

                   foreach (Activity activity in activities)
                   {
                       if (staffsDict.ContainsKey(activity.StaffId))
                       {
                           bool insideRange = false;
                           bool skipCheck = false;

                           Staff staffDict = staffsDict[activity.StaffId];
                           EmploymentValidator employmentVal = new EmploymentValidator(from, to, staffDict);
                           ActivityValidator activityVal = new ActivityValidator(from, to);

                           foreach (Employment employment in staffDict.Employments)
                           {
                               ValidationResult employmentResult = employmentVal.Validate(employment);
                               ValidationResult activityResult = activityVal.Validate(activity);

                               if (employmentResult.IsValid &&
                                   activityResult.IsValid)
                               {
                                   if (activity.DateD >= employment.FromD &&
                                       activity.DateD <= employment.ToD)
                                   {
                                       insideRange = true;
                                       break;
                                   }
                               }
                               else
                               {
                                   skipCheck = true;
                                   break;
                               }
                           }

                           if (!insideRange && !skipCheck)
                           {
                               var index = activities.IndexOf(activity);
                               ctx.AddFailure(new ValidationFailure($"{nameof(HkpvReport.Activities)}[{index}]", Validationmessages.InvalidEmploymentForActivity(activity, staffDict)));
                           }
                       }
                   }

               });
        }
    }
}
