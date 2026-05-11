using ServiceOrder.Domain.Notification;

namespace ServiceOrder.Application.Extensions
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
