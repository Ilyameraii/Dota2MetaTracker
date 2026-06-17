using ChatBot.Constants;
using ChatBot.Contracts;
using Telegram.Bot.Types;

namespace ChatBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки перехода на указанную страницу списка.
/// </summary>
public class ToPageCallbackHandler : ICallbackHandler
{
    /// <inheritdoc/>
    public bool CanHandle(string callbackData) => callbackData.StartsWith(CallbackData.Prefixes.ToPage);

    /// <inheritdoc/>
    public Task HandleAsync(CallbackQuery callbackQuery, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}