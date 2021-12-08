using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
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
                        GroupedStay[] stays;
                        try
                        {
                            // mitunter geht das gar nicht, dann wird ein Fehler geworfen
                            stays = report.GetGroupedStays(person.Id, GroupedStay.SameTypeyGroupMode.NotAllowed).ToArray();
                        }
                        catch (Exception e)
                        {
                            var index = a.Persons.IndexOf(person);
                            ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)[index]}", $"{report.GetPersonName(person.Id)}: {e.Message}"));
                            return;
                        }

                        foreach (var s in stays)
                        {
                            // Aufenthalte vor Meldezeitraum
                            if (s.To < report.FromD)
                            {
                                var index = a.Stays.IndexOf(s.Stays[0]);
                                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}[{index}]",
                                    Validationmessages.StatLpStayAheadOfPeriod(report.GetPersonName(s.Stays[0].PersonId), s.From.ToShortDateString(), s.To.ToShortDateString())));
                            }

                            // Aufenthalte nach Meldezeitraum
                            if (s.To > report.ToD)
                            {
                                var index = a.Stays.IndexOf(s.Stays[0]);
                                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}[{index}]",
                                    Validationmessages.StatLpStayAfterPeriod(report.GetPersonName(s.Stays[0].PersonId), s.From.ToShortDateString(), s.To.ToShortDateString())));
                            }

                            StayLengthValidation(ctx, report, s, AdmissionType.HolidayAt, 42);

                            StayLengthValidation(ctx, report, s, AdmissionType.TransitionalAt, 365);

                            AdmissionTypeChangeValidation(ctx, report, s, AdmissionType.ContinuousAt, AdmissionType.HolidayAt);

                            AdmissionTypeChangeValidation(ctx, report, s, AdmissionType.ContinuousAt, AdmissionType.TransitionalAt);
                        }
                    }
                });
        }

        private void AdmissionTypeChangeValidation(CustomContext ctx, StatLpReport report, GroupedStay s, AdmissionType from, AdmissionType to)
        {
            for (var i = 1; i < s.Stays.Length; i++)
            {
                if (s.Stays[i].Type == to && s.Stays[i - 1].Type == from)
                {
                    var index = report.Stays.IndexOf(s.Stays[i]);

                    ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}[{index}]",
                        Validationmessages.StatLpInvalidAdmissionTyeChange($"{from}", $"{to}", report.GetPersonName(s.Stays[0].PersonId), s.From.ToShortDateString(), s.To.ToShortDateString())));
                }
            }
        }


        private void StayLengthValidation(CustomContext ctx, StatLpReport report, GroupedStay s, AdmissionType admissionType, int days)
        {
            //Länge Urlaubsbetreuung
            var holidayToLong = s.Stays.Where(x => x.Type == admissionType)
                .Where(x => x.ToD.Subtract(x.FromD).TotalDays > days)
                .FirstOrDefault();

            if (holidayToLong != null)
            {
                var index = report.Stays.IndexOf(holidayToLong);
                ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}[{index}]",
                    Validationmessages.StatLpStayToLong($"{holidayToLong.Type}", report.GetPersonName(s.Stays[0].PersonId), s.From.ToShortDateString(), s.To.ToShortDateString(), days)));
            }
        }
    }
}