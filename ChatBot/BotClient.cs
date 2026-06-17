using ChatBot.Contracts;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChatBot;

/// <summary>
/// Основной класс Telegram-бота, отвечающий за обработку обновлений, поступающих через webhook.
/// </summary>
public class BotClient(
    IEnumerable<ICommandHandler> commandHandlers,
    IEnumerable<ICallbackHandler> callbackHandlers,
    ILogger<BotClient> logger)
{
    /// <summary>
    /// Обрабатывает одно обновление, пришедшее от Telegram.
    /// </summary>
    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        try
        {
            await (update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(update.Message!, cancellationToken),
                UpdateType.CallbackQuery => HandleCallbackQueryAsync(update.CallbackQuery!, cancellationToken),
                _ => Task.CompletedTask
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка обработки обновления.");
        }
    }

    private async Task HandleCallbackQueryAsync(CallbackQuery updateCallbackQuery, CancellationToken cancellationToken)
    {
        if (updateCallbackQuery.Data is null) return;
        var handler = callbackHandlers.FirstOrDefault(c => c.CanHandle(updateCallbackQuery.Data));
        if (handler is null) return;
        await handler.HandleAsync(updateCallbackQuery, cancellationToken);
    }

    private async Task HandleMessageAsync(Message updateMessage, CancellationToken cancellationToken)
    {
        if (updateMessage.Text is null) return;
        var handler = commandHandlers.FirstOrDefault(c => c.Command == updateMessage.Text);
        if (handler is null) return;
        await handler.HandleAsync(updateMessage, cancellationToken);
    }
}