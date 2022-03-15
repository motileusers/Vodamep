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

        [Fact]
        public void GetGroupedStays_StaysWithDiversePersonId_ThrowsException()
        {
            var stay1 = this.AddFirstStay(new DateTime(2021, 3, 1), AdmissionType.TrialAt, days: 10);

            var stay2 = this.AddAdjacentStay(AdmissionType.ContinuousAt, days: 10, gap: 0);

            stay2.PersonId = this.PersonId + "0";

            var stays = new[] { stay1, stay2 };

            Assert.Throws<Exception>(() => stays.GetGroupedStays().ToArray());
        }

        [Fact]
        public void GetGroupedStays_StaysNotOrderedByFrom_ThrowsException()
        {
            var stay1 = this.AddFirstStay(new DateTime(2021, 3, 1), AdmissionType.TrialAt, days: 10);

            var stay2 = this.AddAdjacentStay(AdmissionType.ContinuousAt, days: 10, gap: 0);

            var stays = new[] { stay2, stay1 };

            Assert.Throws<Exception>(() => stays.GetGroupedStays().ToArray());
        }

        [Fact]
        public void GetGroupedStays_StaysMergedWithEmptyToDateBetween_ThrowsException()
        {
            var stay1 = this.AddFirstStay(new DateTime(2021, 3, 1), AdmissionType.TrialAt, days: 10);

            var stay2 = this.AddAdjacentStay(AdmissionType.ContinuousAt, days: 10, gap: 0);
            stay2.ToD = null;

            var stay3 = this.AddAdjacentStay(AdmissionType.ContinuousAt, days: 10, gap: 0);

            var stays = new[] { stay1, stay2, stay3 };

            Assert.Throws<Exception>(() => stays.GetGroupedStays(GroupedStay.SameTypeyGroupMode.Merge).ToArray());
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
            var from = this.Report.Stays.Where(x => x.To != null).Select(x => x.ToD.Value).Max().AddDays(1).AddDays(gap);

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


        [Fact]
        public void RemoveDoublets_OneReport_DoubletteIsRemoved()
        {
            var p1 = this.Report.Persons.First();

            //p2 unterscheidet sich nur durch den Id
            var p2 = new Person(p1) { Id = $"x{p1.Id}" };

            this.Report.AddPerson(p2);

            var r = this.Report.RemoveDoubletes();

            Assert.Contains(p1, r.Persons);
            Assert.DoesNotContain(p2, r.Persons);
        }


        [Fact]
        public void RemoveDoublets_TwoReport_DoubletteIsRemoved()
        {

            var r1 = this.Report;
            var p1 = r1.Persons.First();

            var r2 = r1.Clone();

            var p2 = r2.Persons.First();
            p2.Id = $"x{p2.Id}";   //unterscheidet sich nur durch den Id


            var rs = new[] { r1, r2 }.RemoveDoubletes();

            foreach (var r in rs)
            {
                Assert.Contains(p1, r.Persons);
                Assert.DoesNotContain(p2, r.Persons);
            }
        }


    }
}
