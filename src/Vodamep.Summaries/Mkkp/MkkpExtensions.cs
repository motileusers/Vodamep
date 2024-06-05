using Vodamep.Data;
using Vodamep.Data.Mkkp;
using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    //todo: Überlegen wohin. Diese Extensions könnten auch in die Vodamep-Assembly. Vielleicht sind sie bereis dort?
    public static class MkkpExtensions
    {
        public static string Localize(this Gender gender) => gender switch
        {
            Gender.FemaleGe => "w",
            Gender.MaleGe => "m",
            Gender.OpenGe => "o",
            Gender.DiversGe => "div",
            Gender.InterGe => "inter",
            _ => string.Empty
        };

        //todo: Bei Vodamep nachfragen: ReferrerProvider.Instance hat einen string als key, warum nicht den int-value des enum? wird das so irgendwo verwendet?
        public static string Localize(this Referrer referrer)
        {
            return ReferrerProvider.Instance.Values.Where(x => $"{referrer}".Equals(x.Key.Replace("_", ""), StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault() ?? string.Empty;
        }

        public static string Localize(this CareAllowance ca)
        {
            return CareAllowanceProvider.Instance.Values.Where(x => $"{ca}".Equals(x.Key.Replace("_", ""), StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault() ?? string.Empty;
        }

        public static string Localize(this DiagnosisGroup diagnosisGroup)
        {
            return DiagnosisgroupProvider.Instance.Values.Where(x => $"{diagnosisGroup}".Equals(x.Key.Replace("_", ""), StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).FirstOrDefault() ?? string.Empty;
        }

        public static string Localize(this IEnumerable<DiagnosisGroup> diagnosisGroups) => string.Join(", ", diagnosisGroups.Select(x => x.Localize()));


        public static MkkpReport Merge(this MkkpReport[] reports)
        {
            if (reports == null || reports.Length == 0)
            {
                return new MkkpReport();
            }

            if (reports.Length == 1)
            {
                return reports[0];
            }

            var ordered = reports.OrderByDescending(x => x.From);

            var result = ordered.First().Clone();

            foreach (var report in ordered.Skip(1))
            {
                if (report.ToD.Date.AddDays(1) != result.FromD.Date)
                {
                    throw new Exception("Only reports with consecutively periods are allowed!");
                }


                result.From = report.From;
                foreach (var person in report.Persons)
                {
                    if (!result.Persons.Where(x => x.Id == person.Id).Any())
                    {
                        result.Persons.Add(person);
                    }
                }

                foreach (var staff in report.Staffs)
                {
                    if (!result.Staffs.Where(x => x.Id == staff.Id).Any())
                    {
                        result.Staffs.Add(staff);
                    }
                }

                result.Activities.AddRange(report.Activities);
                result.TravelTimes.AddRange(report.TravelTimes);
            }

            return result;
        }



    }
}
