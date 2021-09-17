using System.Linq;
using Vodamep.Mkkp.Model;
using Vodamep.Data.Mkkp;
using Xunit;

namespace Vodamep.Tests.Mkkp.Model
{
    public class PlaceOfActionTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            var list1 = new[] {
                PlaceOfAction.KindergartenPlace,
                PlaceOfAction.LocalDoctorPlace,
                PlaceOfAction.OtherPlace,
                PlaceOfAction.OutpatientHospitalPlace,
                PlaceOfAction.ResidencePlace,
                PlaceOfAction.SchoolPlace,
                PlaceOfAction.UndefinedPlace,
              }.Select(x => x.ToString());

            var values = PlaceOfActionProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}