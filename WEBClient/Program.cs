using WEBClient.Components;
using MudBlazor.Services;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapPost("/webhook", async context =>
{

    if (!context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Response.StatusCode = 400;
        return;
    }
    var apiKey = context.Request.Headers["Authorization"];
    if (apiKey != "APIKEY")
    {
        context.Response.StatusCode = 401;
        return;
    }
    var body = await context.Request.ReadFromJsonAsync<WebhookpayLoad>();
    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("Webhook! ack");
});
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public record WebhookpayLoad(string Header, string Body);