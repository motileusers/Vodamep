using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.Data.AgpCodeProvider;
using Xunit;

namespace Vodamep.Tests.Agp.Model
{
    public class DiagnosisgroupTests
    {
        //todo missing undefineddiagnosisgroup

        [Fact]
        public void DiagnosisGroups_ReturnspectedResult()
        {
            var list1 = new[] {
                DiagnosisGroup.AffectiveDisorder,
                DiagnosisGroup.AnxietyAdjustmentObsessiveDisorder,
                DiagnosisGroup.DementiaDisease,
                DiagnosisGroup.DependenceIllnesse,
                DiagnosisGroup.NonOrganicSleepingDisorder,
                DiagnosisGroup.SchizophreniaDelusionalDisorder,
                //DiagnosisGroup.UndefinedDiagnosisGroup,
            }.Select(x => x.ToString());

            var values = DiagnosisgroupProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}