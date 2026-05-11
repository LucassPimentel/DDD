using ServiceOrder.Domain.Events;

namespace ServiceOrder.Application.Interfaces.EventBus
{
    public interface IEventBusRabbitMq
    {
        Task PublishAsync(IIntegrationEvent @event, CancellationToken ct = default);
        Task PublishAsync(object data, string routingKey, CancellationToken ct = default);
    }
}
