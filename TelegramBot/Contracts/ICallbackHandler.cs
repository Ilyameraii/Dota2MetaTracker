using Telegram.Bot.Types;

namespace TelegramBot.Contracts;

/// <summary>
/// Обработчик callback-запросов (нажатий на inline-кнопки) Telegram-бота.
/// </summary>
public interface ICallbackHandler
{
    /// <summary>
    /// Определяет, способен ли данный обработчик обработать указанные callback-данные.
    /// </summary>
    /// <param name="callbackData">
    /// Строка данных callback-запроса, полученная из <see cref="CallbackQuery.Data"/>.
    /// </param>
    bool CanHandle(string callbackData);

    /// <summary>
    /// Выполняет обработку callback-запроса.
    /// </summary>
    /// <param name="callbackQuery">Callback-запрос, полученный от Telegram Bot API.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    Task HandleAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken = default);
}