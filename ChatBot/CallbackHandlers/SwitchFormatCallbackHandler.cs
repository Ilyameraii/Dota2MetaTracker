using ChatBot.Constants;
using ChatBot.Contracts;
using Telegram.Bot.Types;

namespace ChatBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки переключения формата отображения.
/// </summary>
public class SwitchFormatCallbackHandler : ICallbackHandler
{
    /// <inheritdoc/>
    public bool CanHandle(string callbackData) => callbackData.StartsWith(CallbackData.Actions.SwitchFormat);

    /// <inheritdoc/>
    public Task HandleAsync(CallbackQuery callbackQuery, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}