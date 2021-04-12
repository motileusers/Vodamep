using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Vodamep.ReportBase;
using Vodamep.ValidationBase;

namespace Vodamep.Mohi.Validation
{
    internal class PersonHasOnlyOneActivtyValidator : AbstractValidator<IPerson>
    {
        public PersonHasOnlyOneActivtyValidator(IEnumerable<IPersonActivity> personActivities)
        {
            this.RuleFor(x => x)
                .Must(x => { return personActivities.Count(y => y.PersonId == x.Id)  <= 1; })
                .WithMessage(x => Validationmessages.ReportBaseActivtyMultipleActivitiesForOnePerson(x.Id));
        }
    }
}