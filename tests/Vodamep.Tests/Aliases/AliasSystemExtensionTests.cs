using System;
using System.Collections.Generic;
using Vodamep.Hkpv.Model;
using Xunit;

namespace Vodamep.Aliases.Tests
{
    public class AliasSystemExtensionTests
    {
        [Fact]
        public void SetAliases_NameAndBirthdayAreEqual_IdsAreMapped()
        {
            var p1 = new Person
            {
                Id = "1",
                FamilyName = "Musterfrau",
                GivenName = "Anna",
                BirthdayD = new DateTime(2000, 1, 1)
            };

            var p2 = new Person(p1) { Id = "2" };

            var p3 = new Person(p1) { Id = "3" };

            var map = new AliasSystem()
              .SetAliases(new[] { p1, p2, p3 }, GetId, Reduce)
              .BuildMap();

            var expected = new Dictionary<string, string> { { "2", "1" }, { "3", "1" } };

            Assert.Equal(expected, map);
        }

        [Fact]
        public void SetAliases_NameAndBirthdayAreEqualButSetIsNotAliasIsSet_IdsAreNotMapped()
        {
            var p1 = new Person
            {
                Id = "1",
                FamilyName = "Musterfrau",
                GivenName = "Anna",
                BirthdayD = new DateTime(2000, 1, 1)
            };

            var p2 = new Person(p1) { Id = "2" };

            var p3 = new Person(p1) { Id = "3" };

            var map = new AliasSystem()
              .SetNotAlias("1", "2")
              .SetAliases(new[] { p1, p2, p3 }, GetId, Reduce)
              .BuildMap();

            var expected = new Dictionary<string, string> { { "3", "1" } };

            Assert.Equal(expected, map);
        }

        private static string GetId(Person p) => p.Id;

        private static string Reduce(Person p) => $"{p.FamilyName}|{p.GivenName}|{p.BirthdayD:yyyyMMdd}";
    }
}
