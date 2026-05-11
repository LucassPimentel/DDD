using ServiceOrder.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using ServiceOrder.Domain.Interfaces.Repositories;
namespace ServiceOrder.Infrastructure.Repositories
{

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly ServiceOrderDatabaseContext _serviceOrderDatabaseContext;

        public ServiceOrderRepository(ServiceOrderDatabaseContext serviceOrderDatabaseContext)
        {
            _serviceOrderDatabaseContext = serviceOrderDatabaseContext;
        }

        public async Task<int> AddAsync(Domain.Entities.ServiceOrder serviceOrder)
        {
            var addedServiceOrder = await _serviceOrderDatabaseContext.AddAsync(serviceOrder);
            return addedServiceOrder.Entity.Id;
        }

        public async Task<IEnumerable<Domain.Entities.ServiceOrder>> GetAllAsync()
        {
            return await _serviceOrderDatabaseContext.ServiceOrders.ToListAsync();
        }

        public async Task<Domain.Entities.ServiceOrder?> GetByIdAsync(int id)
        {
            return await _serviceOrderDatabaseContext.ServiceOrders
                .FirstOrDefaultAsync(so => so.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _serviceOrderDatabaseContext.SaveChangesAsync();
        }

        public void Update(Domain.Entities.ServiceOrder serviceOrderToInitialize)
        {
            _serviceOrderDatabaseContext.ServiceOrders.Update(serviceOrderToInitialize);
        }
    }
}
