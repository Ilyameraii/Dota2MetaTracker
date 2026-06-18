using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Renderers;

/// <summary>
/// Базовый класс для рендереров сообщений Telegram-бота.
/// </summary>
public abstract class BaseRenderer(ITelegramBotClient bot)
{
    /// <summary>
    /// Клиент Telegram Bot API.
    /// </summary>
    private readonly ITelegramBotClient bot = bot;

    /// <summary>
    /// Текст сообщения.
    /// </summary>
    protected abstract string Text { get; }

    /// <summary>
    /// Строит inline-клавиатуру сообщения.
    /// </summary>
    /// <returns>Inline-клавиатура.</returns>
    protected abstract InlineKeyboardMarkup BuildKeyboard();
    
    /// <summary>
    /// Отправляет новое сообщение или редактирует существующее.
    /// </summary>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <param name="messageId">Идентификатор сообщения для редактирования. Если <c>null</c> — отправляется новое сообщение.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task RenderAsync(long chatId, int? messageId = null,
        CancellationToken cancellationToken = default)
    {
        if (messageId.HasValue)
        {
            await bot.EditMessageText(chatId,
                messageId: messageId.Value,
                text: Text,
                replyMarkup:  BuildKeyboard(),
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
        else
        {
            await bot.SendMessage(chatId,
                text: Text,
                replyMarkup: BuildKeyboard(),
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }
}