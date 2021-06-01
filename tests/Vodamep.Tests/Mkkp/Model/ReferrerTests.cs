using System.Linq;
using Vodamep.Mkkp.Model;
using Vodamep.Data.Mkkp;
using Xunit;

namespace Vodamep.Tests.Mkkp.Model
{
    public class ReferrerTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            var list1 = new[] {
                Referrer.KhDornbirnReferrer,
                Referrer.LkhBregenzReferrer,
                Referrer.LkhFeldkirchReferrer,
                Referrer.OtherReferrer,
                Referrer.PrivateSpecialistsReferrer,
                Referrer.UndefinedReferrer,
            }.Select(x => x.ToString());

            var values = ReferrerProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}