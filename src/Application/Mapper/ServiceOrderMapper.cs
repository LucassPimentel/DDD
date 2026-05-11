using DDD.ServiceOrder.Api.Application.InputModels;
using DDD.ServiceOrder.Api.Application.ViewModels;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Entities.Mapper;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.ValueObjects;

namespace DDD.ServiceOrder.Api.Application.Mapper
{
    public static class ServiceOrderMapper
    {
        public static ServiceOrderViewModel ToViewModel(DDD.ServiceOrder.Core.Entities.ServiceOrder so, DDD.ServiceOrder.Core.Entities.Technician? tech)
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

        public static DDD.ServiceOrder.Core.Entities.ServiceOrder ToEntity(ServiceOrderInputModel input)
        {
            return new DDD.ServiceOrder.Core.Entities.ServiceOrder(
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
