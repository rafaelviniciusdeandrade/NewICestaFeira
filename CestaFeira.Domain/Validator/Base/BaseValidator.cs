using FluentValidation;


namespace CestaFeira.Domain.Validator.Base
{
    public class BaseValidator<BaseClass> : AbstractValidator<BaseClass>
    {
        public BaseValidator()
        {
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}

