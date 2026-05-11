using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Enums;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.ValueObjects;
using FluentValidation;

namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Entities
{
    public class ServiceOrder : EntityBase
    {
        public ServiceOrderAddress ServiceOrderAddress { get; private set; }
        public ServiceType ServiceType { get; private set; }
        public DateTime OpenedAt { get; private set; }
        public DateTime InitializedAt { get; private set; }
        public DateTime CompletedAt { get; private set; }
        public string? Description { get; private set; }
        public int? TechnicianId { get; private set; }
        public StatusEnum Status { get; private set; }

        private ServiceOrder() { }
        public ServiceOrder(int? technicianId, ServiceOrderAddress serviceOrderAddress, string? description, ServiceType serviceType)
        {
            TechnicianId = technicianId;
            ServiceOrderAddress = serviceOrderAddress;
            Description = description;
            OpenedAt = DateTime.Now;
            ServiceType = serviceType;
            Status = StatusEnum.Pending;

            Validate(this, new ServiceOrderValidator());
        }

        public bool HasTechnician() => TechnicianId != null;

        public void AssignTechnician(int technicianId)
        {
            TechnicianId = technicianId;
        }

        public bool SetInitialized()
        {
            if (!Validate(this, new ServiceOrderInitializeValidator()))
                return false;

            Status = StatusEnum.InProgress;
            InitializedAt = DateTime.Now;
            return true;
        }

        public bool SetCompleted()
        {
            if (!Validate(this, new ServiceOrderCompleteValidator()))
                return false;

            Status = StatusEnum.Completed;
            CompletedAt = DateTime.Now;
            return true;
        }

        public void UpdateStatus(StatusEnum newStatus)
        {
            Status = newStatus;
        }
    }

    public sealed class ServiceOrderValidator : AbstractValidator<ServiceOrder>
    {
        public ServiceOrderValidator()
        {
            RuleFor(so => so.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(500).WithMessage("A descrição não pode exceder 500 caracteres.");

            RuleFor(so => so.ServiceOrderAddress)
                .NotNull().WithMessage("O endereço da ordem de serviço é obrigatório.")
                .SetValidator(new ServiceOrderAddressValidator());

            RuleFor(so => so.ServiceType)
                .NotNull().WithMessage("O tipo de serviço é obrigatório.")
                .SetValidator(new ServiceTypeValidator());
        }
    }

    public sealed class ServiceOrderInitializeValidator : AbstractValidator<ServiceOrder>
    {
        public ServiceOrderInitializeValidator()
        {
            RuleFor(so => so.Status)
                .Equal(StatusEnum.Pending).WithMessage("A ordem de serviço deve estar pendente para ser iniciada.");
            RuleFor(so => so.TechnicianId)
                .NotNull().WithMessage("A ordem de serviço deve ter um técnico atribuído para ser iniciada.");
        }
    }

    public sealed class ServiceOrderCompleteValidator : AbstractValidator<ServiceOrder>
    {
        public ServiceOrderCompleteValidator()
        {
            RuleFor(x => x.Status)
                .Equal(StatusEnum.InProgress)
                .WithMessage("A ordem de serviço deve estar em andamento para ser concluída.");
        }
    }
}