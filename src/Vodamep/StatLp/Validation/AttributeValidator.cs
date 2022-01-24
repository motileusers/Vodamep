using FluentValidation;
using System.Linq;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;
using Attribute = Vodamep.StatLp.Model.Attribute;

namespace Vodamep.StatLp.Validation
{

    internal class AttributeValidator : AbstractValidator<Attribute>
    {
        private static readonly DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public AttributeValidator(StatLpReport report)
        {
            this.RuleFor(x => x.From).SetValidator(new TimestampWithOutTimeValidator());

            this.RuleFor(x => x.FromD).Must(x => x >= report.FromD && x <= report.ToD)                
                .WithMessage(x => Validationmessages.ReportBaseItemMustBeInReportPeriod(report.GetPersonName(x.PersonId)));

            this.RuleFor(x => x)
                .Must(x => x.ValueCase != Attribute.ValueOneofCase.None)
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmptyWithString(report.GetPersonName(x.PersonId)));
        }
    }
}
