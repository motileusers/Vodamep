using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Agp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Agp.Validation
{
    internal class DiagnosisGroupIsUniqueValidator : AbstractValidator<Person>
    {
        public DiagnosisGroupIsUniqueValidator()
        {
            RuleFor(x => x.Diagnoses)
                .Custom((list, ctx) =>
                {
                    var duplicates = list
                        .GroupBy(x => x)
                        .Where(x => x.Count() > 1);

                    foreach (var entry in duplicates)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(Person.Diagnoses), Validationmessages.DoubledDiagnosisGroups));
                    }
                });
        }

    }
}