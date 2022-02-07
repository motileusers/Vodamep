using System;
using System.Linq;
using Vodamep.StatLp.Model;
using Xunit;

namespace Vodamep.StatLp.Validation.Adjacent.Tests
{
    public class StatLpAdjacentReportsStaysValidatorTests
    {
        private readonly StatLpAdjacentReportsStaysValidator _validator;
        private readonly StatLpReport _r1;
        private readonly StatLpReport _r2;

        public StatLpAdjacentReportsStaysValidatorTests()
        {
            _validator = new StatLpAdjacentReportsStaysValidator();

            _r1 = new StatLpReport
            {
                FromD = new DateTime(2021, 1, 1),
                ToD = new DateTime(2021, 12, 31)
            };
            _r1.Persons.Add(new Person { Id = "1" });

            _r1.Stays.Add(new Stay
            {
                FromD = new DateTime(2021, 9, 10),
                Type = AdmissionType.ContinuousAt
            });

            _r2 = new StatLpReport(_r1)
            {
                FromD = _r1.FromD.AddYears(1),
                ToD = _r1.ToD.AddYears(1)
            };
        }


        [Fact]
        public void Validate_AnOngoingStay_StayIsPartOfBothReports_IsValid()
        {
            var result = _validator.Validate((_r1, _r2));

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_AnOngoingStayEndsInLaterReport_ToIsOnlySetInLaterReport_IsValid()
        {
            _r2.Stays.Last().ToD = _r2.FromD.AddDays(30);

            var result = _validator.Validate((_r1, _r2));

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_AnOngoingStayEndsInLaterReport_ToIsOnlySetInBothsReports_IsValid()
        {
            _r2.Stays.Last().ToD = _r2.FromD.AddDays(30);
            _r1.Stays.Last().ToD = _r2.Stays.Last().ToD;

            var result = _validator.Validate((_r1, _r2));

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_AnOngoingStayEndsInLaterReport_ToIsOnlySetInBothsReportsWithDifferntValue_IsNotValid()
        {
            _r2.Stays.Last().ToD = _r2.FromD.AddDays(30);
            _r1.Stays.Last().ToD = _r2.Stays.Last().ToD.Value.AddDays(1);

            var result = _validator.Validate((_r1, _r2));

            Assert.False(result.IsValid);
        }


        [Fact]
        public void Validate_AStayWithGapToOngoingStay_ThisOldStayIsNotPartOfTheLaterReport_IsValid()
        {
            _r1.Stays.Insert(0, new Stay
            {
                FromD = new DateTime(2021, 3, 26),
                ToD = new DateTime(2021, 4, 7),
                Type = AdmissionType.TrialAt
            });

            var result = _validator.Validate((_r1, _r2));

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_OrderOfStaysInNotAscendingDates_IsNotValid()
        {
            _r1.Stays.Add(new Stay
            {
                FromD = new DateTime(2021, 3, 26),
                ToD = new DateTime(2021, 4, 7),
                Type = AdmissionType.TrialAt
            });

            var result = _validator.Validate((_r1, _r2));

            Assert.False(result.IsValid);
        }


        [Fact]
        public void Validate_AHolidayStayEndsAtReportToDateAndIsFollowedByAnotherStay_IsValid()
        {
            _r1.Stays.Clear();
            _r2.Stays.Clear();


            _r1.Stays.Add(new Stay { PersonId = "1", FromD = new DateTime(2021, 11, 29), ToD = new DateTime(2021, 12, 31), Type = AdmissionType.HolidayAt });

            _r2.Stays.Add(new Stay { PersonId = "1", FromD = new DateTime(2021, 11, 29), ToD = new DateTime(2021, 12, 31), Type = AdmissionType.HolidayAt });
            _r2.Stays.Add(new Stay { PersonId = "1", FromD = new DateTime(2022, 1, 1), ToD = null, Type = AdmissionType.ContinuousAt });


            var result = _validator.Validate((_r1, _r2));

            Assert.True(result.IsValid);
        }
    }
}
