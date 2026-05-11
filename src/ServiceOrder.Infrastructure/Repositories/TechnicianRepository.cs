using ServiceOrder.Infrastructure.Database;
using ServiceOrder.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using ServiceOrder.Domain.Entities;
namespace ServiceOrder.Infrastructure.Repositories
{

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
