using FluentValidation;
using FluentValidation.Validators;
using Vodamep.Data;

namespace Vodamep.ValidationBase
{
    internal class ValidCodeValidator<T,TProperty, TCode> : PropertyValidator<T, TProperty>
        where TCode : ValidCodeProviderBase
    {

        public override string Name => nameof(ValidCodeValidator<T, TProperty, TCode>);

        protected override string GetDefaultMessageTemplate(string errorCode) => Validationmessages.InvalidCode;

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            var code = value as string;

            if (string.IsNullOrEmpty(code)) return true;

            var provider = ValidCodeProviderBase.GetInstance<TCode>();

            bool isValid = provider.IsValid(code);

            return isValid;
        }
    }
}
