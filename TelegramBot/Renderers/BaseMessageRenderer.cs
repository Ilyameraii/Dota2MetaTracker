using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Renderers;

/// <summary>
/// Базовый класс для рендереров сообщений Telegram-бота.
/// </summary>
public abstract class BaseMessageRenderer(ITelegramBotClient bot)
{
    private const string ImageFileName = "heroes.png";

    /// <summary>
    /// Клиент Telegram Bot API.
    /// </summary>
    private readonly ITelegramBotClient bot = bot;

    /// <summary>
    /// Формирует текст сообщения.
    /// </summary>
    protected virtual string? BuildText() => null;

    /// <summary>
    /// Формирует клавиатуру сообщения.
    /// </summary>
    protected virtual InlineKeyboardMarkup? BuildKeyboard() => null;

    /// <summary>
    /// Формирует изображение сообщения.
    /// </summary>
    protected virtual byte[]? BuildImage() => null;

    /// <summary>
    /// Отправляет текстовое сообщение в чат.
    /// </summary>
    protected Task SendMessageAsync(long chatId, string text, CancellationToken cancellationToken)
        => bot.SendMessage(chatId, text: text, cancellationToken: cancellationToken);

    /// <summary>
    /// Отправляет новое сообщение или редактирует существующее.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="messageId">Идентификатор сообщения для редактирования. Если <c>null</c> — отправляется новое сообщение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task RenderAsync(
        long chatId,
        int? messageId = null,
        CancellationToken cancellationToken = default)
    {
        var image = BuildImage();
        var keyboard = BuildKeyboard();

        if (image is not null)
        {
            await RenderImageAsync(chatId, messageId, image, keyboard, cancellationToken);
            return;
        }

        await RenderTextAsync(chatId, messageId, BuildText() ?? string.Empty, keyboard, cancellationToken);
    }

    private async Task RenderImageAsync(
        long chatId,
        int? messageId,
        byte[] image,
        InlineKeyboardMarkup? keyboard,
        CancellationToken ct)
    {
        if (messageId is not null)
        {
            try
            {
                await using var stream = new MemoryStream(image);

                await bot.EditMessageMedia(
                    chatId,
                    messageId.Value,
                    media: new InputMediaPhoto(InputFile.FromStream(stream, ImageFileName)),
                    replyMarkup: keyboard,
                    cancellationToken: ct);

                return;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                if (IsMessageNotModified(ex))
                    return;

                await TryDeleteAsync(chatId, messageId.Value, ct);
            }
        }

        await using var newStream = new MemoryStream(image);

        await bot.SendPhoto(
            chatId,
            photo: InputFile.FromStream(newStream, ImageFileName),
            replyMarkup: keyboard,
            cancellationToken: ct);
    }

    private async Task RenderTextAsync(
        long chatId,
        int? messageId,
        string text,
        InlineKeyboardMarkup? keyboard,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new InvalidOperationException("Нельзя отрисовать сообщение с пустым текстом.");

        if (messageId is not null)
        {
            try
            {
                await bot.EditMessageText(
                    chatId,
                    messageId: messageId.Value,
                    text: text,
                    replyMarkup: keyboard,
                    parseMode: ParseMode.Html,
                    cancellationToken: ct);

                return;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                if (IsMessageNotModified(ex))
                    return;

                await TryDeleteAsync(chatId, messageId.Value, ct);
            }
        }

        await bot.SendMessage(
            chatId,
            text: text,
            replyMarkup: keyboard,
            parseMode: ParseMode.Html,
            cancellationToken: ct);
    }

    private async Task TryDeleteAsync(long chatId, int messageId, CancellationToken ct)
    {
        try
        {
            await bot.DeleteMessage(chatId, messageId, cancellationToken: ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
        }
    }

    private static bool IsMessageNotModified(Exception exception) =>
        exception is ApiRequestException { ErrorCode: 400 } apiException
        && apiException.Message.Contains("message is not modified", StringComparison.OrdinalIgnoreCase);
}
