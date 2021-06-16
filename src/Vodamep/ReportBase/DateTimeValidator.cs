using System;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Vodamep.ValidationBase;

namespace Vodamep.ReportBase
{
    internal class DateTimeValidator : AbstractValidator<DateTime>
    {
        public DateTimeValidator(string propertyName, string client, DateTime from, DateTime to, Timestamp timestamp)
        {
            this.RuleFor(x => x)
                .SetValidator(new TimestampWithOutTimeValidator(propertyName, client))
                .Unless(x => timestamp == null);


            if (from != DateTime.MinValue)
            {
                this.RuleFor(x => x).GreaterThanOrEqualTo(from).Unless(x => timestamp == null)
                    .WithMessage(x => Validationmessages.ReportBaseActivityDateMustBeWithinReport(propertyName, client));
            }
            if (to > from)
            {
                this.RuleFor(x => x).LessThanOrEqualTo(to).Unless(x => timestamp == null)
                    .WithMessage(x => Validationmessages.ReportBaseActivityDateMustBeWithinReport(propertyName, client));
            }
        }
    }
}