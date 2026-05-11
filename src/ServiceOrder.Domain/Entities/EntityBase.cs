using FluentValidation;
using FluentValidation.Results;

namespace ServiceOrder.Domain.Entities
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
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
