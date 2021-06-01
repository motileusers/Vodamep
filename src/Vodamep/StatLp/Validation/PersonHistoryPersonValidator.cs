using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryPersonValidator : AbstractValidator<StatLpReportHistory>
    {
        private DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public PersonHistoryPersonValidator()
        {
            this.RuleFor(x => x).Custom((a, ctx) =>
            {
                // Personen im aktuellen Report
                List<string> personIds = a.StatLpReport.Persons.Select(b => b.Id).ToList();

                // Personen in allen Reports
                List<Person> personListFull = a.StatLpReports.SelectMany(x => x.Persons).ToList();
                List<string> personIdsFull = personListFull.Select(b => b.Id).Distinct().ToList();

                if (a.StatLpReports.Count() > 100)
                {
              
                
                }


                var messages = new List<StatLpReport>();
                messages.Add(a.StatLpReport);
                messages.AddRange(a.StatLpReports);

                foreach (string personId in personIdsFull)
                {
                    var changedBirthdays = messages.SelectMany(p => p.Persons).Where(q => q.Id == personId)
                        .GroupBy(r => r.BirthdayD);

                    if (changedBirthdays.Count() > 1)
                    {
                        ctx.AddFailure(nameof(Person.BirthdayD),
                            Validationmessages.StatLpReportPersonHistoryGenderAttributeChanged(
                                displayNameResolver.GetDisplayName(nameof(Person.BirthdayD)), personId,
                                a.StatLpReport.FromD.ToShortDateString()));
                    }

                    var changedGenders = messages.SelectMany(p => p.Admissions).Where(q => q.PersonId == personId)
                        .GroupBy(r => r.Gender);

                    if (changedGenders.Count() > 1)
                    {
                        {
                            ctx.AddFailure(nameof(Admission.Gender),
                                Validationmessages.StatLpReportPersonHistoryGenderAttributeChanged(
                                    displayNameResolver.GetDisplayName(nameof(Admission.Gender)), personId,
                                    a.StatLpReport.FromD.ToShortDateString()));
                        }
                    }
                }
            });
        }
    }
}