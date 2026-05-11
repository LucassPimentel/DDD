using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Entities;

namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Interfaces.Repositories
{
    public interface ITechnicianRepository
    {
        Task<IEnumerable<Technician>> GetAllAsync();
        Task<Technician> AddAsync(Technician technician);
        Task<Technician?> GetByIdAsync(int id);
        Task<IEnumerable<Technician>> GetByIdsAsync(List<int> technicianIds);
    }
}
