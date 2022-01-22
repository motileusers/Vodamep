using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHasUniqueIdValidator : AbstractValidator<StatLpReport>
    {
        public PersonHasUniqueIdValidator()
        {
            this.RuleFor(x => x.Persons)
                .Custom((list, ctx) =>
                {
                    foreach (var entry in list.GroupBy(x => (x.FamilyName, x.GivenName, x.Birthday)).Where(x => x.Count() > 1))
                    {
                        var ids = entry.Select(x => x.Id).ToArray();
                        var index = list.IndexOf(entry.First());
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]", Validationmessages.PersonWithMultipleIds(entry.First(), ids)));
                    }
                });
        }
    }
}
