using Vodamep.Mkkp.Model;

namespace Vodamep.Summaries.Mkkp
{
    //todo: Überlegen wohin. Diese Extensions könnten auch in die Vodamep-Assembly. Vielleicht sind sie bereis dort?
    public static class MkkpExtensions
    {
        public static string Format(this Gender gender) => gender switch
        {
            Gender.FemaleGe => "w",
            Gender.MaleGe => "m",
            Gender.OpenGe => "o",
            Gender.DiversGe => "div",
            Gender.InterGe => "inter",
            _ => string.Empty
        };


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
