using ServiceOrder.Notifications.Worker.Consumers;
using ServiceOrder.Notifications.Worker.Messaging;
using Resend;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(opt =>
{
    opt.HostName = "localhost";
    opt.ExchangeName = "serviceorder-service";
    opt.ExchangeType = "direct";
});

var resendApiToken = builder.Configuration["RESEND_APITOKEN"]
    ?? throw new InvalidOperationException("Config 'RESEND_APITOKEN' não encontrada.");

builder.Services.Configure<ResendClientOptions>(options =>
{
    options.ApiToken = resendApiToken;
});

builder.Services.AddHttpClient<IResend, ResendClient>();

builder.Services.AddSingleton<RabbitMqConnectionProvider>();
builder.Services.AddHostedService<ServiceOrderinitializedTechnicianNotificationConsumer>();

var host = builder.Build();
host.Run();
