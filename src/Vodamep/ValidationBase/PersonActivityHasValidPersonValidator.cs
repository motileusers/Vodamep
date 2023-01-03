using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class PersonActivityHasValidPersonValidator : AbstractValidator<IPersonActivity>
    {
        public PersonActivityHasValidPersonValidator(IEnumerable<IPerson> persons)
        {
            this.RuleFor(x => x)
                .Must(x => { return persons.Any(y => y.Id == x.PersonId); })
                .WithMessage(x => Validationmessages.ReportBaseActivityContainsNonExistingPerson(x.PersonId));
        }
    }
}