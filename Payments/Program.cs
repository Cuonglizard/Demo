using System.Runtime.InteropServices;

using Common.Protobuf;

using Microsoft.EntityFrameworkCore;

using Payments.Models;
using Payments.Services;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGrpcService<PaymentRpcService>();

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
