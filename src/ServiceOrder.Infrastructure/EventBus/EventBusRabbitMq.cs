using ServiceOrder.Application.Interfaces.EventBus;
using ServiceOrder.Domain.Events;
using RabbitMQ.Client;
using System.Text.Json;

namespace ServiceOrder.Infrastructure.EventBus
{
    public class EventBusRabbitMq : IEventBusRabbitMq
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private bool _initialized;

        private const string Exchange = "serviceorder-service";

        private async Task EnsureInitializedAsync(CancellationToken ct)
        {
            if (_initialized) return;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken: ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

            await _channel.ExchangeDeclareAsync(
                exchange: Exchange,
                type: ExchangeType.Direct,
                durable: true,
                cancellationToken: ct);

            _initialized = true;
        }

        public Task PublishAsync(IIntegrationEvent @event, CancellationToken ct = default)
            => PublishAsync(@event, @event.RoutingKey, ct);

        public async Task PublishAsync(object data, string routingKey, CancellationToken ct = default)
        {
            await EnsureInitializedAsync(ct);

            var payload = JsonSerializer.SerializeToUtf8Bytes(data);

            await _channel!.BasicPublishAsync(
                exchange: Exchange,
                routingKey: routingKey,
                mandatory: true,
                body: payload,
                cancellationToken: ct);
        }
    }
}