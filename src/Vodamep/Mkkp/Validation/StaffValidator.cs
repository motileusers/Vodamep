using FluentValidation;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class StaffValidator : AbstractValidator<Staff>
    {
        public StaffValidator()
        {
            MkkpDisplayNameResolver displayNameResolver = new MkkpDisplayNameResolver();


            this.RuleFor(x => x.Id).NotEmpty().WithMessage(x => Validationmessages.ReportBaseValueMustNotBeEmpty(displayNameResolver.GetDisplayName(nameof(Staff)), x.GetDisplayName())); ;

            this.RuleFor(x => x).SetValidator(new StaffNameValidator( displayNameResolver.GetDisplayName(nameof(Staff)),@"^[\p{L}][-\p{L} ]*[\p{L}]$", -1, -1, -1, -1));
        }
    }
}
