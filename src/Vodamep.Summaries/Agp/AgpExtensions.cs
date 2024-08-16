using Vodamep.Agp.Model;
using Vodamep.Data;
using Vodamep.Data.Agp;

namespace Vodamep.Summaries.Agp
{
    //todo: Überlegen wohin. Diese Extensions könnten auch in die Vodamep-Assembly. Vielleicht sind sie bereis dort?
    public static class AgpExtensions
    {
        public static string Format(this Gender gender) => gender switch
        {
            Gender.FemaleGe => "w",
            Gender.MaleGe => "m",
            Gender.OpenGe => "o",
            Gender.DiversGe => "div",
            Gender.InterGe => "inter",
            _ => gender.ToString()
        };

        public static string Format(this PlaceOfAction pa) => PlaceOfActionProvider.Instance.GetEnumValue($"{pa}");

        public static string Format(this ActivityType at) => ActivityTypeProvider.Instance.GetEnumValue($"{at}");

        public static string Format(this StaffActivityType sat) => StaffActivityTypeProvider.Instance.GetEnumValue($"{sat}");

        public static string Format(this DiagnosisGroup dg) => DiagnosisGroupProvider.Instance.GetEnumValue($"{dg}");

        public static string Format(this Referrer r) => ReferrerProvider.Instance.GetEnumValue($"{r}");

        public static string Format(this CareAllowance r) => CareAllowanceProvider.Instance.GetEnumValue($"{r}");


    }
}
