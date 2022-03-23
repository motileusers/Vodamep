using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Linq;
using System.Threading;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation.Adjacent
{
    internal class StatLpAdjacentReportsStaysValidator : AbstractValidator<(StatLpReport Predecessor, StatLpReport Report)>
    {

        private static readonly DisplayNameResolver DisplayNameResolver = new DisplayNameResolver();

        static StatLpAdjacentReportsStaysValidator()
        {
            var loc = new DisplayNameResolver();
            ValidatorOptions.DisplayNameResolver = (type, memberInfo, expression) => loc.GetDisplayName(memberInfo?.Name);
        }

        public StatLpAdjacentReportsStaysValidator()
        {
            this.RuleFor(x => x).Custom(CheckStays);
        }

        private void CheckStays((StatLpReport Predecessor, StatLpReport Report) data, CustomContext ctx)
        {
            // Personen mit Aufenthalten, die auch die nachfolgende Meldung betreffen
            var p1 = data.Predecessor.Stays.Where(x => x.To == null || x.ToD > data.Predecessor.ToD).Select(x => x.PersonId);

            // Personen mit Aufenthalten, die auch die vorangehende Meldung betreffen
            var p2 = data.Report.Stays.Where(x => x.FromD < data.Report.FromD).Select(x => x.PersonId);

            // Personen deren Aufenthalt zum Ende des Meldezeitraus endet und keinen Eintrag Leavings haben
            var p3 = data.Predecessor.Stays
                .Where(x => x.ToD == data.Predecessor.ToD)
                .Where(x => !data.Predecessor.Leavings.Where(l => l.LeavingDateD == x.ToD && x.PersonId == l.PersonId).Any())
                .Select(x => x.PersonId);

            var personIds = p1.Union(p2).Union(p3).Distinct().ToArray();

            foreach (var personId in personIds)
            {
                // die relevanten Einträge der Vorgängermeldung
                var predStays = data.Predecessor.Stays.Where(x => x.PersonId == personId).ToArray();

                // die relevanten Einträge der aktuellen Meldung
                var actualStays = data.Report.Stays.Where(x => x.PersonId == personId && x.FromD < data.Report.FromD).ToArray();

                if (actualStays.Any() && predStays.Any())
                {
                    //bei einem Aufenhalt über den Meldezeitraum hinaus, ist das To des letzten Aufenthaltes des Vorgängerberichts nicht gesetzt.
                    //um den Vergleich einfach zu machen, wird das To des entsprecheden ersten Aufenthalts der aktuellen Meldung ebenfalls auf null gesetzt.

                    var index = actualStays.Length - 1;

                    if (!predStays.Last().ToD.HasValue && actualStays[index].ToD.HasValue)
                    {
                        actualStays[index] = new Stay(actualStays[index])
                        {
                            ToD = null
                        };
                    }
                }

                // verglichen werden die beiden GroupedStays 
                GroupedStay stays1;
                GroupedStay stays2;
                try
                {
                    stays1 = predStays.GetGroupedStays().LastOrDefault();
                    stays2 = actualStays.GetGroupedStays().FirstOrDefault();
                }
                catch (Exception e)
                {
                    ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Stays)}", e.Message));
                    return;
                }


                if (stays1 == null || stays2 == null || !stays1.Equals(stays2))
                {
                    var person = data.Report.Persons.Where(x => x.Id == personId).FirstOrDefault();

                    var index = person != null ? data.Report.Persons.IndexOf(person) : -1;
                    var name = person != null ? data.Report.GetPersonName(personId) : data.Predecessor.GetPersonName(personId);

                    if (stays2 == null)
                    {
                        ctx.AddFailure(new ValidationFailure($"{nameof(StatLpReport.Persons)}[{index}]",
                             $"Aufenthalte von '{name}' wurden letztes Jahr gemeldet. Sie fehlen in diesem Jahr"));
                    }
                    else if (stays1 == null)
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
