using System;
using FluentValidation;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class PersonBirthdayValidator : AbstractValidator<Person>
    {
        public PersonBirthdayValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Birthday)
                .NotEmpty();

            RuleFor(x => x.Birthday)
                .SetValidator(new TimestampWithOutTimeValidator());

            RuleFor(x => x.BirthdayD)
                .LessThan(DateTime.Today)
                .Unless(x => x.Birthday == null)
                .WithMessage(Validationmessages.BirthdayNotInFuture);

            RuleFor(x => x.BirthdayD)
                .GreaterThanOrEqualTo(new DateTime(1900, 01, 01))
                .Unless(x => x.Birthday == null);
        }
    }
}