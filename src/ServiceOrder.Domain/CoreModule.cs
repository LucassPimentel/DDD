using ServiceOrder.Domain.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceOrder.Domain
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
