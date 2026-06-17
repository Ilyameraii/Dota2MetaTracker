using ChatBot.Constants;
using ChatBot.Contracts;
using Telegram.Bot.Types;

namespace ChatBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки действия сброса фильтров.
/// </summary>
public class ClearOptionsCallbackHandler : ICallbackHandler
{
    /// <inheritdoc/>
    public bool CanHandle(string callbackData) => callbackData == CallbackData.Actions.ClearOptions;

    /// <inheritdoc/>
    public Task HandleAsync(CallbackQuery callbackQuery, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}