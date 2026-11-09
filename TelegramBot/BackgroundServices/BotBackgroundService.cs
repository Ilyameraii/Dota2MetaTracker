using Telegram.Bot;

namespace TelegramBot.BackgroundServices;

public class BotBackgroundService(
    ITelegramBotClient botClient,
    IConfiguration configuration,
    ILogger<BotBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Запуск Telegram-бота (webhook)...");

        await botClient.SetWebhook(
            url: configuration["Telegram:WebhookUrl"]!,
            secretToken: configuration["Telegram:SecretToken"]!,
            cancellationToken: stoppingToken);

        logger.LogInformation("Webhook установлен.");

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Бот остановлен.");
        }
    }
}