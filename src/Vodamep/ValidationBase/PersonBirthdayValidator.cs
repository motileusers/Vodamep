using System;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{

    internal class PersonBirthdayValidator : AbstractValidator<IPerson>
    {
        public PersonBirthdayValidator(DateTime earliestBirthday, string clientOrStaff)
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            this.RuleFor(x => x.BirthdayD).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.GetDisplayName()));

            this.RuleFor(x => x.Birthday)
                .SetValidator(new TimestampWithOutTimeValidator()).WithMessage(x => Validationmessages.ReportBaseDateMustNotHaveTime(x.GetDisplayName(), clientOrStaff));

            RuleFor(x => x.BirthdayD)
                .LessThan(DateTime.Today)
                .Unless(x => x.Birthday == null)
                .WithMessage(x => Validationmessages.ReportBaseBirthdayNotInFuture(x.GetDisplayName(), clientOrStaff));

            RuleFor(x => x.BirthdayD)
                .GreaterThanOrEqualTo(earliestBirthday)
                .Unless(x => x.Birthday == null)
                .WithMessage(x => Validationmessages.ReportBaseBirthdayMustNotBeBefore(x.GetDisplayName(), clientOrStaff));

        }

    }
}
