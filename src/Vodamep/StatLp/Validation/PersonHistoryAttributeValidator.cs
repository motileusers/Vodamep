using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class PersonHistoryAttributeValidator : AbstractValidator<StatLpReportHistory>
    {
        private static DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public PersonHistoryAttributeValidator()
        {
            this.RuleFor(x => x).Custom((a, ctx) =>
            {
                var sendMessage = a.StatLpReport;


                var groupedAttributes = sendMessage.Attributes.GroupBy(x => (x.PersonId, x.FromD, x.AttributeType))
                                                    .Select(g => (g.Key.PersonId, g.Key.FromD, g.Key.AttributeType, Count: g.Count()));

                foreach (var group in groupedAttributes)
                {
                    if (group.Count > 1)
                    {
                        ctx.AddFailure(Validationmessages.StatLpReportPersonHistoryAdmissionAttributeMultipleChanged(group.PersonId, group.FromD.ToShortDateString(), group.AttributeType.ToString()));
                    }
                }
            });
        }
    }
}