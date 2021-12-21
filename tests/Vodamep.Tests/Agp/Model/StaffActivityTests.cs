using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Agp.Model;
using Vodamep.Data.Agp;
using Xunit;

namespace Vodamep.Tests.Agp.Model
{
    public class StaffActivityTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            var list1 = new[] {
                StaffActivityType.NetworkingSa,
                StaffActivityType.OrganizationSa,
                StaffActivityType.QualificationSa,
                StaffActivityType.TravelingSa,
                StaffActivityType.UndefinedSa,
            }.Select(x => x.ToString());

            var values = StaffActivityTypeProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}
