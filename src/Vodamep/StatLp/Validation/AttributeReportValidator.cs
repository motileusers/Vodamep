using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{

    internal class AttributeReportValidator : AbstractValidator<StatLpReport>
    {
        private static readonly DisplayNameResolver DisplayNameResolver = new DisplayNameResolver();

        public AttributeReportValidator()
        {
            this.RuleFor(x => x).Custom((x, ctx) =>
            {
                if (x.Admissions.Any())
                {
                    foreach (var personId in x.Attributes.Select(a => a.PersonId).Distinct())
                    {
                        var attributes = x.Attributes.Where(at => at.PersonId == personId);

                        foreach (var attributeType in
                            ((AttributeType[])System.Enum.GetValues(typeof(AttributeType))).Where(at =>
                                at != AttributeType.UndefinedAttribute))
                        {

                            var attributeCount = attributes.Count(a => a.AttributeType == attributeType);

                            if (attributeCount <= 0)
                            {
                                ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.Admissions),
                                    Validationmessages.StatLpReportAttributeMissing(
                                        personId,
                                        x.FromD.ToShortDateString(),
                                        DisplayNameResolver.GetDisplayName(attributeType.ToString()))));
                            }
                            else if (attributeCount > 1)
                            {
                                ctx.AddFailure(new ValidationFailure(nameof(StatLpReport.Admissions),
                                    Validationmessages.StatLpReportMultipleAttribute(
                                        personId,
                                        x.FromD.ToShortDateString(),
                                        DisplayNameResolver.GetDisplayName(attributeType.ToString()))));

                            }
                        }
                    }
                }
            });
        }
    }
}
