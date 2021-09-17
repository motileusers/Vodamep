using System;
using System.Collections.Generic;
using System.Linq;
using Vodamep.Mkkp.Model;
using Vodamep.Data.Mkkp;
using Xunit;

namespace Vodamep.Tests.Mkkp.Model
{
    public class ActivityTests
    {
        [Fact]
        public void AsSorted_ReturnspectedResult()
        {
            var list1 = new[] {
                ActivityType.AccompanyingWithContact,
                ActivityType.AccompanyingWithoutContact,
                ActivityType.Body,
                ActivityType.MedicalCommon,
                ActivityType.MedicalDiet,
                ActivityType.MedicalInjection,
                ActivityType.MedicalVentilationTubes,
                ActivityType.MedicalVital,
                ActivityType.MedicalWound,
                ActivityType.UndefinedActivity,
            }.Select(x => x.ToString());

            var values = ActivityTypeProvider.Instance.Values.Select(x => x.Key);

            Assert.Equal(list1, values);
        }
    }
}
