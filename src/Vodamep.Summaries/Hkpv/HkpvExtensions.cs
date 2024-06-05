using Vodamep.Hkpv.Model;

namespace Vodamep.Summaries.Hkpv
{
    //todo: Überlegen wohin. Diese Extensions könnten auch in die Vodamep-Assembly. Vielleicht sind sie bereis dort?
    public static class HkpvExtensions
    {
        public static string Format(this Gender gender) => gender switch
        {
            Gender.Female => "w",
            Gender.Male => "m",
            Gender.Open => "o",
            Gender.Divers => "div",
            Gender.Inter => "inter",
            _ => string.Empty
        };
    }
}
