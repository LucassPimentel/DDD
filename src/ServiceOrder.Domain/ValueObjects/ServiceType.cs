using ServiceOrder.Domain.Entities;
using FluentValidation;

namespace ServiceOrder.Domain.ValueObjects
{
    public record ServiceType : ValueObjectBase
    {
        public string Name { get; private set; }
        public int HoursToAttend { get; private set; }

        protected ServiceType() { }
        public ServiceType(string name, int hoursToAttend)
        {
            Name = name;
            HoursToAttend = hoursToAttend;

            Validate(this, new ServiceTypeValidator());
        }

        public static readonly ServiceType Normal = new("Padrão", 72);
        public static readonly ServiceType Urgent = new("Urgente", 24);
    }

    public class ServiceTypeValidator : AbstractValidator<ServiceType>
    {
        public ServiceTypeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome é obrigatório.");
            RuleFor(x => x.HoursToAttend).GreaterThan(0).WithMessage("Horas para atendimento devem ser maiores que 0.");
        }
    }
}
