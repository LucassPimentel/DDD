namespace DDD.ServiceOrder.Api.DDD.Notification1.Events
{
    public sealed class ServiceOrderInitializedTechnicianNotificationEvent
    {
        public string Email { get; set; } = default!;
        public string ServiceOrderTypeName { get; set; } = default!;
        public int ServiceOrderId { get; set; }
        public string Name { get; set; } = default!;
    }
}
