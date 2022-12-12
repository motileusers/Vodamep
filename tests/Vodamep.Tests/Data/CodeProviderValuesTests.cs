using Vodamep.Data;
using Xunit;

namespace Vodamep.Tests.Data
{
    public class CodeProviderValuesTests
    {
        [Fact]
        public void ListValues()
        {
            CodeProviderValueEnumerator values = new CodeProviderValueEnumerator();
            values.Enumerate();

        }
    }
}
