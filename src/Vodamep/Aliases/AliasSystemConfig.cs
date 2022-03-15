using System;
using System.Text.RegularExpressions;

namespace Vodamep.Aliases
{
    public class AliasSystemConfig
    {
        /// <summary>
        /// Reihung durch einen <StringComparer>, a wird gegenüber b bevorzugt
        /// </summary>
        public static IdRankingDelegate IdRanking_Default => (id1, id2) => StringComparer.OrdinalIgnoreCase.Compare(id1, id2) > 0 ? (id2, id1) : (id1, id2);

        /// <summary>
        /// Eine Id soll bevorzugt werden, wenn Sie einem bestimmten Muster entspricht
        /// </summary>       
        public static IdRankingDelegate IdRanking_Regex(Regex isPrefered) => (id1, id2) =>
        {
            var isPrefered1 = !string.IsNullOrEmpty(id1) && isPrefered.IsMatch(id1);
            var isPrefered2 = !string.IsNullOrEmpty(id2) && isPrefered.IsMatch(id2);

            if (isPrefered1 == isPrefered2)
            {
                return IdRanking_Default(id1, id2);
            }
            else
            {
                return isPrefered1 ? (id1, id2) : (id2, id1);
            }
        };

        public static AliasSystemConfig Default => new AliasSystemConfig
        {
            IdRanking = IdRanking_Default
        };

        public IdRankingDelegate IdRanking { get; set; }
    }

    public delegate (string Main, string Second) IdRankingDelegate(string id1, string id2);
}
