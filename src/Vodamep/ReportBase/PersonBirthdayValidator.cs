using FluentValidation;
using System;
using Vodamep.ValidationBase;

namespace Vodamep.ReportBase.Validation
{

    internal class PersonBirthdayValidator : AbstractValidator<IPerson>
    {
        public PersonBirthdayValidator(DateTime earliestBirthday)
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            this.RuleFor(x => x.BirthdayD).NotEmpty();

            this.RuleFor(x => x.Birthday).SetValidator(new TimestampWithOutTimeValidator());

            RuleFor(x => x.BirthdayD)
                .LessThan(DateTime.Today)
                .Unless(x => x.Birthday == null)
                .WithMessage(x => Validationmessages.ReportBaseBirthdayNotInFuture(x.Id));

            RuleFor(x => x.BirthdayD)
                .GreaterThanOrEqualTo(earliestBirthday)
                .Unless(x => x.Birthday == null)
                .WithMessage(x => Validationmessages.ReportBaseBirthdayNotInF2uture(x.Id, earliestBirthday));

        }

    }
}
