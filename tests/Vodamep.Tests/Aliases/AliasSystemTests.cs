using System;
using System.Collections.Generic;
using Xunit;

namespace Vodamep.Aliases.Tests
{
    public class AliasSystemTests
    {
        [Fact]
        public void SetAlias_SomePermutations_ReturnsSameMap()
        {

            var permutations = new[]
            {
                new AliasSystem().SetAliases(new[] { ("1", "2"), ("2", "3"), ("1", "3") } ).BuildMap(),
                new AliasSystem().SetAliases(new[] { ("1", "2"), ("1", "3"), ("2", "3") } ).BuildMap(),
                new AliasSystem().SetAliases(new[] { ("1", "3"), ("1", "2"), ("2", "3") } ).BuildMap(),

                new AliasSystem().SetAliases(new[] { ("2", "1"), ("2", "3"), ("1", "3") } ).BuildMap(),
                new AliasSystem().SetAliases(new[] { ("1", "2"), ("3", "1"), ("2", "3") } ).BuildMap(),
                new AliasSystem().SetAliases(new[] { ("1", "3"), ("1", "2"), ("3", "2") } ).BuildMap(),

                new AliasSystem().SetAliases(new[] { ("3", "2"), ("1", "3"), ("1", "2") } ).BuildMap(),

                new AliasSystem().SetAliases(new[] { ("1", "3"), ("3", "2") } ).BuildMap(),
                new AliasSystem().SetAliases(new[] { ("3", "2"), ("1", "3") } ).BuildMap(),
                new AliasSystem().SetAliases(new[] { ("1", "3"), ("1", "2") } ).BuildMap(),
                new AliasSystem().SetAliases(new[] { ("1", "2"), ("3", "2") } ).BuildMap()
            };

            var expected = new Dictionary<string, string> { { "2", "1" }, { "3", "1" } };

            foreach (var entry in permutations)
            {
                Assert.Equal(expected, entry);
            }
        }

        [Fact]
        public void CreateOrderedTuple_Regex()
        {
            var config = new AliasSystemConfig
            {
                //ids, die mit x beginnen sollen bevorzugt werden
                IdRanking = AliasSystemConfig.IdRanking_Regex(new System.Text.RegularExpressions.Regex(@"^x"))
            };

            var map = new AliasSystem(config)
                .SetAlias("1", "x1")
                .SetAlias("x2", "2")
                .BuildMap();

            var expected = new Dictionary<string, string> { { "1", "x1" }, { "2", "x2" } };

            Assert.Equal(expected, map);
        }

        [Fact]
        public void SetAlias_SetNotAliasIsAlreadySet_ThrowsException()
        {
            var system = new AliasSystem()
                 .SetNotAliases(new[] { ("1", "2") });

            var e = Assert.Throws<Exception>(() => system.SetAliases(new[] { ("1", "2") }));
        }

        [Fact]
        public void SetAlias_SetNotAliasIsAlreadySet2_ThrowsException()
        {
            var system = new AliasSystem()
                 .SetNotAliases(new[] { ("1", "2") })
                 .SetAliases(new[] { ("1", "3") });

            Assert.Throws<Exception>(() => system.SetAliases(new[] { ("2", "3") }));
        }

        [Fact]
        public void SetNotAlias_SetAliasIsAlradySet_ThrowsException()
        {
            var system = new AliasSystem()
                 .SetAliases(new[] { ("1", "2") });

            var e = Assert.Throws<Exception>(() => system.SetNotAliases(new[] { ("1", "2") }));
        }

        [Fact]
        public void SetNotAlias_SetAliasIsAlreadySet2_ThrowsException()
        {
            var system = new AliasSystem()
                 .SetAliases(new[] { ("1", "3"), ("2", "3") });

            Assert.Throws<Exception>(() => system.SetNotAliases(new[] { ("1", "2") }));
        }

        
    }
}
