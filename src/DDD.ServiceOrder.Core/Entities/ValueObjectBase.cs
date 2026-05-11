using FluentValidation;
using FluentValidation.Results;

namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Entities
{
    public abstract record ValueObjectBase
    {
        public bool Valid { get; private set; }
        public bool Invalid => !Valid;
        public ValidationResult ValidationResult { get; private set; }

        public bool Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            ValidationResult = validator.Validate(model);
            Valid = ValidationResult.IsValid;
            return Valid;
        }
    }
}
