using RabbitMQ.Client;

namespace DDD.ServiceOrder.Api.DDD.Notification1.Infrastructure
{
    public class RabbitMqConnection
    {
        private readonly IConnection _connection;

        public RabbitMqConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory
                .CreateConnectionAsync("notification-worker")
                .Result;
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            return await _connection.CreateChannelAsync();
        }
    }
}
