using FluentValidation;
using System.Text.RegularExpressions;
using Vodamep.StatLp.Model;

namespace Vodamep.StatLp.Validation
{
    internal class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.FamilyName).NotEmpty();
            this.RuleFor(x => x.GivenName).NotEmpty();

            // Änderung 5.11.2018, LH
            var r = new Regex(@"^[\p{L}][-\p{L}. ]*[\p{L}.]$");
            this.RuleFor(x => x.FamilyName).Matches(r).Unless(x => string.IsNullOrEmpty(x.FamilyName));
            this.RuleFor(x => x.GivenName).Matches(r).Unless(x => string.IsNullOrEmpty(x.GivenName));
            
            this.Include(new PersonBirthdayValidator());
       
            this.RuleFor(x => x.Gender).NotEmpty();
            this.RuleFor(x => x.Country).NotEmpty();


        }
    }
}
