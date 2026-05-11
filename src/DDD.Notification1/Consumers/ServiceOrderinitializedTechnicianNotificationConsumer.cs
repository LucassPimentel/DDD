using DDD.ServiceOrder.Api.DDD.Notification1.Events;
using DDD.ServiceOrder.Api.DDD.Notification1.Messaging;
using Microsoft.Extensions.Options;
using Resend;

namespace DDD.ServiceOrder.Api.DDD.Notification1.Consumers
{
    public class ServiceOrderinitializedTechnicianNotificationConsumer
        : RabbitMqConsumerBase<ServiceOrderInitializedTechnicianNotificationEvent>
    {
        protected override string QueueName => "service-order";
        protected override string RoutingKey => "service-order.initialized";

        public ServiceOrderinitializedTechnicianNotificationConsumer(
            RabbitMqConnectionProvider connectionProvider,
            IServiceScopeFactory scopeFactory,
            ILogger<ServiceOrderinitializedTechnicianNotificationConsumer> logger,
            IOptions<RabbitMqOptions> options)
            : base(connectionProvider, scopeFactory, logger, options)
        {
        }

        protected override async Task HandleAsync(
            ServiceOrderInitializedTechnicianNotificationEvent message,
            IServiceProvider services,
            CancellationToken ct)
        {
            var resend = services.GetRequiredService<IResend>();

            var email = new EmailMessage
            {
                From = "Acme <onboarding@resend.dev>",
                To = "onboarding@resend.dev", // aqui teria o email do tec
                Subject = "Ordem de serviço - Iniciada",
                TextBody = $"Olá, {message.Name}. A Ordem de serviço {message.ServiceOrderId} foi atribuída como iniciada para você."
            };

            await resend.EmailSendAsync(email, ct);
        }
    }
}
