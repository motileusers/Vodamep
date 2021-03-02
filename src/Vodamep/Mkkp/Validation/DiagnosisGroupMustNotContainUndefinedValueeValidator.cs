using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class DiagnosisGroupMustNotContainUndefinedValueeValidator : AbstractValidator<Person>
    {
        public DiagnosisGroupMustNotContainUndefinedValueeValidator()
        {
            RuleFor(x => x.Diagnoses)
                .Custom((list, ctx) =>
                {
                    var palliativeItemsCount = list.Count(x => x == DiagnosisGroup.UndefinedDiagnosisGroup);

                    if (palliativeItemsCount > 0)
                    {
                        ctx.AddFailure(new ValidationFailure(nameof(Person.Diagnoses),
                            Validationmessages.AtLeastOneDiagnosisGroup));
                    }
                });
        }
    }
}