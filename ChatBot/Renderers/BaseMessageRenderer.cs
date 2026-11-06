using ChatBot.Constants;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Renderers;

/// <summary>
/// Базовый класс для рендереров сообщений Telegram-бота.
/// </summary>
public abstract class BaseMessageRenderer(ITelegramBotClient bot)
{
    /// <summary>
    /// Клиент Telegram Bot API.
    /// </summary>
    private readonly ITelegramBotClient bot = bot;

    // Наследники определяют ЧТО показывать
    protected virtual string? BuildText() => null;

    protected virtual InlineKeyboardMarkup? BuildKeyboard() => null;

    protected virtual byte[]? BuildImage() => null;

    /// <summary>
    /// Отправляет новое сообщение или редактирует существующее.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="messageId">Идентификатор сообщения для редактирования. Если <c>null</c> — отправляется новое сообщение.</param>
    /// 
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task RenderAsync(
        long chatId,
        int? messageId = null,
        CancellationToken cancellationToken = default)
    {
        if (messageId.HasValue)
        {
            await EditAsync(chatId, messageId.Value, cancellationToken);
        }
        else
            await SendAsync(chatId, cancellationToken);
    }

    private async Task SendAsync(long chatId, CancellationToken ct)
    {
        var image = BuildImage();

        if (image is not null)
        {
            await using var stream = new MemoryStream(image);
            await bot.SendPhoto(chatId,
                photo: stream,
                replyMarkup: BuildKeyboard(),
                parseMode: ParseMode.Html,
                cancellationToken: ct);
        }
        else
        {
            await bot.SendMessage(chatId,
                text: BuildText() ?? string.Empty,
                replyMarkup: BuildKeyboard(),
                parseMode: ParseMode.Html,
                cancellationToken: ct);
        }
    }

    private async Task EditAsync(long chatId, int messageId, CancellationToken ct)
    {
        await bot.EditMessageText(chatId,
            messageId: messageId,
            text: BuildText() ?? string.Empty,
            replyMarkup: BuildKeyboard(),
            parseMode: ParseMode.Html,
            cancellationToken: ct);
    }
}