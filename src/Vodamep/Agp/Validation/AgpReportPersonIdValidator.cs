using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class AgpReportPersonIdValidator : AbstractValidator<AgpReport>
    {
        public AgpReportPersonIdValidator()
        {
            this.RuleFor(x => x.Persons)
                .Custom((list, ctx) =>
                {
                    foreach (var id in list.Select(x => x.Id).OrderBy(x => x).GroupBy(x => x).Where(x => x.Count() > 1))
                    {
                        var item = list.Where(x => x.Id == id.Key).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(AgpReport.Persons)}[{index}]", Validationmessages.IdIsNotUnique));
                    }
                });
        }
    }
}