using System.Collections.Generic;
using System.Linq;

namespace Vodamep.Agp.Model
{
    public static class ActivitiesExtensions
    {
        public static IEnumerable<Activity> AsSorted(this IEnumerable<Activity> activities)
        {
            var entries = new List<Activity>();
            // zuerst die Reihenfolge der Activity.Entries herstellen
            foreach (var a in activities)
            {
                var e = new Activity()
                {
                    PlaceOfAction = a.PlaceOfAction,
                    Minutes = a.Minutes,
                    Id = a.Id,
                    Date = a.Date, 
                    PersonId = a.PersonId, 
                    StaffId = a.StaffId
                };
                e.Entries.AddRange(a.Entries.OrderBy(x => x));

                entries.Add(e);
            }

            return entries.OrderBy(x => x.StaffId).ThenBy(x => x.PersonId).ThenBy(x => x.Date).ThenBy(x => x.PlaceOfAction).ThenBy(x => x.Minutes);
        }

        public static IEnumerable<StaffActivity> AsSorted(this IEnumerable<StaffActivity> activities)
        {
            var entries = new List<StaffActivity>();
            foreach (var a in activities)
            {
                var e = new StaffActivity()
                {
                    Minutes = a.Minutes,
                    Id = a.Id,
                    ActivityType = a.ActivityType,
                    Date = a.Date,
                    StaffId = a.StaffId
                };

                entries.Add(e);
            }

            // jetzt die Einträge selbst sortieren
            return entries.OrderBy(x => x.StaffId).ThenBy(x => x.Date).ThenBy(x => x.ActivityType).ThenBy(x => x.Minutes);
        }

    }
}