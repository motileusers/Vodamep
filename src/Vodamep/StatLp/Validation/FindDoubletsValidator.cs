using FluentValidation;
using System;
using System.Linq;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{
    internal class FindDoubletsValidator : AbstractValidator<StatLpReport>
    {
        public FindDoubletsValidator()
        {
            this.RuleFor(x => x)
                 .Custom((Action<StatLpReport, ValidationContext<StatLpReport>>)((report, ctx) =>
                 {
                     //mögliche Dubletten identifizieren.
                     var aliases = report.CreatePatientIdMap();

                     if (!aliases.Any())
                         return;

                     report = report.ApplyPersonIdMap(aliases);

                     //genauer hinschauen:
                     foreach (var personId in aliases.Select(x => x.Value).Distinct())
                     {
                         //Dubletten dürfen keine Aufenhalte haben, die sich überschneiden
                         var stays = report.Stays.Where((Func<Stay, bool>)(x => x.PersonId == personId)).ToArray();

                         try
                         {
                             stays.GetGroupedStays(GroupedStay.SameTypeGroupMode.Ignore).ToArray();
                         }
                         catch (Exception e)
                         {
                             var person = report.Persons.Where(x => x.Id == personId).FirstOrDefault();
                             ctx.AddFailure($"'{person?.FamilyName} {person?.GivenName}' wurde mehrfach gemeldet. {e.Message}");
                         }
                     }
                 }));
        }
    }
}
