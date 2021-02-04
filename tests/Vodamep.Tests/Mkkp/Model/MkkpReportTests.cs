using System;
using System.Linq;
using Vodamep.Data.Dummy;
using Xunit;

namespace Vodamep.Mkkp.Model.Tests
{

    public class MkkpReportTests
    {

        public MkkpReportTests()
        {
            //this.Report = MkkpDataGenerator.Instance.CreateMkkpReport(2021, 1, 1, 1, true);
        }

        protected MkkpReport Report { get; }



        [Fact]
        public void WriteThenRead_ReportsAreEqual()
        {
            MkkpReport report = MkkpDataGenerator.Instance.CreateMkkpReport(2021, 1, 1, 1, true);

            using (var s = report.WriteToStream())
            {
                var report2 = MkkpReport.Read(s);

                Assert.Equal(report, report2);
            }
        }
    }
}
