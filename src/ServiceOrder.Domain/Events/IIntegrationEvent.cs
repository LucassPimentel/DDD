namespace ServiceOrder.Domain.Events;

public interface IIntegrationEvent
{
    string RoutingKey { get; }
}
