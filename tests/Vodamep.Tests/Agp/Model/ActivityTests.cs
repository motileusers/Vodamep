using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.Data.Agp;
using Xunit;

namespace Vodamep.Tests.Agp.Model
{
    public class ActivityTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            var list1 = new[] {
                ActivityType.CareDocumentationAt,
                ActivityType.ClearingAt,
                ActivityType.ContactPartnerAt,
                ActivityType.ExecutionTransportAt,
                ActivityType.GeriatricPsychiatricAt,
                ActivityType.GuidanceClientAt,
                ActivityType.GuidancePartnerAt,
                ActivityType.ObservationsAssessmentAt,
                ActivityType.UndefinedAt,
            }.Select(x => x.ToString());

            var values = ActivityTypeProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}
