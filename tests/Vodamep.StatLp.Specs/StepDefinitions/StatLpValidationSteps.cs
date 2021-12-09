using FluentValidation;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TechTalk.SpecFlow;
using Vodamep.Data.Dummy;
using Vodamep.StatLp.Model;
using Vodamep.StatLp.Validation;
using Attribute = Vodamep.StatLp.Model.Attribute;
using Enum = System.Enum;

namespace Vodamep.Specs.StepDefinitions
{

    [Binding]
    public class StatLpValidationSteps
    {
        private readonly ReportContext _context;

        public StatLpValidationSteps(ReportContext context)
        {
            _context = context;
            _context.GetPropertiesByType = this.GetPropertiesByType;
            _context.Validator = new StatLpReportValidator();

            var loc = new DisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            this._context.Report = StatLpDataGenerator.Instance.CreateStatLpReport("0001", 2021, 1);
        }

        private IEnumerable<IMessage> GetPropertiesByType(string type)
        {
            return type switch
            {
                nameof(Person) => new[] { this.Report.Persons[0] },
                nameof(Admission) => new[] { this.Report.Admissions[0] },
                nameof(Leaving) => this.Report.Leavings,
                nameof(Attribute) => this.Report.Attributes,
                nameof(Stay) => this.Report.Stays,
                nameof(Institution) => new[] { this.Report.Institution },
                _ => Array.Empty<IMessage>(),
            };
        }

        public StatLpReport Report => _context.Report as StatLpReport;

        [Given(@"es ist ein 'StatLpReport'")]
        public void GivenItIsAHkpvReport()
        {

        }


        [Given(@"alle Listen sind leer")]
        public void GivenAllListsAreEmpty()
        {
            this.Report.Admissions.Clear();
            this.Report.Attributes.Clear();
            this.Report.Leavings.Clear();
            this.Report.Persons.Clear();
            this.Report.Stays.Clear();
        }

        [Given(@"die Liste '(.*)' ist leer")]
        public void GivenTheListIsEmpty(string type)
        {
            if (type == nameof(Attribute))
            {
                this.Report.Attributes.Clear();
            }
            else if (type == nameof(Admission))
            {
                this.Report.Admissions.Clear();
            }
        }



