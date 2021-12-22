using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Agp.Model;
using Vodamep.ReportBase;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class StaffActivityValidator : AbstractValidator<StaffActivity>
    {
        public StaffActivityValidator(AgpReport report)
        {
            var displayNameResolver = new AgpDisplayNameResolver();

            this.RuleFor(x => x.StaffId).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.StaffId)), displayNameResolver.GetDisplayName(nameof(Activity)), report.GetStaffName(x.StaffId)));
            this.RuleFor(x => x.ActivityType).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.ActivityType)), displayNameResolver.GetDisplayName(nameof(Activity)), report.GetStaffName(x.StaffId)));
            this.RuleFor(x => x.Date).NotEmpty()
                .WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(x.Date)), displayNameResolver.GetDisplayName(nameof(Activity)), report.GetStaffName(x.StaffId)));

            this.RuleFor(x => x.DateD)
                .SetValidator(x => new DateTimeValidator(displayNameResolver.GetDisplayName(nameof(x.Date)),
                    "", report.GetStaffName(x.StaffId), report.FromD, report.ToD, x.Date));

            this.RuleFor(x => x).SetValidator(x => new StaffActivityMinutesValidator(displayNameResolver.GetDisplayName(nameof(Activity.Minutes)), report.GetStaffName(x.StaffId)));

        }
    }
}