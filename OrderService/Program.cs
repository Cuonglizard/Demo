using System.Runtime.InteropServices;
using Common.Protobuf;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IGrpcOrdersService, GrpcOrderService>();

var client = builder.Services.AddGrpcClient<Payments.PaymentsClient>(o => o.Address = new Uri(builder.Configuration.GetSection("Grpc")["PaymentsAddress"]));

if (builder.Environment.IsDevelopment() || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    // NOTE: Normally gRPC should communicate with encripted channel (SSL), however
    // for development (and on Linux) it is easier to not bother with cerificate.
    // On Windows you can just trust the dev certificate and that should be enough.
    client.ConfigurePrimaryHttpMessageHandler(() =>
    {
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        return httpHandler;
    });
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Order",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order V1");
    });
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
