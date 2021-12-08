using System;
using System.Linq;
using Vodamep.Data.Dummy;
using Vodamep.StatLp.Model;
using Xunit;

namespace Vodamep.Tests.StatLp
{
    public class StatLpReportExtensionsTests
    {
        protected StatLpReport Report { get; }
        protected string PersonId => this.Report.Persons[0].Id;

        public StatLpReportExtensionsTests()
        {
            this.Report = StatLpDataGenerator.Instance.CreateStatLpReport("0001", 2021, 1);
        }

        [Fact]
        public void GetGroupedStays_TwoAdjacentStays_ReturnsOneGroup()
        {

            var stay1 = this.AddFirstStay(new DateTime(2021, 3, 1), AdmissionType.TrialAt, days: 10);

            var stay2 = this.AddAdjacentStay(AdmissionType.ContinuousAt, days: 10, gap: 0);

            var result = this.Report.GetGroupedStays(this.PersonId).ToArray();

            Assert.Equal(stay1.FromD, result[0].From);
            Assert.Equal(stay2.ToD, result[0].To);
            Assert.Equal(new[] { stay1, stay2 }, result[0].Stays);
        }

        [Fact]
        public void GetGroupedStays_TwoNonAdjacentStays_ReturnsTwoGroup()
        {

            var stay1 = this.AddFirstStay(new DateTime(2021, 3, 1), AdmissionType.TrialAt, days: 10);

            var stay2 = this.AddAdjacentStay(AdmissionType.ContinuousAt, days: 10, gap: 10);

            var result = this.Report.GetGroupedStays(this.PersonId).ToArray();

            Assert.Equal(stay1.FromD, result[0].From);
            Assert.Equal(stay1.ToD, result[0].To);
            Assert.Equal(new[] { stay1 }, result[0].Stays);

            Assert.Equal(stay2.FromD, result[1].From);
            Assert.Equal(stay2.ToD, result[1].To);
            Assert.Equal(new[] { stay2 }, result[1].Stays);
        }

        [Fact]
        public void GetGroupedStays_TwoOverlappingStays_ThrowsException()
        {
            var stay1 = this.AddFirstStay(new DateTime(2021, 3, 1), AdmissionType.TrialAt, days: 10);

            var stay2 = this.AddAdjacentStay(AdmissionType.ContinuousAt, days: 10, gap: -1);

            Assert.Throws<Exception>(() => this.Report.GetGroupedStays(this.PersonId).ToArray());
        }

        [Fact]
        public void GetGroupedStays_TwoAdjacentStaysWithSameType_ReturnsOneGroupWithOneStay()
        {
            var stay1 = this.AddFirstStay(new DateTime(2021, 3, 1), AdmissionType.ContinuousAt, days: 10);

            var stay2 = this.AddAdjacentStay(AdmissionType.ContinuousAt, days: 10);

            var result = this.Report.GetGroupedStays(this.PersonId, GroupedStay.SameTypeyGroupMode.Merge).ToArray();

            Assert.Equal(stay1.FromD, result[0].From);
            Assert.Equal(stay2.ToD, result[0].To);

            var expectedStay = new Stay(stay1)
            {
                To = stay2.To
            };

            Assert.Equal(new[] { expectedStay }, result[0].Stays);
        }

        protected Stay AddFirstStay(DateTime from, AdmissionType admissionType, int days)
        {
            this.Report.Stays.Clear();

            var stay = new Stay
            {
                PersonId = this.PersonId,
                FromD = from,
                ToD = from.AddDays(days),
                Type = admissionType
            };

            this.Report.Stays.Add(stay);

            return stay;
        }

        protected Stay AddAdjacentStay(AdmissionType admissionType, int days, int gap = 0)
        {
            var from = this.Report.Stays.Select(x => x.ToD).Max().AddDays(1).AddDays(gap);

            var stay = new Stay
            {
                PersonId = this.PersonId,
                FromD = from,
                ToD = from.AddDays(days),
                Type = admissionType
            };

            this.Report.Stays.Add(stay);

            return stay;
        }


    }
}
