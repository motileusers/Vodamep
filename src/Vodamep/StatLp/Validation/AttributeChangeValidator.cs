using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Vodamep.StatLp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.StatLp.Validation
{
    internal class AttributeChangeValidator : AbstractValidator<StatLpReport>
    {
        private static DisplayNameResolver displayNameResolver = new DisplayNameResolver();

        public AttributeChangeValidator()
        {
            this.RuleFor(x => x).Custom((a, ctx) =>
            {
                var sentMessage = a;


                var groupedAttributes = sentMessage.Attributes.GroupBy(x => (x.PersonId, x.FromD, x.AttributeType))
                                                    .Select(g => (g.Key.PersonId, g.Key.FromD, g.Key.AttributeType, Count: g.Count()));

                foreach (var group in groupedAttributes)
                {
                    if (group.Count > 1)
                    {
                        ctx.AddFailure(Validationmessages.StatLpAttributeMultipleChanged(sentMessage.GetPersonName(group.PersonId), group.FromD.ToShortDateString(), group.AttributeType.ToString()));
                    }
                }
            });
        }
    }
}