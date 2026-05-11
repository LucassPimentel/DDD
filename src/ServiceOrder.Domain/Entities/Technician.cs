using FluentValidation;

namespace ServiceOrder.Domain.Entities
{
    public class Technician : EntityBase
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public IEnumerable<int>? ServiceOrderIds { get; private set; }

        protected Technician() { }

        public void SetServiceOrderIds(IEnumerable<int>? serviceOrdersIds)
        {
            ServiceOrderIds = serviceOrdersIds;
        }

        public Technician(int id, string name, string email, IEnumerable<int>? serviceOrdersIds)
        {
            Id = id;
            Name = name;
            Email = email;
            ServiceOrderIds = serviceOrdersIds;

            Validate(this, new TechnicianValidator());
        }
    }

    public class TechnicianValidator : AbstractValidator<Technician>
    {
        public TechnicianValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");
        }
    }

}
