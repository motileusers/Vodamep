using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class MkkpReportPersonIdValidator : AbstractValidator<MkkpReport>
    {
        public MkkpReportPersonIdValidator()
        {
            this.RuleFor(x => x.Persons)
                .Custom((list, ctx) =>
                {
                    foreach (var id in list.Select(x => x.Id).OrderBy(x => x).GroupBy(x => x).Where(x => x.Count() > 1))
                    {
                        var item = list.Where(x => x.Id == id.Key).First();
                        var index = list.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(MkkpReport.Persons)}[{index}]", Validationmessages.IdIsNotUnique));
                    }
                });
        }
    }
}