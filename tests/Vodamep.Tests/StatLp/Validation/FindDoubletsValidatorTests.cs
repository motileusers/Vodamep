using System;
using Vodamep.StatLp.Model;
using Xunit;

namespace Vodamep.StatLp.Validation.Tests
{
    public class FindDoubletsValidatorTests
    {
        [Fact]
        public void Validate_StatLpReportWithDoublets_ReturnsErrorResult()
        {
            var validator = new FindDoubletsValidator();

            var report = new StatLpReport();

            for (var i = 0; i < 2; i++)
            {
                report.Persons.Add(new Person
                {
                    Id = $"{i}",
                    FamilyName = "Test",
                    GivenName = "A",
                    BirthdayD = new DateTime(2000, 1, 1)
                });

                report.Stays.Add(new Stay
                {
                    PersonId = $"{i}",
                    FromD = new DateTime(2022, 5, 5),
                    Type = AdmissionType.ContinuousAt
                });
            }


            var vr = validator.Validate(report);

            //'Test A' wurde mehrfach gemeldet."
            Assert.False(vr.IsValid);
          
        }
    }
}
