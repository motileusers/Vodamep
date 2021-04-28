using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Vodamep.StatLp.Model;
using Attribute = Vodamep.StatLp.Model.Attribute;
using Enum = System.Enum;

namespace Vodamep.Data.Dummy
{
    internal class StatLpDataGenerator : GeneratorBase
    {
        private static StatLpDataGenerator _instance;

        public static StatLpDataGenerator Instance
        {
            get
            {

                if (_instance == null)
                    _instance = new StatLpDataGenerator();

                return _instance;
            }
        }

        private StatLpDataGenerator()
        {

        }

        public StatLpReport CreateStatLpReport(string institutionId = "", int? year = null, int? month = null, int persons = 100, int staffs = 5, bool addActivities = true)
        {
            var report = new StatLpReport()
            {
                Institution = new Institution() { Id = string.IsNullOrEmpty(institutionId) ? "1234" : institutionId, Name = "Testverein" }
            };

            var from = year.HasValue || month.HasValue ? new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1) : DateTime.Today.FirstDateInMonth().AddMonths(-1);


            report.FromD = from;
            report.ToD = report.FromD.LastDateInMonth();

            report.AddDummyPersons(persons);
            report.AddDummyAdmissions();
            report.AddDummyAttributes();
            report.AddDummyStays(from);
            report.AddDummyLeavings();

            return report;
        }

        public StatLpReport CreateEmptyStatLpReport()
        {
            var report = new StatLpReport()
            {
                Institution = new Institution() { }
            };

            return report;
        }

        public Person CreatePerson(bool randomValues = true)
        {
            return this.CreatePerson(_id++, randomValues);
        }

        public Person CreatePerson(long index, bool randomValues)
        {
            if (index < 0)
            {
                index = _id++;
            }

            var person = new Person()
            {
                Id = index.ToString(),
                FamilyName = randomValues ? _familynames[_rand.Next(_familynames.Length)] : _familynames[0],
                GivenName = randomValues ? _names[_rand.Next(_names.Length)] : _names[0],
                Country = randomValues ? CountryCodeProvider.Instance.Values.Keys.ToArray()[_rand.Next(CountryCodeProvider.Instance.Values.Keys.Count())] : CountryCodeProvider.Instance.Values.Keys.ToArray()[0],
                Gender = randomValues ? ((Gender[])(Enum.GetValues(typeof(Gender))))
                    .Where(x => x != Gender.UndefinedGe)
                    .ElementAt(_rand.Next(Enum.GetValues(typeof(Gender)).Length - 1)) : Gender.MaleGe,
            };

            person.BirthdayD = randomValues ? new DateTime(1920, 01, 01).AddDays(_rand.Next(20000)) : new DateTime(1920, 01, 01);

            return person;
        }

        public IEnumerable<Person> CreatePersons(int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreatePerson(i + 1, true);
        }

        public Admission CreateAdmission(string personId, Timestamp valid = null)
        {
            var admission = new Admission()
            {
                PersonId = personId,

                LastPostcode = "6800",
                LastCity = "Feldkirch",

                HousingTypeBeforeAdmission = AdmissionLocation.AmbulatorySupervisedFlatAl,
                MainAttendanceRelation = MainAttendanceRelation.ChildMr,
                MainAttendanceCloseness = MainAttendanceCloseness.NoMainattendanceMc,
                HousingReason = HousingReason.BarriersEntranceHr,
                PersonalChanges = { PersonalChange.IncreasedAssitanceNeedPc },
                SocialChanges = { SocialChange.MissingMealsSc },

            };


            if (valid != null)
                admission.Valid = valid;

            return admission;

        }

        public IEnumerable<Admission> CreateAdmissions(IEnumerable<Person> persons, Timestamp reportFrom)
        {
            foreach (var person in persons)
            {
                yield return CreateAdmission(person.Id, reportFrom);
            }
        }

        public IEnumerable<Attribute> CreateAttributes(IEnumerable<Person> persons, Timestamp @from)
        {
            foreach (var person in persons)
            {
                yield return CreateAttribute(person.Id, @from);
            }
        }

        public Attribute CreateAttribute(string personId, Timestamp @from)
        {
            var attribute = new Attribute()
            {
                PersonId = personId,
            };

            if (from != null)
                attribute.From = @from;

            return attribute;

        }

        public IEnumerable<Stay> CreateStays(IEnumerable<Person> persons, DateTime from)
        {
            foreach (var person in persons)
            {
                yield return CreateStay(person.Id, from);
            }
        }

        public Stay CreateStay(string personId, DateTime from)
        {
            var stay = new Stay()
            {
                PersonId = personId,
                From = from.AsTimestamp(),
                To = from.AddDays(10).AsTimestamp(),
            };

            return stay;
        }

        public IEnumerable<Leaving> CreateLeavings(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                yield return CreateLeaving(person.Id);
            }
        }

        public Leaving CreateLeaving(string personId)
        {
            var stay = new Leaving()
            {
                PersonId = personId,
                LeavingReason = LeavingReason.DischargeLr,
                DischargeLocation = DischargeLocation.HomeLivingAloneDc,
                DischargeReason = DischargeReason.OwnDesireDr,
            };

            return stay;
        }

    }
}