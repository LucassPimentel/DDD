using ServiceOrder.Domain.Entities;
using ServiceOrder.Domain.Interfaces;
using ServiceOrder.Domain.ValueObjects;

namespace ServiceOrder.Infrastructure.Database.Seeder
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ServiceOrderDatabaseContext _serviceOrderDatabaseContext;
        private readonly TechnicianDatabaseContext _technicianDatabaseContext;

        public DataSeeder(ServiceOrderDatabaseContext serviceOrderDatabaseContext, TechnicianDatabaseContext technicianDatabaseContext)
        {
            _serviceOrderDatabaseContext = serviceOrderDatabaseContext;
            _technicianDatabaseContext = technicianDatabaseContext;
        }

        public async Task SeedAsync()
        {
            if (_serviceOrderDatabaseContext.ServiceOrders.Any())
                return;

            await AddServiceOrders();
            await AddTechnicians();

            await _serviceOrderDatabaseContext.SaveChangesAsync();
            await _technicianDatabaseContext.SaveChangesAsync();
        }

        private async Task AddServiceOrders()
        {
            await _serviceOrderDatabaseContext.ServiceOrders.AddRangeAsync(
                new Domain.Entities.ServiceOrder(
                    null,
                    new ServiceOrderAddress("Rua das Acácias", "123", "12345-678", "São Paulo", "SP", "Brasil"),
                    "Instalação de internet residencial",
                    ServiceType.Normal
                ),
                new Domain.Entities.ServiceOrder(
                    1,
                    new ServiceOrderAddress("Rua 2", "2", "23456-789", "Rio de Janeiro", "RJ", "Brasil"),
                    "Reparo de conexão",
                    ServiceType.Urgent
                )
            );
        }

        private async Task AddTechnicians()
        {
            await _technicianDatabaseContext.Technician.AddRangeAsync(
                new Technician(1, "João Silva", "emailteste@gmail.com", null),
                new Technician(2, "Maria Oliveira", "maria.oliveira@gmail.com", new List<int> { 1 })
                );
        }
    }
}
