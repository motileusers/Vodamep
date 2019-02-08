using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class QualificationCodeProviderTests
    {
        [Fact]
        public void Trainee_IsValidCode()
        {
            Assert.True(QualificationCodeProvider.Instance.IsValid(QualificationCodeProvider.Instance.Trainee));
        }

        [Fact]
        public void Trainee_DescriptionIsAuszubildend()
        {
            string.Equals("Auszubildend", QualificationCodeProvider.Instance.Values[QualificationCodeProvider.Instance.Trainee]);
        }
    }
}
