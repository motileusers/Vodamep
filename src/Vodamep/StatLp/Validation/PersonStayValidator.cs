using FluentValidation;
using FluentValidation.Results;
using System;
using System.Linq;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonStayValidator : AbstractValidator<StatLpReport>
    {
        public PersonStayValidator()
        {
            // Zu jeder Person muss es mindestens einen Aufenthalt geben
            this.RuleFor(x => new { x.Persons, x.Stays })
                .Custom((a, ctx) =>
                {
                    StatLpReport report = ctx.InstanceToValidate as StatLpReport;

                    var idPersons = a.Persons.Select(x => x.Id).Distinct().ToArray();
                    var personIdsStays = (a.Stays.Select(x => x.PersonId)).Distinct().ToArray();

                    foreach (var id in idPersons.Except(personIdsStays))
                    {
                        var item = a.Persons.First(x => x.Id == id);
                        var index = a.Persons.IndexOf(item);
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]",
                            Validationmessages.StatLpStayEveryPersonMustBeInAStay(report.GetPersonName(id))));

                    }
                });

            // Zu jedem Aufenthalt muss es die Person geben
            this.RuleFor(x => new { x.Persons, x.Stays })
                .Custom((a, ctx) =>
                {
                    StatLpReport report = ctx.InstanceToValidate as StatLpReport;

                    var idPersons = a.Persons.Select(x => x.Id).Distinct();

                    foreach (var stay in a.Stays.Where(x => !idPersons.Contains(x.PersonId)))
                    {
                        var index = a.Stays.IndexOf(stay);
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}[{index}]", "Für einen Aufenthalt wurde keine Person gemeldet!"));
                    }
                });


            // der Zeiträume der Aufenthalte müssen passen
            this.RuleFor(x => new { x.Persons, x.Stays })
                .Custom((a, ctx) =>
                {
                    var report = ctx.InstanceToValidate as StatLpReport;


                    foreach (var person in a.Persons)
                    {
                        (DateTime From, DateTime To, Stay[] Stays)[] stays;
                        try
                        {
                            // mitunter geht das gar nicht, dann wird ein Fehler geworfen
                            stays = report.GetGroupedStays(person.Id).ToArray();
                        }
                        catch (Exception e)
                        {
                            var index = a.Persons.IndexOf(person);
                            ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)[index]}", $"{report.GetPersonName(person.Id)}: {e.Message}"));
                            return;
                        }

                        foreach (var s in stays)
                        {
                            if (s.To < report.FromD)
                            {
                                var index = a.Stays.IndexOf(s.Stays[0]);
                                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}[{index}]",
                                    Validationmessages.StatLpStayAheadOfPeriod(report.GetPersonName(s.Stays[0].PersonId), s.From.ToShortDateString(), s.To.ToShortDateString())));
                            }

                            if (s.To > report.ToD)
                            {
                                var index = a.Stays.IndexOf(s.Stays[0]);
                                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}[{index}]",
                                    Validationmessages.StatLpStayAfterPeriod(report.GetPersonName(s.Stays[0].PersonId), s.From.ToShortDateString(), s.To.ToShortDateString())));
                            }
                        }
                    }
                });
        }
    }
}