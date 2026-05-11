using DDD.ServiceOrder.Api.DDD.ServiceOrder.Core.Events;

namespace DDD.ServiceOrder.Api.Application.Interfaces.EventBus
{
    public interface IEventBusRabbitMq
    {
        Task PublishAsync(IIntegrationEvent @event, CancellationToken ct = default);
        Task PublishAsync(object data, string routingKey, CancellationToken ct = default);
    }
}
