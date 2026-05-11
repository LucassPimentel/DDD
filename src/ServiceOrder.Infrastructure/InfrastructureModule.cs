using ServiceOrder.Application.Interfaces.EventBus;
using ServiceOrder.Domain.Interfaces.Repositories;
using ServiceOrder.Infrastructure.Database;
using ServiceOrder.Infrastructure.Database.Seeder;
using ServiceOrder.Infrastructure.EventBus;
using ServiceOrder.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceOrder.Domain.Interfaces;

namespace ServiceOrder.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddServices();
            services.AddInMemoryDatabase();

            return services;
        }

        private static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
        {
            services.AddDbContext<ServiceOrderDatabaseContext>(opt => opt.UseInMemoryDatabase("ServiceOrderDb"));
            services.AddDbContext<TechnicianDatabaseContext>(opt => opt.UseInMemoryDatabase("TechnicianDb"));
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
            services.AddScoped<ITechnicianRepository, TechnicianRepository>();
            services.AddScoped<IDataSeeder, DataSeeder>();

            services.AddSingleton<IEventBusRabbitMq, EventBusRabbitMq>();
            return services;
        }
    }
}