        [Given(@"die Auflistungs Eigenschaft von Admission mit dem Auflistungstyp '(\w*)' ist auf '(.*)' gesetzt")]
        public void GivenTheEnumLIstPropertyFromAdmissionIsSetTo(string type, string value)
        {
            //todo geht bestimmt eleeganter
            switch (type)
            {
                case "PersonalChanges":
                    this.SetPersonalChanges(value);
                    break;

                case "SocialChanges":
                    this.SetSocialChanges(value);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void SetPersonalChanges(string value)
        {
            this.Report.Admissions.First().PersonalChanges.Clear();

            if (value.Contains(','))
            {
                var personalChange = value.Split(',').Select(x => (PersonalChange)Enum.Parse(typeof(PersonalChange), x));
                this.Report.Admissions[0].PersonalChanges.AddRange(personalChange);
            }
            else if (Enum.TryParse(value, out PersonalChange activityType))
            {
                this.Report.Admissions[0].PersonalChanges.Add(activityType);
            }
            else if (value == "")
            {
                //nothing do do, already emptied yet
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void SetSocialChanges(string value)
        {
            this.Report.Admissions.First().SocialChanges.Clear();

            if (value.Contains(','))
            {
                var activityTypes = value.Split(',').Select(x => (SocialChange)Enum.Parse(typeof(SocialChange), x));
                this.Report.Admissions[0].SocialChanges.AddRange(activityTypes);
            }
            else if (Enum.TryParse(value, out SocialChange socialChange))
            {
                this.Report.Admissions[0].SocialChanges.Add(socialChange);
            }
            else if (value == "")
            {
                //nothing do do, already emptied yet
            }
            else
            {
                throw new NotImplementedException();
            }
        }



        [Given(@"der Id einer Person ist nicht eindeutig")]
        public void GivenPersonIdNotUnique()
        {
            var p0 = this.Report.Persons[0];

            var p = this.Report.AddDummyPerson();

            p.Id = p0.Id;
            p.Id = p0.Id;
        }


        [Given(@"es gibt eine weitere Person")]
        public void GivenThereIsAnotherPerson()
        {
            var p = this.Report.AddDummyPerson(2);
        }

        [Given(@"Bis ist vor Von bei einem Stay")]
        public void GivenToIsBeforeFromAtFirstStay()
        {
            var stay = this.Report.Stays[0];

            stay.From = stay.To.AsDate().AddDays(2).AsTimestamp();
        }

        [Given(@"das Attribut '(.*)' fehlt")]
        public void GivenAttributeIsMissing(string attributeType)
        {
            var type = (AttributeType)Enum.Parse(typeof(AttributeType), attributeType);

            this.Report.Attributes.Remove(this.Report.Attributes.FirstOrDefault(x => x.AttributeType == type));
        }

        [Given(@"das Attribut mit dem  Typ '(.*)' ist auf den Wert '(.*)' gesetzt")]
        public void GivenAttributeValueIsSet(string attributeType, string value)
        {
            var type = (AttributeType)Enum.Parse(typeof(AttributeType), attributeType);

            this.Report.Attributes.First(x => x.AttributeType == type).Value = value;
        }

        [Given(@"es gibt am '(.*)' ein zusätzliches Attribut vom Typ '(.*)' und dem Wert '(.*)'")]
        public void GivenThereIsOneAdditionalAttribute(string date, string attributeType, string value)
        {
            var type = (AttributeType)Enum.Parse(typeof(AttributeType), attributeType);

            this.Report.Attributes.Add(new Attribute()
            {
                AttributeType = type,
                FromD = DateTime.Parse(date, new CultureInfo("de-DE")),
                PersonId = this.Report.Persons[0].Id,
                Value = value
            });
        }


        [Given(@"die erste Aufnahme startet am '(.+)', dauert (\d+) Tage und ist eine '(\w+)'")]
        public void GivenTheFirstStayWithDuration(string date, int days, string type)
        {
            this.Report.Stays.Clear();
            this.Report.Leavings.Clear();
            this.Report.Admissions.Clear();
            this.Report.Attributes.Clear();

            this.GivenAnOtherStayWithDuration(date, days, type);

        }

        [Given(@"eine weitere Aufnahme startet am '(.+)', dauert (\d+) Tage und ist eine '(\w+)'")]
        public void GivenAnOtherStayWithDuration(string date, int days, string type)
        {

            var personId = this.Report.Persons[0].Id;
            var from = DateTime.Parse(date, new CultureInfo("de-DE"));

            this.Report.Stays.Add(new Stay
            {
                PersonId = personId,
                FromD = from,
                ToD = days >= 0 ? from.AddDays(days) : null,
                Type = Enum.Parse<AdmissionType>(type)
            });

            this.Report.Admissions.Add(StatLpDataGenerator.Instance.CreateAdmission(personId, from));

            this.Report.Attributes.Add(StatLpDataGenerator.Instance.CreateAttributes(personId, from >= this.Report.FromD ? from : this.Report.FromD));

        }

        [Given(@"die erste Aufnahme startet am '(.+)' und ist eine '(\w+)'")]
        public void GivenTheFirstStay(string date, string type) => this.GivenTheFirstStayWithDuration(date, -1, type);

        [Given(@"es gibt eine weitere Aufnahme '(\w+)', die (\d+) Tage dauert")]
        public void GivenAnotherStay(string type, int days) => this.GivenAnotherStayWithGap(type, days, 0);


        [Given(@"es gibt eine weitere Aufnahme '(\w+)', die (\d+) Tage dauert, dazwischen liegen (-?\d+) Tage")]
        public void GivenAnotherStayWithGap(string type, int days, int gap)
        {
            var personId = this.Report.Persons[0].Id;
            var from = this.Report.Stays.Where(x => x.PersonId == personId && x.To != null).Select(x => x.ToD.Value).Max().AddDays(1).AddDays(gap);

            var stay = new Stay
            {
                PersonId = personId,
                FromD = from,
                ToD = from.AddDays(days),
                Type = Enum.Parse<AdmissionType>(type)
            };

            if (stay.ToD > Report.ToD)
            {
                stay.ToD = null;
            }

            this.Report.Stays.Add(stay);
        }

        [Given("es gibt eine Entlassungsmeldung mehrfach")]
        public void GivenMultipleLeavings()
        {
            if (this.Report.Leavings.Any())
            {
                this.Report.Leavings.Add(new Leaving(this.Report.Leavings[0]));
            }
        }
    }
}
