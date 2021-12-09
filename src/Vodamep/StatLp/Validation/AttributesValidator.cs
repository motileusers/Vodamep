
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{

    internal class AttributesValidator : AbstractValidator<StatLpReport>
    {
        private static readonly DisplayNameResolver DisplayNameResolver = new DisplayNameResolver();

        public AttributesValidator()
        {
            // keine Duplikate
            this.RuleFor(x => x.Attributes)
                .Custom((attributes, ctx) =>
            {
                var report = ctx.InstanceToValidate as StatLpReport;

                foreach (var a in attributes
                    .GroupBy(x => (x.PersonId, x.AttributeType, x.FromD))
                    .Where(x => x.Count() > 1))
                {
                    ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.Admissions),
                                        Validationmessages.StatLpAttributeMultiple(
                                            report.GetPersonName(a.Key.PersonId),
                                            a.Key.FromD.ToShortDateString(),
                                            DisplayNameResolver.GetDisplayName(a.Key.AttributeType.ToString()))));
                }
            });
        }
    }
}
