using FluentValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Vodamep.Data;
using Vodamep.Hkpv.Model;

namespace Vodamep.Hkpv.Validation
{
    internal class EmploymentValidator : AbstractValidator<Employment>
    {
        public EmploymentValidator()
        {
            this.RuleFor(x => x.From).NotEmpty();
            this.RuleFor(x => x.To).NotEmpty();
         
            this.RuleFor(x => x.HoursPerWeek).ExclusiveBetween(0, 100).WithMessage(Validationmessages.EmploymentHoursPerWeekMustBeBetween0And100);
          
        }
    }
}
