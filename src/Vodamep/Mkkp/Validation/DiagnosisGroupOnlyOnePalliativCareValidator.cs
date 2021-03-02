using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Vodamep.Mkkp.Model;
using Vodamep.ValidationBase;

namespace Vodamep.Mkkp.Validation
{
    internal class DiagnosisGroupOnlyOnePalliativCareValidator : AbstractValidator<Person>
    {
        public DiagnosisGroupOnlyOnePalliativCareValidator()
        {
            RuleFor(x => x.Diagnoses)
                .Custom((list, ctx) =>
                {
                    var palliativeItemsCount = list.Count(x => x == DiagnosisGroup.PalliativeCare1 ||
                                                               x == DiagnosisGroup.PalliativeCare2 ||
                                                               x == DiagnosisGroup.PalliativeCare3 ||
                                                               x == DiagnosisGroup.PalliativeCare4);

                    if (palliativeItemsCount > 1)
                    { 
                        ctx.AddFailure(new ValidationFailure(nameof(Person.Diagnoses),
                            Validationmessages.OnlyONePalliativeDiagnosisGroup));
                    }
                });
        }
    }
}