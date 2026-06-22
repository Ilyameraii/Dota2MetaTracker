using Telegram.Bot;

namespace Dota2MetaTracker.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SetupTelegramWebhook(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        
        await botClient.SetWebhook(
            url: configuration["Telegram:WebhookUrl"]!,
            secretToken: configuration["Telegram:SecretToken"]!);
    }
}