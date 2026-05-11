namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Interfaces.Repositories
{
    using System.Collections.Generic;
    using DDD.ServiceOrder.Core.Entities;

    public interface IServiceOrderRepository
    {
        Task<IEnumerable<ServiceOrder>> GetAllAsync();
        Task<ServiceOrder?> GetByIdAsync(int id);
        Task<int> AddAsync(ServiceOrder serviceOrder);
        Task SaveChangesAsync();
        void Update(ServiceOrder serviceOrderToInitialize);
    }
}
