namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Events
{
    public class ServiceOrderInitialized : IIntegrationEvent
    {
        public const string EventRoutingKey = "service-order.initialized";
        public string RoutingKey => EventRoutingKey;

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string ServiceOrderTypeName { get; private set; }
        public int ServiceOrderId { get; private set; }

        public ServiceOrderInitialized(string email, string serviceOrderTypeName, int serviceOrderId, string technicianName)
        {
            Email = email;
            ServiceOrderTypeName = serviceOrderTypeName;
            ServiceOrderId = serviceOrderId;
            Name = technicianName;
        }
    }
}
