using TelegramBot;
using Telegram.Bot.Types;

namespace TelegramBot.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapBotEndpoints(this WebApplication app, IConfiguration configuration)
    {
        app.MapPost("/bot", async (
            Update update,
            BotClient botClient,
            HttpRequest request,
            CancellationToken ct) =>
        {
            var secretToken = configuration["Telegram:SecretToken"];

            if (request.Headers["X-Telegram-Bot-Api-Secret-Token"] != secretToken)
                return Results.Unauthorized();

            await botClient.HandleUpdateAsync(update, ct);
            return Results.Ok();
        });

        return app;
    }
}