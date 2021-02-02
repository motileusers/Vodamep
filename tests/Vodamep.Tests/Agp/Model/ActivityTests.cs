using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.Data.AgpCodeProvider;
using Xunit;

namespace Vodamep.Tests.Agp.Model
{
    public class ActivityTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            //todo das Miscellaneous fehlt im CSV

            var list1 = new[] {
                ActivityType.CareDocumentation,
                ActivityType.Clearing,
                ActivityType.ContactPartner,
                ActivityType.ExecutionTransport,
                ActivityType.GeriatricPsychiatric,
                ActivityType.GuidanceClient,
                ActivityType.GuidancePartner,
                //ActivityType.Miscellaneous,
                ActivityType.ObservationsAssessment
            }.Select(x => x.ToString());

            var values = ActivityTypeProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}
