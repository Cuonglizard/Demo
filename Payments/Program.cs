using System.Runtime.InteropServices;
using Payments.Application.EventHandlers;

using Common.Protobuf;
using MicroRabbit.Domain.Core.Bus;
using Payments.Application.Events;
using Microsoft.EntityFrameworkCore;
using Payments.Infrastructure;
using Payments.Models;
using Payments.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddDbContext<AppDbContext>(options =>

    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.AddApplicationServices();

// Add services to the container.
// NOTE: Be aware of this, could contain sensitive data!
builder.Services.AddGrpc(o => o.EnableDetailedErrors = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var eventBus = services.GetRequiredService<IEventBus>();

    eventBus.Subscribe<ORDEREvent, ORDEREventHandler>();
    // ??ng ký các s? ki?n khác n?u c?n
}
app.MapGrpcService<PaymentRpcService>();

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
