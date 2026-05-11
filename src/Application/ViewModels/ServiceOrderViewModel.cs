using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Entities;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Enums;

namespace DDD.ServiceOrder.Api.Application.ViewModels
{
    public class ServiceOrderViewModel
    {
        public int Id { get; set; }
        public ServiceOrderAddressViewModel? ServiceOrderAddress { get; set; }
        public ServiceTypeViewModel? ServiceType { get; set; }
        public DateTime OpenedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public TechnicianSummaryViewModel? Technician { get; set; }
        public StatusEnum Status { get; set; }

        public static ServiceOrderViewModel FromEntity(DDD.ServiceOrder.Core.Entities.ServiceOrder serviceOrder, Technician? technician)
        {
            return new ServiceOrderViewModel
            {
                Id = serviceOrder.Id,
                ServiceOrderAddress = new ServiceOrderAddressViewModel
                {
                    Street = serviceOrder.ServiceOrderAddress.Street,
                    City = serviceOrder.ServiceOrderAddress.City,
                    State = serviceOrder.ServiceOrderAddress.State,
                    ZipCode = serviceOrder.ServiceOrderAddress.ZipCode
                },
                ServiceType = new ServiceTypeViewModel { Name = serviceOrder.ServiceType.Name, HoursToAttend = serviceOrder.ServiceType.HoursToAttend },
                OpenedAt = serviceOrder.OpenedAt,
                Description = serviceOrder.Description,
                Technician = technician != null ? new TechnicianSummaryViewModel { Id = technician.Id, Name = technician.Name } : null,
                Status = serviceOrder.Status
            };
        }
    }
}
