using FluentValidation.Validators;
using Vodamep.Data;

namespace Vodamep.ValidationBase
{
    internal class CodeValidator<T> : PropertyValidator
        where T: CodeProviderBase
    {
        public CodeValidator()
            : base(Validationmessages.InvalidCode)
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {

            string test = typeof(T).Name;

            if (test == "QualificationCodeProvider")
            {
            }

            var code = context.PropertyValue as string;

            if (string.IsNullOrEmpty(code)) return true;

            var provider = CodeProviderBase.GetInstance<T>();

            bool isValid = provider.IsValid(code);

            return isValid;
        }
    }
}
