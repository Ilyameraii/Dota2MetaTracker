using TelegramBot.Contracts;
using TelegramBot.Renderers;
using Repository.Contracts;
using Telegram.Bot.Types;

namespace TelegramBot.CallbackHandlers;

/// <summary>
/// Базовый класс обработчиков callback-запросов: обновляет кэш настроек и перерисовывает сообщение.
/// </summary>
public abstract class CallbackHandlerBase(
    IUserPreferencesCache cache,
    HeroStatsMessageRenderer renderer) : ICallbackHandler
{
    /// <summary>
    /// Кэш настроек пользователей.
    /// </summary>
    protected IUserPreferencesCache PreferencesCache { get; } = cache;

    /// <inheritdoc/>
    public abstract bool CanHandle(string callbackData);

    /// <inheritdoc/>
    public async Task HandleAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken = default)
    {
        if (callbackQuery.Data is null || callbackQuery.Message is null)
            return;

        var chatId = callbackQuery.Message.Chat.Id;

        if (!TryUpdate(chatId, callbackQuery.Data))
            return;

        await renderer.RenderAsync(
            chatId,
            callbackQuery.Message.MessageId,
            PreferencesCache.GetOrCreate(chatId),
            cancellationToken);
    }

    /// <summary>
    /// Обновляет настройки пользователя в кэше, если callback-данные корректны.
    /// </summary>
    protected abstract bool TryUpdate(long userId, string callbackData);

    /// <summary>
    /// Разбирает значение enum из callback-данных.
    /// </summary>
    protected static bool TryParseEnumValue<TEnum>(string callbackData, string prefix, out TEnum value)
        where TEnum : struct, Enum
    {
        value = default;
        return callbackData.StartsWith(prefix)
               && Enum.TryParse(callbackData[prefix.Length..], out value)
               && Enum.IsDefined(value);
    }
}
