using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Vodamep.Data.Dummy;
using Xunit;

namespace Vodamep.Agp.Model.Tests
{

    public class AGPReportTests
    {

        public AGPReportTests()
        {
            //this.Report = MkkpDataGenerator.Instance.CreateMkkpReport(2021, 1, 1, 1, true);
        }

        protected AgpReport Report { get; }



        [Fact]
        public void WriteThenRead_ReportsAreEqual()
        {
            
            AgpReport report = AgpDataGenerator.Instance.CreateAgpReport(2021, 1, 1, 1, true);

            using (var s = report.WriteToStream())
            {
                var report2 = AgpReport.Read(s);
                Assert.Equal(report, report2);
            }
        }
    }
}
