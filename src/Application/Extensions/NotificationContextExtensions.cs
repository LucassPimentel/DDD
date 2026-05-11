using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Notification;

namespace DDD.ServiceOrder.Api.Application.Extensions
{
    public static class NotificationContextExtensions
    {
        public static bool NotifyIfNull<T>(
        this NotificationContext context,
        T? value,
        string key,
        string message)
        where T : class
        {
            if (value is not null) return false;

            context.AddNotification(key, message);
            return true;
        }
    }
}
