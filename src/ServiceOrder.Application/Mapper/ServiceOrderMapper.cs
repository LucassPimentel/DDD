using ServiceOrder.Application.InputModels;
using ServiceOrder.Application.ViewModels;
using ServiceOrder.Application.Mapper;
using ServiceOrder.Domain.ValueObjects;

namespace ServiceOrder.Application.Mapper
{
    public static class ServiceOrderMapper
    {
        public static ServiceOrderViewModel ToViewModel(ServiceOrder.Domain.Entities.ServiceOrder so, ServiceOrder.Domain.Entities.Technician? tech)
        {
            return new ServiceOrderViewModel
            {
                Id = so.Id,
                ServiceOrderAddress = MapAddress(so.ServiceOrderAddress),
                Description = so.Description,
                OpenedAt = so.OpenedAt,
                ServiceType = MapServiceType(so.ServiceType),
                Status = so.Status,
                Technician = tech != null ? TechnicianMapper.ToSummaryViewModel(tech) : null,
            };
        }

        public static ServiceOrder.Domain.Entities.ServiceOrder ToEntity(ServiceOrderInputModel input)
        {
            return new ServiceOrder.Domain.Entities.ServiceOrder(
                input.TechnicianId,
                input.ServiceOrderAddress.ToValueObject(),
                input.Description,
                input.ServiceType.ToEntity()
            );
        }

        private static ServiceOrderAddressViewModel? MapAddress(ServiceOrderAddress? address)
        {
            if (address == null) return null;

            return new ServiceOrderAddressViewModel
            {
                Street = address.Street,
                Number = address.Number,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode,
                Country = address.Country,

            };
        }

        private static ServiceTypeViewModel? MapServiceType(ServiceType? serviceType)
        {
            return serviceType != null
                ? new ServiceTypeViewModel { Name = serviceType.Name, HoursToAttend = serviceType.HoursToAttend }
                : null;
        }
    }
}
