using FluentValidation;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Vodamep.Data.Dummy;
using Vodamep.StatLp.Model;
using Vodamep.StatLp.Validation;
using Attribute = Vodamep.StatLp.Model.Attribute;

namespace Vodamep.Specs.StatLp.StepDefinitions
{
    [Binding]
    public class StatLpAdjacentValidationSteps
    {
        private readonly ReportContext _context;

        public StatLpAdjacentValidationSteps(ReportContext context)
        {
            if (context.Report == null)
            {
                InitContext(context);
            }

            _context = context;
        }

        private void InitContext(ReportContext context)
        {
            context.GetPropertiesByType = this.GetPropertiesByType;
            context.Validator = new StatLpAdjacentReportsValidator();
            context.Validate = () => new StatLpAdjacentReportsValidator().Validate(((StatLpReport)context.PrecedingReport, (StatLpReport)context.Report));

            var loc = new DisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);

            var r1 = StatLpDataGenerator.Instance.CreateStatLpReport("0001", 2020, 1);
            r1.Admissions[0].Gender = Gender.FemaleGe;
            r1.Persons[0].BirthdayD = new DateTime(1955, 1, 1);
            r1.Stays[0].To = null;

            var r2 = new StatLpReport(r1)
            {
                FromD = r1.FromD.AddYears(1),
                ToD = r1.ToD.AddYears(1)
            };

            foreach (var attribut in r2.Attributes)
            {
                attribut.FromD = r2.FromD;
            }

            context.PrecedingReport = r1;
            context.Report = r2;

        }

        public StatLpReport Report => _context.Report as StatLpReport;

        [Given("es gibt zwei aneinander grenzende StatLp-Datenmeldungen")]
        public void GivenAdjacentReports()
        {

        }

        private IEnumerable<IMessage> GetPropertiesByType(string type)
        {
            return type switch
            {
                nameof(Person) => this.Report.Persons,
                nameof(Admission) => this.Report.Admissions,
                nameof(Leaving) => this.Report.Leavings,
                nameof(Attribute) => this.Report.Attributes,
                nameof(Stay) => this.Report.Stays,
                nameof(Institution) => new[] { this.Report.Institution },
                _ => Array.Empty<IMessage>(),
            };
        }
    }
}
