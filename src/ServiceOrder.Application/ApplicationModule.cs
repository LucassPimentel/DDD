using ServiceOrder.Application.Services;
using ServiceOrder.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceOrder.Application
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
