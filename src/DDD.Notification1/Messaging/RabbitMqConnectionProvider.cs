using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DDD.ServiceOrder.Api.DDD.Notification1.Messaging
{
    public sealed class RabbitMqConnectionProvider : IAsyncDisposable
    {
        private IConnection? _connection;
        private readonly RabbitMqOptions _options;

        public RabbitMqConnectionProvider(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IConnection> GetOpenConnectingAsync(CancellationToken ct)
        {
            if (_connection?.IsOpen == true)
                return _connection;

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = await factory.CreateConnectionAsync("notification-consumer", ct);
            return _connection;
        }

        public async Task<IChannel> CreateChannelAsync(CancellationToken ct)
        {
            var connection = await GetOpenConnectingAsync(ct);
            return await connection.CreateChannelAsync(null, ct);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null) 
                await _connection.CloseAsync();
            _connection?.Dispose();
        }
    }
}
