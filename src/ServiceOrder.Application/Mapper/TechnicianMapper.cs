using ServiceOrder.Application.InputModels;
using ServiceOrder.Application.ViewModels;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Application.Mapper
{
    public static class TechnicianMapper
    {
        public static TechnicianViewModel ToViewModel(Technician technician, IEnumerable<ServiceOrderViewModel>? serviceOrders = null)
        {
            return new TechnicianViewModel
            {
                Id = technician.Id,
                Name = technician.Name,
                Email = technician.Email,
                ServiceOrders = serviceOrders ?? new List<ServiceOrderViewModel>()
            };
        }

        public static TechnicianSummaryViewModel ToSummaryViewModel(Technician technician)
        {
            return new TechnicianSummaryViewModel
            {
                Id = technician.Id,
                Name = technician.Name
            };
        }

        public static Technician ToEntity(TechnicianInputModel input)
        {
            return new Technician(0, input.Name, input.Email, null);
        }
    }
}