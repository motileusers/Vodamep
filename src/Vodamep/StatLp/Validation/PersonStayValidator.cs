using System;
using System.Linq;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;


namespace Vodamep.StatLp.Validation
{
    internal class PersonStayValidator : AbstractValidator<StatLpReport>
    {
        public PersonStayValidator()
        {
            //corert kann derzeit nicht mit AnonymousType umgehen. Vielleicht später: new { x.Staffs, x.Activities, x.Consultations }
            this.RuleFor(x => new Tuple<IList<Person>, IEnumerable<Stay>>(x.Persons, x.Stays))
                .Custom((a, ctx) =>
                {
                    var persons = a.Item1;
                    var stays = a.Item2;

                    var idPersons = persons.Select(x => x.Id).Distinct();
                    var personIdsStays = (stays.Select(x => x.PersonId)).Distinct();

                    foreach (var id in idPersons.Except(personIdsStays))
                    {
                        var item = persons.First(x => x.Id == id);
                        var index = persons.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]", Validationmessages.StatLpStayEveryPersonMustBeInAStay(id)));

                    }
                });
        } 
    }
}