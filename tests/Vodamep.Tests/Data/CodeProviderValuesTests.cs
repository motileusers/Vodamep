using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class CodeProviderValuesTests
    {
        [Fact]
        public void ListValues()
        {
            var a = CommuneProvider.Instance.Values;

            CodeProviderValueEnumerator values = new CodeProviderValueEnumerator();
            values.Enumerate();

        }
    }
}
