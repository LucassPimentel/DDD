namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Infrastructure.Repositories
{
    using DDD.ServiceOrder.Infrastructure.Database;
    using DDD.ServiceOrder.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using global::DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Interfaces.Repositories;

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly ServiceOrderDatabaseContext _serviceOrderDatabaseContext;

        public ServiceOrderRepository(ServiceOrderDatabaseContext serviceOrderDatabaseContext)
        {
            _serviceOrderDatabaseContext = serviceOrderDatabaseContext;
        }

        public async Task<int> AddAsync(ServiceOrder serviceOrder)
        {
            var addedServiceOrder = await _serviceOrderDatabaseContext.AddAsync(serviceOrder);
            return addedServiceOrder.Entity.Id;
        }

        public async Task<IEnumerable<ServiceOrder>> GetAllAsync()
        {
            return await _serviceOrderDatabaseContext.ServiceOrders.ToListAsync();
        }

        public async Task<ServiceOrder?> GetByIdAsync(int id)
        {
            return await _serviceOrderDatabaseContext.ServiceOrders
                .FirstOrDefaultAsync(so => so.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _serviceOrderDatabaseContext.SaveChangesAsync();
        }

        public void Update(ServiceOrder serviceOrderToInitialize)
        {
            _serviceOrderDatabaseContext.ServiceOrders.Update(serviceOrderToInitialize);
        }
    }
}
