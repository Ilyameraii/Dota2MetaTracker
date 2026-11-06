using ChatBot.Constants;
using Telegram.Bot;
using Telegram.Bot.Types;
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
        var image = BuildImage();
        var text = BuildText();
        var keyboard = BuildKeyboard();

        if (messageId is null)
        {
            await SendAsync(chatId, image, text, keyboard, cancellationToken);
            return;
        }

        await EditAsync(chatId, messageId.Value, image, text, keyboard, cancellationToken);
    }

    private async Task SendAsync(
        long chatId,
        byte[]? image,
        string? text,
        InlineKeyboardMarkup? keyboard,
        CancellationToken ct)
    {
        if (image is not null)
        {
            await using var stream = new MemoryStream(image);

            await bot.SendPhoto(
                chatId,
                photo: InputFile.FromStream(stream, "heroes.png"),
                replyMarkup: keyboard,
                cancellationToken: ct);
        }
        else
        {
            await bot.SendMessage(
                chatId,
                text: text ?? string.Empty,
                replyMarkup: keyboard,
                parseMode: ParseMode.Html,
                cancellationToken: ct);
        }
    }

    private async Task EditAsync(
        long chatId,
        int messageId,
        byte[]? image,
        string? text,
        InlineKeyboardMarkup? keyboard,
        CancellationToken ct)
    {
        if (image is not null)
        {
            await using var stream = new MemoryStream(image);

            await bot.EditMessageMedia(
                chatId,
                messageId,
                media: new InputMediaPhoto(InputFile.FromStream(stream, "heroes.png")),
                replyMarkup: keyboard,
                cancellationToken: ct);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidOperationException(
                    "Нельзя редактировать текстовое сообщение пустым текстом.");

            await bot.EditMessageText(
                chatId,
                messageId: messageId,
                text: text,
                replyMarkup: keyboard,
                parseMode: ParseMode.Html,
                cancellationToken: ct);
        }
    }
}