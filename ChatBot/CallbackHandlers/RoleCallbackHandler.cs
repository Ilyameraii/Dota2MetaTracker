using ChatBot.Constants;
using ChatBot.Contracts;
using Telegram.Bot.Types;

namespace ChatBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки выбора фильтра по роли.
/// </summary>
public class RoleCallbackHandler : ICallbackHandler
{
    /// <inheritdoc/>
    public bool CanHandle(string callbackData) => callbackData.StartsWith(CallbackData.Prefixes.Role);

    /// <inheritdoc/>
    public Task HandleAsync(CallbackQuery callbackQuery, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}