using System;
using System.Linq;

namespace Vodamep.StatLp.Model
{
    public class GroupedStay : IEquatable<GroupedStay>
    {

        public enum SameTypeGroupMode
        {
            Ignore,
            Merge,
            NotAllowed
        }

        public GroupedStay(DateTime from, DateTime? to, Stay[] stays)
        {
            this.From = from;
            this.To = to;
            this.Stays = stays ?? Array.Empty<Stay>();
        }

        public DateTime From { get; }

        public DateTime? To { get; }

        public Stay[] Stays { get; }

        public bool Equals(GroupedStay other) => (From, To) == (other?.From, other?.To) && Stays.SequenceEqual(other?.Stays);

        public override int GetHashCode() => (From, To, Stays).GetHashCode();

        public static bool operator ==(GroupedStay left, GroupedStay right) => Equals(left, right);

        public static bool operator !=(GroupedStay left, GroupedStay right) => !Equals(left, right);

        public override bool Equals(object obj) => obj is GroupedStay gs && Equals(gs);
    }
}