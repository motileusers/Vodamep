using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.ReportBase;
using Vodamep.ValidationBase;

namespace Vodamep.ValidationBase
{
    
    internal class UniqePersonValidator : AbstractValidator<IReportBase>
    {
        public UniqePersonValidator()
        {
            this.RuleFor(x => x.Persons)
                .Custom((list, ctx) =>
                {
                    foreach (var id in list.Select(x => x.Id).OrderBy(x => x).GroupBy(x => x).Where(x => x.Count() > 1))
                    {
                        var item = list.Where(x => x.Id == id.Key).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(ctx.PropertyName)}[{index}]", Validationmessages.IdIsNotUnique));
                    }
                });
        }
    }

}