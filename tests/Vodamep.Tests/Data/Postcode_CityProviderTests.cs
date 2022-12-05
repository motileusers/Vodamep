using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class Postcode_CityProviderTests
    {
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
