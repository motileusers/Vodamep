using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{
    internal class ClientActivityContainsCorrectPersonIdValidator : AbstractValidator<IPersonDateActivity>
    {
        public ClientActivityContainsCorrectPersonIdValidator(IEnumerable<string> availablePersonIds)
        {
            this.RuleFor(x => x)
                .Must(x => availablePersonIds.Contains(x.PersonId))
                .WithMessage(x => Validationmessages.ReportBaseClientActivityUnknownPerson(x.Date.ToDateTime().ToShortDateString()));
        }

        
    }
}
