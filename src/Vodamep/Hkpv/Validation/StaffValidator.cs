using FluentValidation;
using System;
using System.Text.RegularExpressions;
using Vodamep.Data;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class StaffValidator : AbstractValidator<Staff>
    {
        public StaffValidator(DateTime from, DateTime to)
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();

            // Änderung 5.11.2018, LH
            var r = new Regex(@"^[\p{L}][-\p{L} ]*[\p{L}]$");
            this.RuleFor(x => x.FamilyName).Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName));

            // keine Einschränkungen für Vornamen: (z.B. Auszubildende 02) this.RuleFor(x => x.GivenName).Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName));

            // Änderung 27.11.2018, LH
            // Nicht für Bestandsdaten
            if (to < new DateTime(2019, 01, 01))
            {
                this.RuleFor(x => x.Qualification)
                .SetValidator(new CodeValidator<QualificationCodeProvider>())
                .Unless(x => string.IsNullOrEmpty(x.Qualification))
                .WithSeverity(Severity.Warning);
            }
            else
            {
                this.RuleFor(x => x.Qualification).NotEmpty();

                this.RuleFor(x => x.Qualification)
                .SetValidator(new CodeValidator<QualificationCodeProvider>())
                .Unless(x => string.IsNullOrEmpty(x.Qualification));
            }

        }
    }
}
