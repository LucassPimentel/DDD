using ServiceOrder.Application.InputModels;
using ServiceOrder.Application.ViewModels;

namespace ServiceOrder.Application.Services.Interfaces
{
    public interface IServiceOrderService
    {
        Task<IEnumerable<ServiceOrderViewModel>?> GetAllWithTechnicianAsync();
        Task<ServiceOrderViewModel?> GetByIdAsync(int id);
        Task<int?> AddAsync(ServiceOrderInputModel serviceOrder);
        Task InitializeAsync(InitializeServiceInput initializeServiceInput);
    }
}