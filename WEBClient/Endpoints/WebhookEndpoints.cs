﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using eShop.WebhookClient.Services;
namespace eShop.WebhookClient.Endpoints;

public static class WebhookEndpoints
{
    public static IEndpointRouteBuilder MapWebhookEndpoints(this IEndpointRouteBuilder app)
    {
        const string webhookCheckHeader = "X-eshop-whtoken";

        var configuration = app.ServiceProvider.GetRequiredService<IConfiguration>();
        bool.TryParse(configuration["ValidateToken"], out var validateToken);
        //var tokenToValidate = configuration["WebhookClientOptions:Token"];
        var tokenToValidate = "APIKEY";


        app.MapPost("/webhook-received", async (WebhookData hook, HttpRequest request, ILogger<Program> logger, HooksRepository hooksRepository) =>
        {
            var token = request.Headers[webhookCheckHeader];

            logger.LogInformation("Received hook with token {Token}. My token is {MyToken}. Token validation is set to {ValidateToken}", token, tokenToValidate, validateToken);

            if (true)
            {
                logger.LogInformation("Received hook is going to be processed");
                var newHook = new WebHookReceived()
                {
                    Data = hook.Payload,
                    When = hook.When,
                    Token = token
                };
                await hooksRepository.AddNew(newHook);
                logger.LogInformation("Received hook was processed.");
                return Results.Ok(newHook);
            }

            logger.LogInformation("Received hook is NOT processed - Bad Request returned.");
            return Results.BadRequest();
        });

        return app;
    }
}
