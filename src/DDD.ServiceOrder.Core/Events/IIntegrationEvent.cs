namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Events;

public interface IIntegrationEvent
{
    string RoutingKey { get; }
}
