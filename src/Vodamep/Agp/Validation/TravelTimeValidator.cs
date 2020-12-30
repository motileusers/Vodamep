using System;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class TravelTimeValidator : AbstractValidator<TravelTime>
    {
        public TravelTimeValidator()
        {
            this.RuleFor(x => x.Minutes).GreaterThan(0);
        }

    }
}