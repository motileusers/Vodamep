﻿using System;
using FluentValidation;
using Vodamep.ReportBase;

namespace Vodamep.ValidationBase
{

    internal class PersonBirthdayValidator : AbstractValidator<IPerson>
    {
        public PersonBirthdayValidator(DateTime earliestBirthday)
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;

            this.RuleFor(x => x.BirthdayD).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(x.Id));

            this.RuleFor(x => x.Birthday)
                .SetValidator(new TimestampWithOutTimeValidator()).WithMessage(x => Validationmessages.ReportBaseDateMustnotHaveTime(x.Id));

            RuleFor(x => x.BirthdayD)
                .LessThan(DateTime.Today)
                .Unless(x => x.Birthday == null)
                .WithMessage(x => Validationmessages.ReportBaseBirthdayNotInFuture(x.Id));

            RuleFor(x => x.BirthdayD)
                .GreaterThanOrEqualTo(earliestBirthday)
                .Unless(x => x.Birthday == null)
                .WithMessage(x => Validationmessages.ReportBaseBirthdayMustNotBeBefore(x.Id));

        }

    }
}
