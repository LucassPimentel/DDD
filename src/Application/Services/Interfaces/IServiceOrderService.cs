using DDD.ServiceOrder.Api.Application.InputModels;
using DDD.ServiceOrder.Api.Application.ViewModels;

namespace DDD.ServiceOrder.Api.Application.Services.Interfaces
{
    public interface IServiceOrderService
    {
        Task<IEnumerable<ServiceOrderViewModel>?> GetAllWithTechnicianAsync();
        Task<ServiceOrderViewModel?> GetByIdAsync(int id);
        Task<int?> AddAsync(ServiceOrderInputModel serviceOrder);
        Task InitializeAsync(InitializeServiceInput initializeServiceInput);
    }
}