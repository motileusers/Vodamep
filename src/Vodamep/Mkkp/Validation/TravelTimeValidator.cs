using System;
using FluentValidation;
using Vodamep.Mkkp.Model;


namespace Vodamep.Mkkp.Validation
{
    internal class TravelTimeValidator : AbstractValidator<TravelTime>
    {
        public TravelTimeValidator()
        {
            this.RuleFor(x => x.Minutes).GreaterThan(0);
        }

    }
}