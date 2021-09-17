using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Vodamep.ReportBase;
using Vodamep.ValidationBase;

internal class UniqePersonValidatorWithClientId : AbstractValidator<IPerson>
{
    public UniqePersonValidatorWithClientId(IEnumerable<IPerson> persons)
    {
        this.RuleFor(x => x)
            .Must(x => { return persons.Count(y => x.Id == y.Id) == 1; })
            .WithMessage(x => Validationmessages.ReportBaseIdIsNotUnique(x.Id));
    }
}