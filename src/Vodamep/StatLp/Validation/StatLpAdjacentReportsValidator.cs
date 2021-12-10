using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Linq;
using System.Threading;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class StatLpAdjacentReportsValidator : AbstractValidator<(StatLpReport Predecessor, StatLpReport Report)>
    {

        private static readonly DisplayNameResolver DisplayNameResolver = new DisplayNameResolver();

        static StatLpAdjacentReportsValidator()
        {
            var isGerman = Thread.CurrentThread.CurrentCulture.Name.StartsWith("de", StringComparison.CurrentCultureIgnoreCase);
            if (isGerman)
            {
                var loc = new DisplayNameResolver();
                ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
            }
        }

        public StatLpAdjacentReportsValidator()
        {
            this.RuleFor(x => x).Custom((data, ctx) =>
            {
                // gemeinsame PersonIds
                var personIds = data.Report.Persons.Select(x => x.Id)
                    .Intersect(data.Predecessor.Persons.Select(x => x.Id))
                    .ToArray();

                CheckBirthday(data, ctx, personIds);

                CheckGenders(data, ctx, personIds);
            });

            this.RuleFor(x => x).Custom(CheckStays);
        }

        private void CheckBirthday((StatLpReport Predecessor, StatLpReport Report) data, CustomContext ctx, string[] personIds)
        {
            var values1 = data.Report.Persons
                   .Where(x => personIds.Contains(x.Id))
                   .Select(x => (x.Id, x.BirthdayD))
                   .Distinct()
                   .ToDictionary(x => x.Id, x => x.BirthdayD);

            var values2 = data.Predecessor.Persons
                .Where(x => personIds.Contains(x.Id))
                .Select(x => (x.Id, x.BirthdayD))
                .Distinct().ToDictionary(x => x.Id, x => x.BirthdayD);

            foreach (var personId in personIds)
            {
                if (!values1.TryGetValue(personId, out var v1) || values2.TryGetValue(personId, out var v2) || v1 != v2)
                {
                    var person = data.Report.Persons.Where(x => x.Id == personId).FirstOrDefault();
                    var index = person != null ? data.Report.Persons.IndexOf(person) : -1;

                    ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]",
                        Validationmessages.PersonsPropertyDiffers(
                            data.Report.GetPersonName(personId),
                            DisplayNameResolver.GetDisplayName(nameof(Person.Birthday)),
                            values1[personId].ToShortDateString(),
                            values2[personId].ToShortDateString()
                            ))
                    {
                        Severity = Severity.Warning
                    });
                }
            }
        }

        private void CheckGenders((StatLpReport Predecessor, StatLpReport Report) data, CustomContext ctx, string[] personIds)
        {
            var values1 = data.Report.Admissions
                   .Where(x => personIds.Contains(x.PersonId))
                   .Select(x => (x.PersonId, x.Gender))
                   .Distinct().ToDictionary(x => x.PersonId, x => x.Gender);

            var values2 = data.Predecessor.Admissions
                .Where(x => personIds.Contains(x.PersonId))
                .Select(x => (x.PersonId, x.Gender))
                .Distinct().ToDictionary(x => x.PersonId, x => x.Gender);

            foreach (var personId in personIds)
            {
                if (!values1.TryGetValue(personId, out var v1) || values2.TryGetValue(personId, out var v2) || v1 != v2)
                {
                    var person = data.Report.Persons.Where(x => x.Id == personId).FirstOrDefault();
                    var index = person != null ? data.Report.Persons.IndexOf(person) : -1;

                    ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]",
                        Validationmessages.PersonsPropertyDiffers(
                            data.Report.GetPersonName(personId),
                            DisplayNameResolver.GetDisplayName(nameof(Admission.Gender)),
                            DisplayNameResolver.GetDisplayName(values1[personId].ToString()),
                            DisplayNameResolver.GetDisplayName(values2[personId].ToString())
                            ))
                    {
                        Severity = Severity.Warning
                    });
                }
            }
        }

        private void CheckStays((StatLpReport Predecessor, StatLpReport Report) data, CustomContext ctx)
        {
            // Personen mit Aufenthalten, die auch die nachfolgende Meldung betreffen
            var p1 = data.Predecessor.Stays.Where(x => x.To == null || x.ToD > data.Predecessor.ToD).Select(x => x.PersonId);

            // Personen mit Aufenthalten, die auch die vorangehende Meldung betreffen
            var p2 = data.Report.Stays.Where(x => x.FromD < data.Report.FromD).Select(x => x.PersonId);

            var personIds = p1.Union(p2).Distinct().ToArray();

            foreach (var personId in personIds)
            {
                var stays1 = data.Predecessor.Stays.Where(x => x.PersonId == personId).ToArray()
                    .GetGroupedStays()
                    .Where(x => x.To == null || x.To > data.Predecessor.ToD)
                    .FirstOrDefault();

                var stays2 = data.Report.Stays.Where(x => x.PersonId == personId && x.FromD < data.Report.FromD).ToArray()
                    .GetGroupedStays()
                    .FirstOrDefault();


                if (stays1 == null || stays2 == null || !stays1.Equals(stays2))
                {
                    var person = data.Report.Persons.Where(x => x.Id == personId).FirstOrDefault();

                    var index = person != null ? data.Report.Persons.IndexOf(person) : -1;
                    var name = person != null ? data.Report.GetPersonName(personId) : data.Predecessor.GetPersonName(personId);

                    if (stays1 == null)
                    {
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]",
                             $"Aufenthalte von '{name}' wurden letztes Jahr gemeldet. Sie fehlen in diesem Jahr"));
                    }
                    else if (stays2 == null)
                    {
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]",
                            $"Aufenthalte von '{name}' wurden dieses Jahr gemeldet. Sie fehlen im letzten Jahr"));
                    }
                    else
                    {
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]",
                            $"Aufenthalte von '{name}' stimmen nicht mit der vorhergehenden Meldung überein"));
                    }
                }

            }


        }
    }
}
