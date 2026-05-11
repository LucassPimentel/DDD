namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Infrastructure.Repositories
{
    using DDD.ServiceOrder.Core.Entities;
    using DDD.ServiceOrder.Infrastructure.Database;
    using global::DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class TechnicianRepository : ITechnicianRepository
    {
        private readonly TechnicianDatabaseContext _technicianDatabaseContext;

        public TechnicianRepository(TechnicianDatabaseContext technicianDatabaseContext)
        {
            _technicianDatabaseContext = technicianDatabaseContext;
        }

        public async Task<Technician> AddAsync(Technician technician)
        {
            var addedTechnician = await _technicianDatabaseContext.AddAsync(technician);
            return addedTechnician.Entity;
        }

        public async Task<IEnumerable<Technician>> GetAllAsync()
        {
            return await _technicianDatabaseContext.Technician.ToListAsync();
        }

        public async Task<Technician?> GetByIdAsync(int id)
        {
            return await _technicianDatabaseContext.Technician.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Technician>> GetByIdsAsync(List<int> technicianIds)
        {
            return await _technicianDatabaseContext.Technician.Where(t => technicianIds.Contains(t.Id)).ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _technicianDatabaseContext.SaveChangesAsync();
        }
    }
}
