using DDD.ServiceOrder.Api.Application.Interfaces.EventBus;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Interfaces.Repositories;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Infrastructure.Database;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Infrastructure.Database.Seeder;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Infrastructure.EventBus;
using DDD.ServiceOrder.Api.DDD.ServiceOrder.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Infrastructure
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
