using System.Text;
using System.Text.Json;
using ServiceOrder.Notifications.Worker.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ServiceOrder.Notifications.Worker.Consumers;

public abstract class RabbitMqConsumerBase<T> : BackgroundService
{
    private readonly RabbitMqConnectionProvider _connectionProvider;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;
    private readonly RabbitMqOptions _options;

    private IChannel? _channel;

    protected RabbitMqConsumerBase(
        RabbitMqConnectionProvider connectionProvider,
        IServiceScopeFactory scopeFactory,
        ILogger logger,
        IOptions<RabbitMqOptions> options)
    {
        _connectionProvider = connectionProvider;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = options.Value;
    }

    protected abstract string QueueName { get; }
    protected abstract string RoutingKey { get; }

    protected abstract Task HandleAsync(T message, IServiceProvider services, CancellationToken ct);

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = await _connectionProvider.CreateChannelAsync(cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: _options.ExchangeType,
            durable: _options.Durable,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: _options.Durable,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await _channel.BasicQosAsync(0, _options.PrefetchCount, global: false, cancellationToken);

        await _channel.QueueBindAsync(
            queue: QueueName,
            exchange: _options.ExchangeName,
            routingKey: RoutingKey,
            arguments: null,
            cancellationToken: cancellationToken);

        _logger.LogInformation(
            "RabbitMQ consumidor pronto. Exchange={Exchange} Queue={Queue} RoutingKey={RoutingKey}",
            _options.ExchangeName, QueueName, RoutingKey);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null)
            throw new InvalidOperationException("Channel não inicializado!");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            using var scope = _scopeFactory.CreateScope();

            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var msg = JsonSerializer.Deserialize<T>(json);

                if (msg is null)
                    throw new JsonException($"Falha ao desserializar a mensagem para {typeof(T).Name}");

                await HandleAsync(msg, scope.ServiceProvider, stoppingToken);

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem do RabbitMQ. Fila={Queue}", QueueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        _channel?.Dispose();
        await base.StopAsync(cancellationToken);
    }
}