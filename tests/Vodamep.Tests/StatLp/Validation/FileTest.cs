using System.IO;
using Vodamep.StatLp.Model;
using Xunit;

namespace Vodamep.Tests.StatLp.Validation
{
    public class FileTest
    {
        [Fact]
        public void TestFile()
        {
            string file = @"D:\0301.json";

            if (File.Exists(file))
            {
                var report = StatLpReport.ReadFile(file);
                var result = report.Validate();
            }
        }
    }
}
