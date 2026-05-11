using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core
{
    public static class CoreModule
    {

        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<NotificationContext>();
            return services;
        }
    }
}
