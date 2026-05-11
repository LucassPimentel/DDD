using DDD.ServiceOrder.Api.Application.Services;
using DDD.ServiceOrder.Api.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.ServiceOrder.Api.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IServiceOrderService, ServiceOrderService>();
            return services;
        }
    }
}
