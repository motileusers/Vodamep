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
    internal class ActivtiesTimeValidator : AbstractValidator<AgpReport>
    {
        public ActivtiesTimeValidator()
        {

            // Max. 12 Std. pro Mitarbeiter pro Tag

            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                int maxNoOfMinutes = 12 * 60;

                Dictionary<string, StaffActivity> timeDictionary = new Dictionary<string, StaffActivity>();

                foreach (StaffActivity activity in x.StaffActivities)
                {
                    string key = activity.StaffId + activity.DateD.ToShortDateString();
                    if (!timeDictionary.ContainsKey(key))
                    {
                        timeDictionary.Add(key, activity);
                    }
                    else
                    {
                        timeDictionary[key].Minutes += activity.Minutes;
                    }
                }


                foreach (Activity activity in x.Activities)
                {
                    string key = activity.StaffId + activity.DateD.ToShortDateString();
                    if (!timeDictionary.ContainsKey(key))
                    {
                        timeDictionary.Add(key, new StaffActivity() { DateD = activity.DateD, StaffId = activity.StaffId, Minutes = activity.Minutes });
                    }
                    else
                    {
                        timeDictionary[key].Minutes += activity.Minutes;
                    }
                }

                foreach (KeyValuePair<string, StaffActivity> keyValue in timeDictionary)
                {
                    if (keyValue.Value.Minutes > maxNoOfMinutes)
                    {
                        string name = x.GetStaffName(keyValue.Value.StaffId);
                        ctx.AddFailure(new ValidationFailure(nameof(AgpReport.Activities), Validationmessages.ReportBaseMaxSumOfMinutesPerStaffMemberIs12Hours(keyValue.Value.DateD.ToShortDateString(), name)));
                    }
                }

            });




            // Max. 5 Std. pro Mitarbeiter pro Tag Wegzeiten

            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                int maxNoOfMinutes = 5 * 60;

                Dictionary<string, StaffActivity> timeDictionary = new Dictionary<string, StaffActivity>();

                foreach (StaffActivity activity in x.StaffActivities)
                {
                    if (activity.ActivityType == StaffActivityType.TravelingSa)
                    {
                        string key = activity.StaffId + activity.DateD.ToShortDateString();
                        if (!timeDictionary.ContainsKey(key))
                        {
                            timeDictionary.Add(key, activity);
                        }
                        else
                        {
                            timeDictionary[key].Minutes += activity.Minutes;
                        }
                    }
                }


                foreach (KeyValuePair<string, StaffActivity> keyValue in timeDictionary)
                {
                    if (keyValue.Value.Minutes > maxNoOfMinutes)
                    {
                        string name = x.GetStaffName(keyValue.Value.StaffId);
                        ctx.AddFailure(new ValidationFailure(nameof(AgpReport.Activities), Validationmessages.MaxSumOfMinutesTravelTimesIs5Hours(name, keyValue.Value.DateD.ToShortDateString())));
                    }
                }

            });

        }
    }

}
