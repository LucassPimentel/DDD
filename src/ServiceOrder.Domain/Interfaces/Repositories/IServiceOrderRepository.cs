namespace ServiceOrder.Domain.Interfaces.Repositories
{
    using System.Collections.Generic;
    using ServiceOrder.Domain.Entities;

    public interface IServiceOrderRepository
    {
        Task<IEnumerable<ServiceOrder>> GetAllAsync();
        Task<ServiceOrder?> GetByIdAsync(int id);
        Task<int> AddAsync(ServiceOrder serviceOrder);
        Task SaveChangesAsync();
        void Update(ServiceOrder serviceOrderToInitialize);
    }
}
