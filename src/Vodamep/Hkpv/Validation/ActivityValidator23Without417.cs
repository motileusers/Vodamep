using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{

    internal class ActivityValidator23Without417 : AbstractValidator<Activity>
    {
        public ActivityValidator23Without417(IEnumerable<Person> persons, IEnumerable<Staff> staffs)
        {
            RuleFor(x => x.Entries)
                .Custom((list, ctx) =>
                {
                    if (!list?.Any() ?? false)
                        return;

                   var a =  ctx.ParentContext.InstanceToValidate;

                    var l = list;


                    var activtiy = ctx.ParentContext.InstanceToValidate as Activity;
                    string person = null;

                    if (!string.IsNullOrWhiteSpace(activtiy.PersonId))
                    {
                        var p = persons.FirstOrDefault(x => x.Id == activtiy.PersonId);
                        if (p != null)
                            person = $"{p.GivenName} {p.FamilyName}";
                    }

                    if (!string.IsNullOrWhiteSpace(activtiy.StaffId))
                    {
                        var s = staffs.FirstOrDefault(x => x.Id == activtiy.StaffId);
                        if (s != null)
                            person = $"{s.GivenName} {s.FamilyName}";
                    }

                    var entries23 = l.Where(x => x == ActivityType.Lv02 || x == ActivityType.Lv03).Any();
                    
                    var entries4 = l.Where(x => ((int)x > 3)).Any();
                    
                    if (entries23 && !entries4)
                    {
                        ctx.AddFailure(new ValidationFailure($"{nameof(Activity.Entries)}", Validationmessages.WithoutEntry("4-17", person))
                        {
                            Severity = Severity.Warning
                        });
                    }


                });
        }
    }
}
