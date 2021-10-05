using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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


        public StatLpReport CreateStandardAdmissionMessage(DateTime validFrom, DateTime validTo, string personId, DateTime admissionDate)
        {
            StatLpReport report = StatLpDataGenerator.Instance.CreateEmptyStatLpReport();

            report.FromD = validFrom;
            report.ToD = validTo;

            int personIndex = Convert.ToInt32(personId);

            var person = report.Persons.FirstOrDefault(x => x.Id == personId.ToString()) ?? report.AddDummyPerson(personIndex, false);

            var admission = StatLpDataGenerator.Instance.CreateAdmission(personId.ToString(), admissionDate);
            report.Admissions.Add(admission);

            IEnumerable<Attribute> attributes = StatLpDataGenerator.Instance.CreateAttributesForSingleAdmission(admission);
            report.Attributes.AddRange(attributes);

            var stay = StatLpDataGenerator.Instance.CreateStay(personId, admissionDate, report.ToD);
            report.Stays.Add(stay);

            return report;
        }





        public StatLpReport CreateStatLpReport(string institutionId = "", int? year = null, int? month = null, int persons = 100, int staffs = 5, bool addActivities = true)
        {
            var report = new StatLpReport()
            {
                Institution = new Institution() { Id = string.IsNullOrEmpty(institutionId) ? "1234" : institutionId, Name = "Testverein" },
                SourceSystemId = "System1"
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
                Institution = new Institution() { Id = "1234", Name = "Testverein" },
                SourceSystemId = "System1"
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
            };

            var regex = new Regex(@"^[a-zA-ZäöüÄÖÜß][-a-zA-ZäöüÄÖÜß ]*?[a-zA-ZäöüÄÖÜß]$");
            while (!regex.IsMatch(person.GivenName))
            {
                person.GivenName = randomValues ? _names[_rand.Next(_names.Length)] : _names[0];
            }
            while (!regex.IsMatch(person.FamilyName))
            {
                person.FamilyName = randomValues ? _names[_rand.Next(_names.Length)] : _names[0];
            }

            person.BirthdayD = randomValues ? new DateTime(1920, 01, 01).AddDays(_rand.Next(20000)) : new DateTime(1920, 01, 01);

            return person;
        }

        public IEnumerable<Person> CreatePersons(int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreatePerson(i + 1, true);
        }

        public Admission CreateAdmission(string personId, Nullable<DateTime> admissionDate = null)
        {

            var admission = new Admission()
            {
                PersonId = personId,

                LastPostcode = "6800",
                LastCity = "Feldkirch",

                HousingTypeBeforeAdmission = AdmissionLocation.AmbulatorySupervisedFlatAl,
                OtherHousingType = "",
                MainAttendanceRelation = MainAttendanceRelation.ChildMr,
                MainAttendanceCloseness = MainAttendanceCloseness.NoMainattendanceMc,
                HousingReason = HousingReason.BarriersEntranceHr,
                HousingReasonOther = "",
                PersonalChanges = { PersonalChange.IncreasedAssitanceNeedPc },
                PersonalChangeOther = "",
                SocialChanges = { SocialChange.MissingMealsSc },
                SocialChangeOther = "",
                Country = "AT",
                Gender = Gender.MaleGe,
            };

            if (admissionDate != null)
            {
                admission.AdmissionDateD = admissionDate.Value;
                admission.OriginAdmissionDateD = admissionDate.Value;
            }

            return admission;

        }

        public IEnumerable<Admission> CreateAdmissions(IEnumerable<Person> persons, Nullable<DateTime> reportFrom)
        {
            foreach (var person in persons)
            {
                yield return CreateAdmission(person.Id, reportFrom);
            }
        }

        public IEnumerable<Attribute> CreateAttributes(IEnumerable<Admission> admissions)
        {
            var attributes = new List<Attribute>();

            foreach (var admission in admissions)
            {
                attributes.AddRange(CreateAttributesForSingleAdmission(admission));
            }

            return attributes;
        }

        public IEnumerable<Attribute> CreateAttributesForSingleAdmission(Admission admission)
        {
            var attributes = new List<Attribute>();

            var admissionTypeAttribute = new Attribute();
            admissionTypeAttribute.PersonId = admission.PersonId;
            admissionTypeAttribute.FromD = admission.AdmissionDateD.GetValueOrDefault();
            admissionTypeAttribute.AttributeType = AttributeType.AdmissionType;
            admissionTypeAttribute.Value = AdmissionType.ContinuousAt.ToString();
            attributes.Add(admissionTypeAttribute);

            var careAllowanceAttribute = new Attribute();
            careAllowanceAttribute.FromD = admission.AdmissionDateD.GetValueOrDefault();
            careAllowanceAttribute.PersonId = admission.PersonId;
            careAllowanceAttribute.AttributeType = AttributeType.CareAllowance;
            careAllowanceAttribute.Value = CareAllowance.L1.ToString();
            attributes.Add(careAllowanceAttribute);

            var careAllowanceArgeAttribute = new Attribute();
            careAllowanceArgeAttribute.FromD = admission.AdmissionDateD.GetValueOrDefault();
            careAllowanceArgeAttribute.PersonId = admission.PersonId;
            careAllowanceArgeAttribute.AttributeType = AttributeType.CareAllowanceArge;
            careAllowanceArgeAttribute.Value = CareAllowanceArge.L1Ar.ToString();
            attributes.Add(careAllowanceArgeAttribute);

            var financeAttribute = new Attribute();
            financeAttribute.FromD = admission.AdmissionDateD.GetValueOrDefault();
            financeAttribute.PersonId = admission.PersonId;
            financeAttribute.AttributeType = AttributeType.Finance;
            financeAttribute.Value = Finance.SelfFi.ToString();
            attributes.Add(financeAttribute);

            return attributes;

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

        public Stay CreateStay(string personId, DateTime from, DateTime to)
        {
            var stay = CreateStay(personId, from);
            stay.ToD = to;
            return stay;
        }


        public IEnumerable<Leaving> CreateLeavings(IEnumerable<Person> persons, DateTime valid)
        {
            foreach (var person in persons)
            {
                yield return CreateLeaving(person.Id, valid);
            }
        }

        public Leaving CreateLeaving(string personId, DateTime valid)
        {
            var stay = new Leaving()
            {
                PersonId = personId,
                LeavingReason = LeavingReason.DischargeLr,
                DischargeLocation = DischargeLocation.HomeLivingAloneDc,
                DischargeReason = DischargeReason.OwnDesireDr,
                LeavingDateD = valid,
            };

            return stay;
        }

    }
}
