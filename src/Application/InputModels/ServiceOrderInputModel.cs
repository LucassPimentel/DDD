using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Enums;

namespace DDD.ServiceOrder.Api.Application.InputModels
{
    public class ServiceOrderInputModel
    {
        public ServiceOrderAddressInputModel ServiceOrderAddress { get; set; }
        public ServiceTypeEnum? ServiceType { get; set; }
        public string? Description { get; set; }
        public int? TechnicianId { get; set; }
    }
}
