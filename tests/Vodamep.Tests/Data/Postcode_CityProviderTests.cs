using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class Postcode_CityProviderTests
    {
        [Theory]
        [InlineData("abc", "", 0, false)]
        [InlineData("80404", "Feldkirch", 13, true)]
        [InlineData("80101", "Bartholomäberg", 1, true)]
        public void IsValidCommune(string code, string name, int count, bool expected)
        {
            var p = Vodamep.Data.CommuneProvider.Instance;

            Assert.Equal(expected, p.IsValid(code));

            if (expected)
            {
                var c = Vodamep.Data.CommuneProvider.Instance.Values[code];

                Assert.Equal(c.Name, name);
                Assert.Equal(c.PostcodeCities.Count, count);
            }

        }


        [Theory]
        [InlineData("6900 Bregenz", true)]
        [InlineData("Bregenz", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void IsValidRecord(string code, bool expected)
        {
            var p = Vodamep.Data.PostcodeCityProvider.Instance;

            Assert.Equal(expected, p.IsValid(code));
        }

        [Theory]
        [InlineData("6900 Bregenz", true)]
        [InlineData("Bregenz", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void IsValidRecordHkpv(string code, bool expected)
        {
            var p = Vodamep.Data.PostcodeCityProvider.Instance;

            Assert.Equal(expected, p.IsValid(code));
        }
    }
}
