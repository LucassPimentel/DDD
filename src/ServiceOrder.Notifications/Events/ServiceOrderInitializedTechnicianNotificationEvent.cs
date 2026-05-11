namespace ServiceOrder.Notifications.Worker.Events
{
    public sealed class ServiceOrderInitializedTechnicianNotificationEvent
    {
        public string Email { get; set; } = default!;
        public string ServiceOrderTypeName { get; set; } = default!;
        public int ServiceOrderId { get; set; }
        public string Name { get; set; } = default!;
    }
}
