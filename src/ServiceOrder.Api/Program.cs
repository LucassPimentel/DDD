using Scalar.AspNetCore;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.Mvc;
using ServiceOrder.Api.Filters;
using ServiceOrder.Application;
using ServiceOrder.Infrastructure;
using ServiceOrder.Domain;
using ServiceOrder.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
    });

builder.Services
    .AddInfrastructure()
    .AddApplication()
    .AddCore();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddMvc(opt => opt.Filters.Add<NotificationFilter>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthorization();

app.UseSwagger();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/DDD.ServiceOrder")
    {
        context.Response.Redirect("/scalar");
        return;
    }

    await next();
});

app.MapScalarApiReference(options =>
{
    options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
});

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await dataSeeder.SeedAsync();
}

app.MapControllers();

app.Run();