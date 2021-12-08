using System;


namespace Vodamep.StatLp.Model
{
    public class GroupedStay
    {

        public enum SameTypeyGroupMode
        {
            Ignore,
            Merge,
            NotAllowed
        }

        public GroupedStay(DateTime from, DateTime to, Stay[] stays)
        {
            this.From = from;
            this.To = to;
            this.Stays = stays;
        }

        public DateTime From { get; }

        public DateTime To { get; }

        public Stay[] Stays { get; }

    }
}