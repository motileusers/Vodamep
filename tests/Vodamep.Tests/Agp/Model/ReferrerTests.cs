using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.Data.Agp;
using Xunit;

namespace Vodamep.Tests.Agp.Model
{
    public class ReferrerTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            var list1 = new[] {
                Referrer.CaseManagementReferrer,
                Referrer.FamilyDoctorReferrer,
                Referrer.HomeHealthCareReferrer,
                Referrer.LkhRankweilReferrer,
                Referrer.MedicalSpecialistReferrer,
                Referrer.MobileCareServiceReferrer,
                Referrer.OtherReferrer,
                Referrer.RelativesReferrer,
                Referrer.SelfReferrer,
                Referrer.UndefinedReferrer,
            }.Select(x => x.ToString());

            var values = ReferrerProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}