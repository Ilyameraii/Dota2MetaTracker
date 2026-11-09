using TelegramBot.Constants;
using TelegramBot.Contracts;
using TelegramBot.Renderers;
using Repository.Contracts;

namespace TelegramBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки действия сброса фильтров.
/// </summary>
public class ClearOptionsCallbackHandler(
    IUserPreferencesCache cache,
    HeroStatsMessageRenderer renderer) : CallbackHandlerBase(cache, renderer)
{
    /// <inheritdoc/>
    public override bool CanHandle(string callbackData) => callbackData == CallbackData.Actions.ClearOptions;

    /// <inheritdoc/>
    protected override bool TryUpdate(long userId, string callbackData)
    {
        PreferencesCache.Reset(userId);
        return true;
    }
}
