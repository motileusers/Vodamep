using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.Data.AgpCodeProvider;
using Xunit;

namespace Vodamep.Tests.Agp.Model
{
    public class PlaceOfActionTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            //todo das Miscellaneous fehlt im CSV

            var list1 = new[] {
                PlaceOfAction.Base,
                PlaceOfAction.LkhRankweil,
                PlaceOfAction.MedicalOrination,
                PlaceOfAction.OtherPlace,
                PlaceOfAction.Residence,
              }.Select(x => x.ToString());

            var values = PlaceOfActionProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}