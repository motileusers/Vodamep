using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class PersonHasOnlyOneActivtyValidator : AbstractValidator<IPerson>
    {
        public PersonHasOnlyOneActivtyValidator(IEnumerable<IPersonActivity> personActivities)
        {
            this.RuleFor(x => x)
                .Must(x => { return personActivities.Count(y => y.PersonId == x.Id)  <= 1; })
                .WithMessage(x => Validationmessages.ReportBaseActivityMultipleActivitiesForOnePerson(x.Id));
        }
    }
}