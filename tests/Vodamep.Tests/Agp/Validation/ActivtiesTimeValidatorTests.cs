using System;
using Vodamep.Agp.Model;
using Vodamep.Agp.Validation;
using Xunit;

namespace Vodamep.Tests.Agp.Validation
{
    public class ActivtiesTimeValidatorTests
    {
        private readonly ActivtiesTimeValidator _validator;
        private readonly AgpReport _report;

        public ActivtiesTimeValidatorTests()
        {
            _validator = new ActivtiesTimeValidator();

            _report = new AgpReport
            {
                FromD = new DateTime(2022, 2, 1),
                ToD = new DateTime(2022, 2, 28)
            };
            
            _report.Staffs.Add(new Staff { Id = "a", FamilyName = "staff a" });
        }

        [Fact]
        public void Validate_Two4HourActivitiesOnSameDay_IsValid()
        {
            _report.StaffActivities.Add(new StaffActivity
            {
                ActivityType = StaffActivityType.TravelingSa,
                DateD = new DateTime(2022, 2, 24),
                StaffId = "a",
                Minutes = 4 * 60
            });

            _report.StaffActivities.Add(new StaffActivity
            {
                ActivityType = StaffActivityType.QualificationSa,
                DateD = new DateTime(2022, 2, 24),
                StaffId = "a",
                Minutes = 4 * 60
            });


            var result = _validator.Validate((_report));


            Assert.True(result.IsValid);
        }
    }
}
